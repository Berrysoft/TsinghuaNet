#include "pch.h"

#include "ChangeUserDialog.h"
#include "EditSuggestionDialog.h"
#include "FluxUserBox.h"
#include "MainPage.h"
#include "NetStateSsidBox.h"
#include "UserContentHelper.h"
#include <winrt/Windows.ApplicationModel.Core.h>
#include <winrt/Windows.UI.Core.h>
#include <winrt/Windows.UI.ViewManagement.h>

using namespace std::chrono_literals;
using sf::sprint;
using namespace linq;
using namespace concurrency;
using namespace winrt;
using namespace Windows::ApplicationModel::Core;
using namespace Windows::Foundation;
using namespace Windows::Networking::Connectivity;
using namespace Windows::UI;
using namespace Windows::UI::Core;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::ViewManagement;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    MainPage::MainPage() : m_ToastLogined(false)
    {
        InitializeComponent();
        // 调整标题栏的颜色为透明
        // 按钮的背景色为透明
        auto titleBar{ ApplicationView::GetForCurrentView().TitleBar() };
        titleBar.BackgroundColor(Colors::Transparent());
        titleBar.ButtonBackgroundColor(Colors::Transparent());
        titleBar.ButtonInactiveBackgroundColor(Colors::Transparent());
        // 按钮的前景色根据主题调节
        ThemeChangedImpl();
        // 将用户区拓展到全窗口
        auto viewTitleBar{ CoreApplication::GetCurrentView().TitleBar() };
        viewTitleBar.ExtendViewIntoTitleBar(true);
        // 设置主窗格为标题栏
        Window::Current().SetTitleBar(MainFrame());
        // 获取用户设置的主题
        Model().Theme(settings.Theme());
        Model().ContentType(settings.ContentType());
        // 设置计时器
        mainTimer.Interval(1s);
        mainTimer.Tick({ this, &MainPage::MainTimerTick });
        // 监视网络情况变化
        networkListener.NetworkStatusChanged({ this, &MainPage::NetworkChanged });
    }

    /// <summary>
    /// 保存设置
    /// </summary>
    void MainPage::SaveSettings()
    {
        settings.Theme(Model().Theme());
        settings.ContentType(Model().ContentType());
        settings.SaveSettings();
    }

    /// <summary>
    /// 页面装载时触发
    /// </summary>
    fire_and_forget MainPage::PageLoaded(IInspectable const, RoutedEventArgs const)
    {
        // 先刷新状态
        RefreshStatusImpl();
        // 自动登录
        bool al{ settings.AutoLogin() };
        Model().AutoLogin(al);
        // 后台任务
        bool bal{ settings.BackgroundAutoLogin() };
        Model().BackgroundAutoLogin(bal);
        bool blt{ settings.BackgroundLiveTile() };
        Model().BackgroundLiveTile(blt);
        // 调整后台任务
        if (co_await BackgroundHelper::RequestAccessAsync())
        {
            BackgroundHelper::RegisterLogin(bal);
            BackgroundHelper::RegisterLiveTile(blt);
        }
        // 上一次登录的用户名
        hstring un{ settings.StoredUsername() };
        if (!un.empty())
        {
            // 设置为当前用户名并获取密码
            Model().Username(un);
            hstring pw{ CredentialHelper::GetCredential(un) };
            Model().Password(pw);
            // 自动登录的条件为：
            // 打开了自动登录
            // 不知道后台任务成功登录
            // 密码不为空
            if (al && !m_ToastLogined && !pw.empty())
            {
                co_await LoginImpl();
            }
            else
            {
                co_await RefreshImpl();
            }
            // 刷新当前用户所有连接状态
            co_await RefreshNetUsersImpl();
        }
    }

    /// <summary>
    /// 调用Dispatcher刷新网络状态
    /// </summary>
    fire_and_forget MainPage::NetworkChanged(IInspectable const)
    {
        co_await Dispatcher().RunAsync(CoreDispatcherPriority::Normal, { this, &MainPage::NetworkChangedImpl });
    }

    /// <summary>
    /// 打开“更改用户”对话框
    /// </summary>
    fire_and_forget MainPage::ShowChangeUser(IInspectable const, RoutedEventArgs const)
    {
        auto dialog{ make<ChangeUserDialog>(Model().Username()) };
        dialog.RequestedTheme(Model().Theme());
        // 显示对话框
        auto result{ co_await dialog.ShowAsync() };
        // 确定
        if (result == ContentDialogResult::Primary)
        {
            hstring un{ dialog.UnBox().Text() };
            hstring pw{ dialog.PwBox().Password() };
            // 不管是否保存，都需要先删除
            CredentialHelper::RemoveCredential(un);
            if (dialog.SaveBox().IsChecked().Value())
            {
                CredentialHelper::SaveCredential(un, pw);
            }
            // 同步
            settings.StoredUsername(un);
            Model().Username(un);
            Model().Password(pw);
            // 关闭设置栏并登录
            Split().IsPaneOpen(false);
            LoginImpl();
        }
    }

    /// <summary>
    /// 根据主题调节标题栏按钮前景色
    /// </summary>
    void MainPage::ThemeChangedImpl()
    {
        auto titleBar{ ApplicationView::GetForCurrentView().TitleBar() };
        switch (ActualTheme())
        {
        case ElementTheme::Light:
            titleBar.ButtonForegroundColor(Colors::Black());
            break;
        case ElementTheme::Dark:
            titleBar.ButtonForegroundColor(Colors::White());
            break;
        }
    }

    /// <summary>
    /// 刷新网络状态
    /// </summary>
    task<void> MainPage::NetworkChangedImpl()
    {
        RefreshStatusImpl();
        if (Model().AutoLogin() && !Model().Password().empty())
        {
            co_await LoginImpl();
        }
        else
        {
            co_await RefreshImpl();
        }
        co_await RefreshNetUsersImpl();
    }

    /// <summary>
    /// 登录当前用户并刷新
    /// </summary>
    task<void> MainPage::LoginImpl()
    {
        UserContentManager ring(Model().UserContent().try_as<IUserContent>());
        try
        {
            auto helper{ GetHelper() };
            if (helper)
            {
                ShowResponse(co_await helper.LoginAsync());
            }
            co_await RefreshImpl(helper);
        }
        catch (hresult_error const& e)
        {
            ShowHresultError(e);
        }
    }

    /// <summary>
    /// 注销当前用户并刷新
    /// </summary>
    task<void> MainPage::LogoutImpl()
    {
        UserContentManager ring(Model().UserContent().try_as<IUserContent>());
        try
        {
            auto helper{ GetHelper() };
            if (helper)
            {
                ShowResponse(co_await helper.LogoutAsync());
            }
            co_await RefreshImpl(helper);
        }
        catch (hresult_error const& e)
        {
            ShowHresultError(e);
        }
    }

    /// <summary>
    /// 刷新
    /// </summary>
    task<void> MainPage::RefreshImpl()
    {
        UserContentManager ring(Model().UserContent().try_as<IUserContent>());
        try
        {
            auto helper{ GetHelper() };
            co_await RefreshImpl(helper);
        }
        catch (hresult_error const& e)
        {
            ShowHresultError(e);
        }
    }

    /// <summary>
    /// 具体的刷新操作
    /// </summary>
    /// <param name="helper">网络连接辅助类，用于执行刷新任务</param>
    task<void> MainPage::RefreshImpl(IConnect const helper)
    {
        FluxUser flux{};
        if (helper)
        {
            flux = co_await helper.FluxAsync();
        }
        // 更新磁贴
        NotificationHelper::UpdateTile(flux);
        // 设置内容
        auto content{ Model().UserContent().try_as<IUserContent>() };
        content.User(make<FluxUserBox>(flux));
        content.BeginAnimation();
        mainTimer.Start();
    }

    /// <summary>
    /// 根据IP强制下线某个连接
    /// </summary>
    /// <param name="address">连接的IP地址</param>
    task<void> MainPage::DropImpl(hstring const address)
    {
        try
        {
            UseregHelper helper;
            helper.Username(Model().Username());
            helper.Password(Model().Password());
            co_await helper.LoginAsync();
            co_await helper.LogoutAsync(address);
            co_await RefreshNetUsersImpl(helper);
        }
        catch (hresult_error const& e)
        {
            ShowHresultError(e);
        }
    }

    /// <summary>
    /// 根据当前类型、用户名与密码实例化辅助类
    /// </summary>
    IConnect MainPage::GetHelper()
    {
        return ConnectHelper::GetHelper(Model().State(), Model().Username(), Model().Password());
    }

    fire_and_forget MainPage::ShowResponse(LogResponse const& response)
    {
        Model().Response(response);
        ResponseFlyout().ShowAt(MainBar());
        co_await 3s;
        Dispatcher().RunAsync(CoreDispatcherPriority::Normal,
                              [this]() { ResponseFlyout().Hide(); });
    }

    void MainPage::ShowHresultError(hresult_error const& e)
    {
        ShowResponse({ hstring(sprint(L"异常 0x{:x8,u}", e.code())), e.message() });
    }

    void MainPage::MainTimerTickImpl()
    {
        auto content{ Model().UserContent().try_as<IUserContent>() };
        if (!content.AddOneSecond())
            mainTimer.Stop();
    }

    /// <summary>
    /// 打开“编辑建议”对话框
    /// </summary>
    fire_and_forget MainPage::ShowEditSuggestion(IInspectable const, RoutedEventArgs const)
    {
        auto dialog{ make<EditSuggestionDialog>() };
        dialog.RequestedTheme(Model().Theme());
        dialog.LanCombo().Value(settings.LanState());
        dialog.WwanCombo().Value(settings.WwanState());
        auto s{ settings.WlanStates() };
        dialog.RefreshWlanList(s);
        auto result{ co_await dialog.ShowAsync() };
        if (result == ContentDialogResult::Primary)
        {
            settings.LanState(dialog.LanCombo().Value());
            settings.WwanState(dialog.WwanCombo().Value());
            s.Clear();
            for (auto item :
                 dialog.WlanList() >>
                     select([](auto pair) { return pair.try_as<TsinghuaNetUWP::NetStateSsidBox>(); }))
            {
                s.Insert(item.Ssid(), item.Value());
            }
            settings.WlanStates(s);
            RefreshStatusImpl();
        }
    }

    void MainPage::AutoLoginChanged(IInspectable const&, bool const& e)
    {
        settings.AutoLogin(e);
    }

    fire_and_forget MainPage::BackgroundAutoLoginChanged(IInspectable const, bool const e)
    {
        settings.BackgroundAutoLogin(e);
        if (co_await BackgroundHelper::RequestAccessAsync())
        {
            BackgroundHelper::RegisterLogin(e);
        }
    }

    fire_and_forget MainPage::BackgroundLiveTileChanged(IInspectable const, bool const e)
    {
        settings.BackgroundLiveTile(e);
        if (co_await BackgroundHelper::RequestAccessAsync())
        {
            BackgroundHelper::RegisterLiveTile(e);
        }
    }

    fire_and_forget MainPage::ContentTypeChanged(IInspectable const, UserContentType const)
    {
        co_await RefreshImpl();
    }

    /// <summary>
    /// 根据网络类型与SSID判断建议网络类型
    /// </summary>
    void MainPage::RefreshStatusImpl()
    {
        hstring ssid;
        auto status{ SettingsHelper::InternetStatus(ssid) };
        NetState state{ settings.SuggestNetState(status, ssid) };
        Model().NetStatus(status);
        Model().Ssid(ssid);
        Model().SuggestState(state);
        Model().State(state);
    }

    /// <summary>
    /// 刷新所有连接情况
    /// </summary>
    task<void> MainPage::RefreshNetUsersImpl()
    {
        try
        {
            if ((int)Model().State())
            {
                UseregHelper helper;
                helper.Username(Model().Username());
                helper.Password(Model().Password());
                co_await helper.LoginAsync();
                co_await RefreshNetUsersImpl(helper);
            }
        }
        catch (hresult_error const& e)
        {
            ShowHresultError(e);
        }
    }

    /// <summary>
    /// 使用给定的帮助类刷新所有连接情况。
    /// 在调用这个方法前要调用<see cref="UseregHelper.LoginAsync"/>。
    /// </summary>
    /// <param name="helper">帮助类实例</param>
    task<void> MainPage::RefreshNetUsersImpl(UseregHelper const helper)
    {
        auto users{ co_await helper.UsersAsync() };
        auto usersmodel{ Model().NetUsers() };
        for (uint32_t i{ 0 }; i < usersmodel.Size(); i++)
        {
            if (auto olduser{ usersmodel.GetAt(i).try_as<NetUser>() })
            {
                // 循环判断旧元素是否存在于新集合中
                for (uint32_t j{ 0 }; j < users.Size(); j++)
                {
                    // 如果存在则移除新元素
                    if (users.GetAt(j).Equals(olduser))
                    {
                        users.RemoveAt(j);
                        goto continue_outer;
                    }
                }
                // 反之移除旧元素
                usersmodel.RemoveAt(i);
                i--;
            }
        continue_outer:;
        }
        // 最后添加新增元素
        for (auto user : users)
        {
            user.DropUser({ this, &MainPage::DropUser });
            usersmodel.Append(user);
        }
    }
} // namespace winrt::TsinghuaNetUWP::implementation

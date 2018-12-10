#include "pch.h"

#include "MainPage.h"

#include "ChangeUserDialog.h"
#include "EditSuggestionDialog.h"
#include <winrt/Windows.ApplicationModel.Core.h>
#include <winrt/Windows.UI.ViewManagement.h>

using namespace winrt;
using namespace Windows::ApplicationModel::Core;
using namespace Windows::Foundation;
using namespace Windows::UI;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::ViewManagement;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    MainPage::MainPage()
    {
        InitializeComponent();
        // 调整标题栏的颜色为透明
        // 按钮的背景色为透明
        auto titleBar = ApplicationView::GetForCurrentView().TitleBar();
        titleBar.BackgroundColor(Colors::Transparent());
        titleBar.ButtonBackgroundColor(Colors::Transparent());
        titleBar.ButtonInactiveBackgroundColor(Colors::Transparent());
        // 按钮的前景色根据主题调节
        ThemeChangedImpl();
        // 将用户区拓展到全窗口
        auto viewTitleBar = CoreApplication::GetCurrentView().TitleBar();
        viewTitleBar.ExtendViewIntoTitleBar(true);
    }

    /// <summary>
    /// 页面装载时触发
    /// </summary>
    IAsyncAction MainPage::PageLoaded(IInspectable const, RoutedEventArgs const)
    {
        // 先刷新状态
        RefreshStatusImpl();
        // 设置当前类型为建议类型
        NetState state = Model().SuggestState();
        Model().State(state);
        // 自动登录
        bool al = settings.AutoLogin();
        Model().AutoLogin(al);
        // 后台任务
        bool bal = settings.BackgroundAutoLogin();
        Model().BackgroundAutoLogin(bal);
        bool blt = settings.BackgroundLiveTile();
        Model().BackgroundLiveTile(blt);
        // 调整后台任务
        if (co_await BackgroundHelper::RequestAccessAsync())
        {
            BackgroundHelper::RegisterLogin(bal);
            BackgroundHelper::RegisterLiveTile(blt);
        }
        // 上一次登录的用户名
        hstring un = settings.StoredUsername();
        if (!un.empty())
        {
            // 设置为当前用户名并获取密码
            Model().Username(un);
            hstring pw = CredentialHelper::GetCredential(un);
            Model().Password(pw);
            // 自动登录的条件为：
            // 打开了自动登录
            // 不知道后台任务成功登录
            // 无Internet访问
            // 密码不为空
            if (al && !m_ToastLogined && !settings.InternetAvailable() && !pw.empty())
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

    void MainPage::ThemeChanged(IFrameworkElement const&, IInspectable const&)
    {
        ThemeChangedImpl();
    }

    void MainPage::OpenSettings(IInspectable const&, RoutedEventArgs const&)
    {
        Split().IsPaneOpen(true);
    }

    IAsyncAction MainPage::Login(IInspectable const, RoutedEventArgs const)
    {
        return LoginImpl();
    }

    IAsyncAction MainPage::Logout(IInspectable const, RoutedEventArgs const)
    {
        return LogoutImpl();
    }

    IAsyncAction MainPage::Refresh(IInspectable const, RoutedEventArgs const)
    {
        return RefreshImpl();
    }

    IAsyncAction MainPage::DropUser(IInspectable const, hstring const e)
    {
        return DropImpl(e);
    }

    /// <summary>
    /// 打开“更改用户”对话框
    /// </summary>
    IAsyncAction MainPage::ShowChangeUser(IInspectable const, RoutedEventArgs const)
    {
        auto dialog = make<ChangeUserDialog>(Model().Username());
        // 显示对话框
        auto result = co_await dialog.ShowAsync();
        // 确定
        if (result == ContentDialogResult::Primary)
        {
            hstring un = dialog.UnBox().Text();
            hstring pw = dialog.PwBox().Password();
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
        auto titleBar = ApplicationView::GetForCurrentView().TitleBar();
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
    /// 一个帮助类，管理<see cref="Windows.UI.Xaml.Controls.ProgressRing"/>的活动状态
    /// </summary>
    class ProgressRingManager
    {
    private:
        ProgressRing ring;

    public:
        ProgressRingManager(ProgressRing const& ring) : ring(ring) { ring.IsActive(true); }
        ~ProgressRingManager() { ring.IsActive(false); }
    };

    /// <summary>
    /// 登录当前用户并刷新
    /// </summary>
    IAsyncAction MainPage::LoginImpl()
    {
        ProgressRingManager ring(Progress());
        try
        {
            auto helper = GetHelper();
            if (helper)
            {
                co_await helper.LoginAsync();
                co_await RefreshImpl(helper);
            }
        }
        catch (hresult_error const&)
        {
        }
    }

    /// <summary>
    /// 注销当前用户并刷新
    /// </summary>
    IAsyncAction MainPage::LogoutImpl()
    {
        ProgressRingManager ring(Progress());
        try
        {
            auto helper = GetHelper();
            if (helper)
            {
                co_await helper.LogoutAsync();
                co_await RefreshImpl(helper);
            }
        }
        catch (hresult_error const&)
        {
        }
    }

    /// <summary>
    /// 刷新
    /// </summary>
    IAsyncAction MainPage::RefreshImpl()
    {
        ProgressRingManager ring(Progress());
        try
        {
            auto helper = GetHelper();
            if (helper)
            {
                co_await RefreshImpl(helper);
            }
        }
        catch (hresult_error const&)
        {
        }
    }

    /// <summary>
    /// 免费流量25G
    /// </summary>
    constexpr uint64_t BaseFlux = 25000000000;
    /// <summary>
    /// 具体的刷新操作
    /// </summary>
    /// <param name="helper">网络连接辅助类，用于执行刷新任务</param>
    IAsyncAction MainPage::RefreshImpl(IConnect const helper)
    {
        auto flux = co_await helper.FluxAsync();
        // 更新磁贴
        NotificationHelper::UpdateTile(flux);
        // 更新窗口信息
        Model().OnlineUser(flux.Username());
        Model().Flux(flux.Flux());
        Model().OnlineTime(flux.OnlineTime());
        Model().Balance(flux.Balance());
        // 动画
        double maxf = (double)UserHelper::GetMaxFlux(flux);
        Model().FluxPercent(flux.Flux() / maxf);
        Model().FreePercent(BaseFlux / maxf);
        FluxStoryboard().Begin();
    }

    /// <summary>
    /// 根据IP强制下线某个连接
    /// </summary>
    /// <param name="address">连接的IP地址</param>
    IAsyncAction MainPage::DropImpl(hstring const address)
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
        catch (hresult_error const&)
        {
        }
    }

    /// <summary>
    /// 根据当前类型、用户名与密码实例化辅助类
    /// </summary>
    IConnect MainPage::GetHelper()
    {
        return ConnectHelper::GetHelper(Model().State(), Model().Username(), Model().Password());
    }

    void MainPage::StateChanged(IInspectable const&, NetState const& e)
    {
        switch (e)
        {
        case NetState::Auth4:
            Auth4Radio().IsChecked(true);
            break;
        case NetState::Auth6:
            Auth6Radio().IsChecked(true);
            break;
        case NetState::Net:
            NetRadio().IsChecked(true);
            break;
        case NetState::Auth4_25:
            Auth425Radio().IsChecked(true);
            break;
        case NetState::Auth6_25:
            Auth625Radio().IsChecked(true);
            break;
        }
    }
    void MainPage::Auth4Checked(IInspectable const&, RoutedEventArgs const&)
    {
        Model().State(NetState::Auth4);
    }
    void MainPage::Auth6Checked(IInspectable const&, RoutedEventArgs const&)
    {
        Model().State(NetState::Auth6);
    }
    void MainPage::NetChecked(IInspectable const&, RoutedEventArgs const&)
    {
        Model().State(NetState::Net);
    }
    void MainPage::Auth425Checked(IInspectable const&, RoutedEventArgs const&)
    {
        Model().State(NetState::Auth4_25);
    }
    void MainPage::Auth625Checked(IInspectable const&, RoutedEventArgs const&)
    {
        Model().State(NetState::Auth6_25);
    }

    void MainPage::RefreshStatus(IInspectable const&, RoutedEventArgs const&)
    {
        RefreshStatusImpl();
    }

    /// <summary>
    /// 打开“编辑建议”对话框
    /// </summary>
    IAsyncAction MainPage::ShowEditSuggestion(IInspectable const, RoutedEventArgs const)
    {
        auto dialog = make<EditSuggestionDialog>();
        dialog.LanCombo().SelectedIndex((int)settings.LanState());
        dialog.WwanCombo().SelectedIndex((int)settings.WwanState());
        auto result = co_await dialog.ShowAsync();
        if (result == ContentDialogResult::Primary)
        {
            settings.LanState((NetState)dialog.LanCombo().SelectedIndex());
            settings.WwanState((NetState)dialog.WwanCombo().SelectedIndex());
            RefreshStatusImpl();
        }
    }

    IAsyncAction MainPage::RefreshNetUsers(IInspectable const, RoutedEventArgs const)
    {
        return RefreshNetUsersImpl();
    }

    void MainPage::AutoLoginChanged(IInspectable const&, bool const& e)
    {
        settings.AutoLogin(e);
    }

    IAsyncAction MainPage::BackgroundAutoLoginChanged(IInspectable const, bool const e)
    {
        settings.BackgroundAutoLogin(e);
        if (co_await BackgroundHelper::RequestAccessAsync())
        {
            BackgroundHelper::RegisterLogin(e);
        }
    }

    IAsyncAction MainPage::BackgroundLiveTileChanged(IInspectable const, bool const e)
    {
        settings.BackgroundLiveTile(e);
        if (co_await BackgroundHelper::RequestAccessAsync())
        {
            BackgroundHelper::RegisterLiveTile(e);
        }
    }

    /// <summary>
    /// 根据网络类型与SSID判断建议网络类型
    /// </summary>
    void MainPage::RefreshStatusImpl()
    {
        NetState state;
        hstring ssid;
        auto status = settings.InternetStatus(ssid);
        switch (status)
        {
        case InternetStatus::Lan:
            state = settings.LanState();
            break;
        case InternetStatus::Wwan:
            state = settings.WwanState();
            break;
        case InternetStatus::Wlan:
            state = settings.WlanState(ssid);
            break;
        default:
            state = NetState::Unknown;
            break;
        }
        Model().NetStatus(status);
        Model().Ssid(ssid);
        Model().SuggestState(state);
    }

    IAsyncAction MainPage::RefreshNetUsersImpl()
    {
        try
        {
            UseregHelper helper;
            helper.Username(Model().Username());
            helper.Password(Model().Password());
            co_await helper.LoginAsync();
            co_await RefreshNetUsersImpl(helper);
        }
        catch (hresult_error const&)
        {
        }
    }
    IAsyncAction MainPage::RefreshNetUsersImpl(UseregHelper const helper)
    {
        auto users = co_await helper.UsersAsync();
        auto usersmodel = Model().NetUsers();
        usersmodel.Clear();
        for (auto user : users)
        {
            usersmodel.Append(user);
        }
    }
} // namespace winrt::TsinghuaNetUWP::implementation

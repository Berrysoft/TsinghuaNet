#include "pch.h"

#include "MainPage.h"

#include "ChangeUserDialog.h"
#include "LanHelper.h"
#include "NotificationHelper.h"
#include "SettingsHelper.h"
#include <cmath>
#include <pplawait.h>
#include <winrt/Windows.ApplicationModel.Core.h>
#include <winrt/Windows.System.h>
#include <winrt/Windows.UI.ViewManagement.h>

using namespace std;
using namespace concurrency;
using namespace winrt;
using namespace Windows::ApplicationModel::Core;
using namespace Windows::Foundation;
using namespace Windows::UI;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::ViewManagement;
using namespace Windows::Storage;
using namespace Windows::System;

namespace winrt::TsinghuaNetUWP::implementation
{
    MainPage::MainPage()
    {
        InitializeComponent();
        auto titleBar = ApplicationView::GetForCurrentView().TitleBar();
        titleBar.BackgroundColor(Colors::Transparent());
        titleBar.ButtonBackgroundColor(Colors::Transparent());
        titleBar.ButtonInactiveBackgroundColor(Colors::Transparent());
        auto viewTitleBar = CoreApplication::GetCurrentView().TitleBar();
        viewTitleBar.ExtendViewIntoTitleBar(true);
    }

    IAsyncAction MainPage::PageLoaded(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        co_await LoadStates();
        co_await LoadTileTemplate();
        RefreshStatusImpl();
        NetState state = Model().SuggestState();
        Model().State(state);
        bool al = AutoLogin();
        Model().AutoLogin(al);
        hstring un = StoredUsername();
        if (!un.empty())
        {
            Model().Username(un);
            hstring pw = GetCredential(un);
            Model().Password(pw);
            if (al && state != NetState::Unknown && state != NetState::Direct && !pw.empty())
            {
                co_await LoginImpl();
            }
            else
            {
                co_await RefreshImpl();
            }
        }
    }

    void MainPage::OpenSettings(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        Split().IsPaneOpen(true);
    }

    IAsyncAction MainPage::Login(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        return LoginImpl();
    }

    IAsyncAction MainPage::Logout(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        return LogoutImpl();
    }

    IAsyncAction MainPage::Refresh(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        return RefreshImpl();
    }

    IAsyncAction MainPage::DropUser(IInspectable const& /*sender*/, hstring const& e)
    {
        return DropImpl(wstring(e));
    }

    IAsyncAction MainPage::ShowChangeUser(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        auto dialog = make<ChangeUserDialog>();
        hstring oldun = Model().Username();
        dialog.UnBox().Text(oldun);
        hstring oldpw = GetCredential(oldun);
        dialog.PwBox().Password(oldpw);
        if (!oldpw.empty())
        {
            dialog.SaveBox().IsChecked(true);
        }
        dialog.UnBox().TextChanged([&dialog](IInspectable const&, TextChangedEventArgs const&) {
            dialog.PwBox().Password(GetCredential(dialog.UnBox().Text()));
        });
        auto result = co_await dialog.ShowAsync();
        if (result == ContentDialogResult::Primary)
        {
            hstring un = dialog.UnBox().Text();
            hstring pw = dialog.PwBox().Password();
            RemoveCredential(un);
            if (dialog.SaveBox().IsChecked().Value())
            {
                SaveCredential(un, pw);
            }
            StoredUsername(un);
            Model().Username(un);
            Model().Password(pw);
            Split().IsPaneOpen(false);
            LoginImpl();
        }
    }

    class ProgressRingManager
    {
    private:
        ProgressRing ring;

    public:
        ProgressRingManager(ProgressRing ring) : ring(ring) { ring.IsActive(true); }
        ~ProgressRingManager() { ring.IsActive(false); }
    };

    IAsyncAction MainPage::LoginImpl()
    {
        ProgressRingManager ring(Progress());
        try
        {
            auto helper = GetHelper();
            if (helper)
            {
                co_await helper->LoginAsync();
                co_await RefreshImpl(*helper);
            }
        }
        catch (hresult_error const&)
        {
        }
    }
    IAsyncAction MainPage::LogoutImpl()
    {
        ProgressRingManager ring(Progress());
        try
        {
            auto helper = GetHelper();
            if (helper)
            {
                co_await helper->LogoutAsync();
                co_await RefreshImpl(*helper);
            }
        }
        catch (hresult_error const&)
        {
        }
    }

    IAsyncAction MainPage::RefreshImpl()
    {
        ProgressRingManager ring(Progress());
        try
        {
            auto helper = GetHelper();
            if (helper)
            {
                co_await RefreshImpl(*helper);
            }
        }
        catch (hresult_error const&)
        {
        }
    }
    IAsyncAction MainPage::RefreshImpl(IConnect const& helper)
    {
        auto flux = co_await helper.FluxAsync();
        UpdateTile(flux);
        Model().OnlineUser(flux.username);
        Model().Flux(flux.flux);
        Model().OnlineTime(flux.online_time);
        Model().Balance(flux.balance);
        double maxf = (double)GetMaxFlux(flux);
        Model().FluxPercent(flux.flux / maxf);
        Model().FreePercent(BaseFlux / maxf);
        FluxStoryboard().Begin();
    }

    IAsyncAction MainPage::DropImpl(wstring address)
    {
        try
        {
            UseregHelper helper;
            helper.username = Model().Username();
            helper.password = Model().Password();
            co_await helper.LoginAsync();
            co_await helper.LogoutAsync(address);
            co_await RefreshNetUsersImpl(helper);
        }
        catch (hresult_error const&)
        {
        }
    }

    template <typename T>
    unique_ptr<T> MakeHelper(hstring username, hstring password)
    {
        unique_ptr<T> result = make_unique<T>();
        result->username = username;
        result->password = password;
        return result;
    }

    unique_ptr<IConnect> MainPage::GetHelper()
    {
        hstring un = Model().Username();
        hstring pw = Model().Password();
        switch (Model().State())
        {
        case NetState::Auth4:
            return MakeHelper<Auth4Helper>(un, pw);
        case NetState::Auth6:
            return MakeHelper<Auth6Helper>(un, pw);
        case NetState::Net:
            return MakeHelper<NetHelper>(un, pw);
        default:
            return nullptr;
        }
    }

    void MainPage::StateChanged(IInspectable const& /*sender*/, NetState const& e)
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
        }
    }
    void MainPage::Auth4Checked(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        Model().SyncState(NetState::Auth4);
    }
    void MainPage::Auth6Checked(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        Model().SyncState(NetState::Auth6);
    }
    void MainPage::NetChecked(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        Model().SyncState(NetState::Net);
    }

    void MainPage::RefreshStatus(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        RefreshStatusImpl();
    }

    IAsyncAction MainPage::ShowEditSuggestion(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        co_await Launcher::LaunchFileAsync(co_await StorageFile::GetFileFromApplicationUriAsync(Uri(L"ms-appx:///states.json")));
    }

    IAsyncAction MainPage::RefreshNetUsers(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        return RefreshNetUsersImpl();
    }

    void MainPage::AutoLoginChanged(IInspectable const& /*sender*/, optional<bool> const& e)
    {
        AutoLogin(e.Value());
    }

    void MainPage::RefreshStatusImpl()
    {
        NetState state;
        auto status = GetCurrentInternetStatus();
        switch (get<0>(status))
        {
        case InternetStatus::Lan:
            state = GetLanState();
            break;
        case InternetStatus::Wwan:
            state = GetWwanState();
            break;
        case InternetStatus::Wlan:
            state = GetWlanState(get<1>(status));
            break;
        default:
            state = NetState::Unknown;
            break;
        }
        Model().NetStatus(get<0>(status));
        Model().Ssid(get<1>(status));
        Model().SuggestState(state);
    }

    IAsyncAction MainPage::RefreshNetUsersImpl()
    {
        try
        {
            UseregHelper helper;
            helper.username = Model().Username();
            helper.password = Model().Password();
            co_await RefreshNetUsersImpl(helper);
        }
        catch (hresult_error const&)
        {
        }
    }
    IAsyncAction MainPage::RefreshNetUsersImpl(UseregHelper const& helper)
    {
        co_await helper.LoginAsync();
        auto users = co_await helper.UsersAsync();
        auto usersmodel = Model().NetUsers();
        usersmodel.Clear();
        for (auto& user : users)
        {
            auto u = make<NetUserModel>();
            u.Address(hstring(user.address));
            u.LoginTime(hstring(user.login_time));
            u.Client(hstring(user.client));
            usersmodel.Append(u);
        }
    }
} // namespace winrt::TsinghuaNetUWP::implementation

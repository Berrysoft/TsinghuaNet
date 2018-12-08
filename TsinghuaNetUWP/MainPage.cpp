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
        auto titleBar = ApplicationView::GetForCurrentView().TitleBar();
        titleBar.BackgroundColor(Colors::Transparent());
        titleBar.ButtonBackgroundColor(Colors::Transparent());
        titleBar.ButtonInactiveBackgroundColor(Colors::Transparent());
        ThemeChangedImpl();
        auto viewTitleBar = CoreApplication::GetCurrentView().TitleBar();
        viewTitleBar.ExtendViewIntoTitleBar(true);
    }

    IAsyncAction MainPage::PageLoaded(IInspectable const&, RoutedEventArgs const&)
    {
        RefreshStatusImpl();
        NetState state = Model().SuggestState();
        Model().State(state);
        bool al = settings.AutoLogin();
        Model().AutoLogin(al);
        bool bal = settings.BackgroundAutoLogin();
        Model().BackgroundAutoLogin(bal);
        bool blt = settings.BackgroundLiveTile();
        Model().BackgroundLiveTile(blt);
        if (co_await BackgroundHelper::RequestAccessAsync())
        {
            BackgroundHelper::RegisterLogin(bal);
            BackgroundHelper::RegisterLiveTile(blt);
        }
        hstring un = settings.StoredUsername();
        if (!un.empty())
        {
            Model().Username(un);
            hstring pw = CredentialHelper::GetCredential(un);
            Model().Password(pw);
            if (al && !m_ToastLogined && settings.InternetAvailable() && state != NetState::Unknown && state != NetState::Direct && !pw.empty())
            {
                co_await LoginImpl();
            }
            else
            {
                co_await RefreshImpl();
            }
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

    IAsyncAction MainPage::Login(IInspectable const&, RoutedEventArgs const&)
    {
        return LoginImpl();
    }

    IAsyncAction MainPage::Logout(IInspectable const&, RoutedEventArgs const&)
    {
        return LogoutImpl();
    }

    IAsyncAction MainPage::Refresh(IInspectable const&, RoutedEventArgs const&)
    {
        return RefreshImpl();
    }

    IAsyncAction MainPage::DropUser(IInspectable const&, hstring const& e)
    {
        return DropImpl(e);
    }

    IAsyncAction MainPage::ShowChangeUser(IInspectable const&, RoutedEventArgs const&)
    {
        auto dialog = make<ChangeUserDialog>();
        hstring oldun = Model().Username();
        dialog.UnBox().Text(oldun);
        hstring oldpw = CredentialHelper::GetCredential(oldun);
        dialog.PwBox().Password(oldpw);
        if (!oldpw.empty())
        {
            dialog.SaveBox().IsChecked(true);
        }
        dialog.UnBox().TextChanged([&dialog](IInspectable const&, TextChangedEventArgs const&) {
            dialog.PwBox().Password(CredentialHelper::GetCredential(dialog.UnBox().Text()));
        });
        auto result = co_await dialog.ShowAsync();
        if (result == ContentDialogResult::Primary)
        {
            hstring un = dialog.UnBox().Text();
            hstring pw = dialog.PwBox().Password();
            CredentialHelper::RemoveCredential(un);
            if (dialog.SaveBox().IsChecked().Value())
            {
                CredentialHelper::SaveCredential(un, pw);
            }
            settings.StoredUsername(un);
            Model().Username(un);
            Model().Password(pw);
            Split().IsPaneOpen(false);
            LoginImpl();
        }
    }

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

    class ProgressRingManager
    {
    private:
        ProgressRing ring;

    public:
        ProgressRingManager(ProgressRing const& ring) : ring(ring) { ring.IsActive(true); }
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
                co_await helper.LoginAsync();
                co_await RefreshImpl(helper);
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
                co_await helper.LogoutAsync();
                co_await RefreshImpl(helper);
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
                co_await RefreshImpl(helper);
            }
        }
        catch (hresult_error const&)
        {
        }
    }
    constexpr uint64_t BaseFlux = 25000000000;
    IAsyncAction MainPage::RefreshImpl(IConnect const& helper)
    {
        auto flux = co_await helper.FluxAsync();
        NotificationHelper::UpdateTile(flux);
        Model().OnlineUser(flux.Username());
        Model().Flux(flux.Flux());
        Model().OnlineTime(flux.OnlineTime());
        Model().Balance(flux.Balance());
        double maxf = (double)UserHelper::GetMaxFlux(flux);
        Model().FluxPercent(flux.Flux() / maxf);
        Model().FreePercent(BaseFlux / maxf);
        FluxStoryboard().Begin();
    }

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

    IAsyncAction MainPage::ShowEditSuggestion(IInspectable const&, RoutedEventArgs const&)
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

    IAsyncAction MainPage::RefreshNetUsers(IInspectable const&, RoutedEventArgs const&)
    {
        return RefreshNetUsersImpl();
    }

    void MainPage::AutoLoginChanged(IInspectable const&, bool const& e)
    {
        settings.AutoLogin(e);
    }

    IAsyncAction MainPage::BackgroundAutoLoginChanged(IInspectable const&, bool const& e)
    {
        settings.BackgroundAutoLogin(e);
        if (co_await BackgroundHelper::RequestAccessAsync())
        {
            BackgroundHelper::RegisterLogin(e);
        }
    }

    IAsyncAction MainPage::BackgroundLiveTileChanged(IInspectable const&, bool const& e)
    {
        settings.BackgroundLiveTile(e);
        if (co_await BackgroundHelper::RequestAccessAsync())
        {
            BackgroundHelper::RegisterLiveTile(e);
        }
    }

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
        for (auto user : users)
        {
            usersmodel.Append(user);
        }
    }
} // namespace winrt::TsinghuaNetUWP::implementation

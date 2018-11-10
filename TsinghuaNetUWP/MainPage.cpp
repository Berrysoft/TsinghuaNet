#include "pch.h"

#include "MainPage.h"

#include "ChangeUserDialog.h"
#include "LanHelper.h"
#include <cmath>
#include <pplawait.h>
#include <winrt/Windows.ApplicationModel.Core.h>

using namespace std;
using namespace winrt;
using namespace Windows::ApplicationModel::Core;
using namespace Windows::Foundation;
using namespace Windows::UI;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::ViewManagement;

namespace winrt::TsinghuaNetUWP::implementation
{
    MainPage::MainPage()
    {
        InitializeComponent();
        auto titleBar = ApplicationView::GetForCurrentView().TitleBar();
        titleBar.BackgroundColor(Colors::Transparent());
        titleBar.ButtonBackgroundColor(Colors::Transparent());
        auto viewTitleBar = CoreApplication::GetCurrentView().TitleBar();
        viewTitleBar.ExtendViewIntoTitleBar(true);
        Model().StateChanged({ this, &MainPage::StateChanged });
        RefreshStatusImpl();
        Model().State(Model().SuggestState());
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

    IAsyncAction MainPage::ShowChangeUser(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        auto dialog = make<ChangeUserDialog>();
        dialog.UnBox().Text(Model().Username());
        auto result = co_await dialog.ShowAsync();
        if (result == ContentDialogResult::Primary)
        {
            Model().Username(dialog.UnBox().Text());
            Model().Password(dialog.PwBox().Password());
            Split().IsPaneOpen(false);
            LoginImpl();
        }
    }

    IAsyncAction MainPage::LoginImpl()
    {
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

    constexpr double base_flux = 25.0 * 1000 * 1000 * 1000;
    IAsyncAction MainPage::RefreshImpl()
    {
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
        Model().OnlineUser(flux.username);
        Model().Flux(flux.flux);
        Model().OnlineTime(flux.online_time);
        Model().Balance(flux.balance);
        double maxf = max((double)flux.flux, base_flux) + flux.balance * 2 * 1000 * 1000 * 1000;
        Model().FluxPercent(flux.flux / maxf * 100);
        Model().FreePercent(base_flux / maxf * 100);
        FluxStoryboard().Begin();
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
        switch (Model().State())
        {
        case NetState::Auth4:
            return MakeHelper<Auth4Helper>(Model().Username(), Model().Password());
        case NetState::Auth6:
            return MakeHelper<Auth6Helper>(Model().Username(), Model().Password());
        case NetState::Net:
            return MakeHelper<NetHelper>(Model().Username(), Model().Password());
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
    IAsyncAction MainPage::Auth4Checked(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        Model().SyncState(NetState::Auth4);
        return RefreshImpl();
    }
    IAsyncAction MainPage::Auth6Checked(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        Model().SyncState(NetState::Auth6);
        return RefreshImpl();
    }
    IAsyncAction MainPage::NetChecked(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        Model().SyncState(NetState::Net);
        return RefreshImpl();
    }

    void MainPage::RefreshStatus(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        RefreshStatusImpl();
    }

    void MainPage::RefreshStatusImpl()
    {
        NetState state;
        auto status = GetCurrentInternetStatus();
        switch (get<0>(status))
        {
        case InternetStatus::Lan:
            state = NetState::Auth4;
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
} // namespace winrt::TsinghuaNetUWP::implementation

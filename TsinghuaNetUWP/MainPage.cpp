#include "pch.h"

#include "MainPage.h"

#include "ChangeUserDialog.h"
#include "LanHelper.h"
#include <cmath>
#include <pplawait.h>
#include <winrt/Windows.ApplicationModel.Core.h>
#include <winrt/Windows.Security.Credentials.h>
#include <winrt/Windows.Storage.h>
#include <winrt/Windows.UI.ViewManagement.h>

using namespace std;
using namespace winrt;
using namespace Windows::ApplicationModel::Core;
using namespace Windows::Foundation;
using namespace Windows::UI;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::ViewManagement;
using namespace Windows::Security::Credentials;
using namespace Windows::Storage;

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
        NetState state = Model().SuggestState();
        Model().State(state);
        hstring un = StoredUsername();
        if (!un.empty())
        {
            Model().Username(un);
            hstring pw = GetCredential(un);
            Model().Password(pw);
            if (state != NetState::Unknown && state != NetState::Direct && !pw.empty())
            {
                LoginImpl();
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
        try
        {
            UseregHelper helper;
            helper.username = Model().Username();
            helper.password = Model().Password();
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

    IAsyncAction MainPage::DropImpl(wstring address)
    {
        try
        {
            UseregHelper helper;
            helper.username = Model().Username();
            helper.password = Model().Password();
            co_await helper.LoginAsync();
            co_await helper.LogoutAsync(address);
            co_await RefreshImpl();
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

    constexpr wchar_t CredentialResource[] = L"TsinghuaNetUWP";
    hstring GetCredential(hstring const& username)
    {
        PasswordVault vault;
        auto all = vault.RetrieveAll();
        for (auto c : all)
        {
            if (c.Resource() == CredentialResource && c.UserName() == username)
            {
                c.RetrievePassword();
                return c.Password();
            }
        }
        return {};
    }
    void SaveCredential(hstring const& username, hstring const& password)
    {
        PasswordVault vault;
        vault.Add({ CredentialResource, username, password });
    }
    void RemoveCredential(hstring const& username)
    {
        PasswordVault vault;
        auto all = vault.RetrieveAll();
        for (auto c : all)
        {
            if (c.Resource() == CredentialResource && c.UserName() == username)
            {
                vault.Remove(c);
            }
        }
    }

    constexpr wchar_t StoredUsernameKey[] = L"Username";
    hstring StoredUsername()
    {
        auto settings = ApplicationData::Current().LocalSettings();
        auto values = settings.Values();
        if (values.HasKey(StoredUsernameKey))
        {
            return unbox_value<hstring>(values.Lookup(StoredUsernameKey));
        }
        return {};
    }
    void StoredUsername(winrt::hstring const& value)
    {
        auto settings = ApplicationData::Current().LocalSettings();
        auto values = settings.Values();
        values.Insert(StoredUsernameKey, box_value(value));
    }
} // namespace winrt::TsinghuaNetUWP::implementation

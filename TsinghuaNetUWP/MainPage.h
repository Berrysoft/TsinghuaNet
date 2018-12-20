#pragma once
#include "MainPage.g.h"

#include "MainViewModel.h"
#include "NetStateCheckedConverter.h"
#include "ThemeCheckedConverter.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct MainPage : MainPageT<MainPage>
    {
    public:
        MainPage();
        void SaveSettings();

        fire_and_forget PageLoaded(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const);
        void ThemeChanged(Windows::UI::Xaml::IFrameworkElement const&, Windows::Foundation::IInspectable const&) { ThemeChangedImpl(); }
        fire_and_forget NetworkChanged(Windows::Foundation::IInspectable const);

        void OpenSettings(Windows::Foundation::IInspectable const&, Windows::UI::Xaml::RoutedEventArgs const&) { Split().IsPaneOpen(true); }
        fire_and_forget Login(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const) { co_await LoginImpl(); }
        fire_and_forget Logout(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const) { co_await LogoutImpl(); }
        fire_and_forget Refresh(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const) { co_await RefreshImpl(); }
        fire_and_forget DropUser(Windows::Foundation::IInspectable const, hstring const e) { co_await DropImpl(e); }

        fire_and_forget ShowChangeUser(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const);

        void MainTimerTick(Windows::Foundation::IInspectable const&, Windows::Foundation::IInspectable const&) { MainTimerTickImpl(); }

        void RefreshStatus(Windows::Foundation::IInspectable const&, Windows::UI::Xaml::RoutedEventArgs const&) { RefreshStatusImpl(); }
        fire_and_forget ShowEditSuggestion(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const);

        fire_and_forget RefreshNetUsers(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const) { co_await RefreshNetUsersImpl(); }
        void AutoLoginChanged(Windows::Foundation::IInspectable const&, bool const& e);
        fire_and_forget BackgroundAutoLoginChanged(Windows::Foundation::IInspectable const, bool const e);
        fire_and_forget BackgroundLiveTileChanged(Windows::Foundation::IInspectable const, bool const e);

        PROP_DECL(ToastLogined, bool)

    private:
        TsinghuaNetHelper::SettingsHelper settings;
        Windows::UI::Xaml::DispatcherTimer mainTimer;

        void ThemeChangedImpl();
        concurrency::task<void> NetworkChangedImpl();

        concurrency::task<void> LoginImpl();
        concurrency::task<void> LogoutImpl();
        concurrency::task<void> RefreshImpl();
        concurrency::task<void> RefreshImpl(TsinghuaNetHelper::IConnect const helper);
        concurrency::task<void> DropImpl(hstring const address);
        TsinghuaNetHelper::IConnect GetHelper();
        void ShowResponse(TsinghuaNetHelper::LogResponse const& response);
        void ShowHresultError(hresult_error const& e);

        void MainTimerTickImpl();

        void RefreshStatusImpl();
        concurrency::task<void> RefreshNetUsersImpl();
        concurrency::task<void> RefreshNetUsersImpl(TsinghuaNetHelper::UseregHelper const helper);
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct MainPage : MainPageT<MainPage, implementation::MainPage>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

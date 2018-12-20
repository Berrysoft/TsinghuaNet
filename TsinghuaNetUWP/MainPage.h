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

        Windows::Foundation::IAsyncAction PageLoaded(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const);
        void ThemeChanged(Windows::UI::Xaml::IFrameworkElement const&, Windows::Foundation::IInspectable const&) { ThemeChangedImpl(); }
        Windows::Foundation::IAsyncAction NetworkChanged(Windows::Foundation::IInspectable const);

        void OpenSettings(Windows::Foundation::IInspectable const&, Windows::UI::Xaml::RoutedEventArgs const&) { Split().IsPaneOpen(true); }
        Windows::Foundation::IAsyncAction Login(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const) { return LoginImpl(); }
        Windows::Foundation::IAsyncAction Logout(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const) { return LogoutImpl(); }
        Windows::Foundation::IAsyncAction Refresh(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const) { return RefreshImpl(); }
        Windows::Foundation::IAsyncAction DropUser(Windows::Foundation::IInspectable const, hstring const e) { return DropImpl(e); }

        Windows::Foundation::IAsyncAction ShowChangeUser(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const);

        void MainTimerTick(Windows::Foundation::IInspectable const&, Windows::Foundation::IInspectable const&) { MainTimerTickImpl(); }

        void RefreshStatus(Windows::Foundation::IInspectable const&, Windows::UI::Xaml::RoutedEventArgs const&) { RefreshStatusImpl(); }
        Windows::Foundation::IAsyncAction ShowEditSuggestion(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const);

        Windows::Foundation::IAsyncAction RefreshNetUsers(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const) { return RefreshNetUsersImpl(); }
        void AutoLoginChanged(Windows::Foundation::IInspectable const&, bool const& e);
        Windows::Foundation::IAsyncAction BackgroundAutoLoginChanged(Windows::Foundation::IInspectable const, bool const e);
        Windows::Foundation::IAsyncAction BackgroundLiveTileChanged(Windows::Foundation::IInspectable const, bool const e);

        PROP_DECL(ToastLogined, bool)

    private:
        TsinghuaNetHelper::SettingsHelper settings;
        Windows::UI::Xaml::DispatcherTimer mainTimer;

        void ThemeChangedImpl();
        Windows::Foundation::IAsyncAction NetworkChangedImpl();

        Windows::Foundation::IAsyncAction LoginImpl();
        Windows::Foundation::IAsyncAction LogoutImpl();
        Windows::Foundation::IAsyncAction RefreshImpl();
        Windows::Foundation::IAsyncAction RefreshImpl(TsinghuaNetHelper::IConnect const helper);
        Windows::Foundation::IAsyncAction DropImpl(hstring const address);
        TsinghuaNetHelper::IConnect GetHelper();
        void ShowResponse(TsinghuaNetHelper::LogResponse const& response);

        void MainTimerTickImpl();

        void RefreshStatusImpl();
        Windows::Foundation::IAsyncAction RefreshNetUsersImpl();
        Windows::Foundation::IAsyncAction RefreshNetUsersImpl(TsinghuaNetHelper::UseregHelper const helper);
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct MainPage : MainPageT<MainPage, implementation::MainPage>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

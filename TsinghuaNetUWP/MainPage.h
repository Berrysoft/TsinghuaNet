#pragma once
#include "MainPage.g.h"

#include "MainViewModel.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct MainPage : MainPageT<MainPage>
    {
    public:
        MainPage();

        bool ToastLogined() { return m_ToastLogined; }
        void ToastLogined(bool value) { m_ToastLogined = value; }

        Windows::Foundation::IAsyncAction PageLoaded(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const);
        void ThemeChanged(Windows::UI::Xaml::IFrameworkElement const&, Windows::Foundation::IInspectable const&);

        void OpenSettings(Windows::Foundation::IInspectable const&, Windows::UI::Xaml::RoutedEventArgs const&);
        Windows::Foundation::IAsyncAction Login(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const);
        Windows::Foundation::IAsyncAction Logout(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const);
        Windows::Foundation::IAsyncAction Refresh(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const);
        Windows::Foundation::IAsyncAction DropUser(Windows::Foundation::IInspectable const, winrt::hstring const e);

        Windows::Foundation::IAsyncAction ShowChangeUser(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const);

        void StateChanged(Windows::Foundation::IInspectable const&, TsinghuaNetHelper::NetState const&);
        void Auth4Checked(Windows::Foundation::IInspectable const&, Windows::UI::Xaml::RoutedEventArgs const&);
        void Auth6Checked(Windows::Foundation::IInspectable const&, Windows::UI::Xaml::RoutedEventArgs const&);
        void NetChecked(Windows::Foundation::IInspectable const&, Windows::UI::Xaml::RoutedEventArgs const&);
        void Auth425Checked(Windows::Foundation::IInspectable const&, Windows::UI::Xaml::RoutedEventArgs const&);
        void Auth625Checked(Windows::Foundation::IInspectable const&, Windows::UI::Xaml::RoutedEventArgs const&);

        void RefreshStatus(Windows::Foundation::IInspectable const&, Windows::UI::Xaml::RoutedEventArgs const&);
        Windows::Foundation::IAsyncAction ShowEditSuggestion(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const);

        Windows::Foundation::IAsyncAction RefreshNetUsers(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const);
        void AutoLoginChanged(Windows::Foundation::IInspectable const&, bool const& e);
        Windows::Foundation::IAsyncAction BackgroundAutoLoginChanged(Windows::Foundation::IInspectable const, bool const e);
        Windows::Foundation::IAsyncAction BackgroundLiveTileChanged(Windows::Foundation::IInspectable const, bool const e);

    private:
        bool m_ToastLogined{ false };
        TsinghuaNetHelper::SettingsHelper settings;

        void ThemeChangedImpl();

        Windows::Foundation::IAsyncAction LoginImpl();
        Windows::Foundation::IAsyncAction LogoutImpl();
        Windows::Foundation::IAsyncAction RefreshImpl();
        Windows::Foundation::IAsyncAction RefreshImpl(TsinghuaNetHelper::IConnect const helper);
        Windows::Foundation::IAsyncAction DropImpl(winrt::hstring const address);
        TsinghuaNetHelper::IConnect GetHelper();

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

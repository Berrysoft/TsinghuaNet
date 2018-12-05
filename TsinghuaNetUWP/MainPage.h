#pragma once
#include "MainPage.g.h"

#include "Arc.h"
#include "CurrencyStringConverter.h"
#include "DropUserCommand.h"
#include "FluxStringConverter.h"
#include "InternetStatusStringConverter.h"
#include "MainViewModel.h"
#include "NetStateStringConverter.h"
#include "TimeSpanStringConverter.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct MainPage : MainPageT<MainPage>
    {
    public:
        MainPage();

        bool ToastLogined() { return m_ToastLogined; }
        void ToastLogined(bool value) { m_ToastLogined = value; }

        Windows::Foundation::IAsyncAction PageLoaded(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        void ThemeChanged(Windows::UI::Xaml::IFrameworkElement const& sender, Windows::Foundation::IInspectable const& e);

        void OpenSettings(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        Windows::Foundation::IAsyncAction Login(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        Windows::Foundation::IAsyncAction Logout(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        Windows::Foundation::IAsyncAction Refresh(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        Windows::Foundation::IAsyncAction DropUser(Windows::Foundation::IInspectable const& sender, winrt::hstring const& e);

        Windows::Foundation::IAsyncAction ShowChangeUser(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);

        void StateChanged(Windows::Foundation::IInspectable const& sender, TsinghuaNetHelper::NetState const& e);
        void Auth4Checked(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        void Auth6Checked(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        void NetChecked(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        void Auth425Checked(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        void Auth625Checked(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);

        void RefreshStatus(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        Windows::Foundation::IAsyncAction ShowEditSuggestion(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);

        Windows::Foundation::IAsyncAction RefreshNetUsers(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        void AutoLoginChanged(Windows::Foundation::IInspectable const& sender, bool const& e);
        Windows::Foundation::IAsyncAction BackgroundAutoLoginChanged(Windows::Foundation::IInspectable const& sender, bool const& e);
        Windows::Foundation::IAsyncAction BackgroundLiveTileChanged(Windows::Foundation::IInspectable const& sender, bool const& e);

    private:
        bool m_ToastLogined;
        TsinghuaNetHelper::SettingsHelper settings;

        void ThemeChangedImpl();

        Windows::Foundation::IAsyncAction LoginImpl();
        Windows::Foundation::IAsyncAction LogoutImpl();
        Windows::Foundation::IAsyncAction RefreshImpl();
        Windows::Foundation::IAsyncAction RefreshImpl(TsinghuaNetHelper::IConnect const& helper);
        Windows::Foundation::IAsyncAction DropImpl(winrt::hstring const address);
        TsinghuaNetHelper::IConnect GetHelper();

        void RefreshStatusImpl();
        Windows::Foundation::IAsyncAction RefreshNetUsersImpl();
        Windows::Foundation::IAsyncAction RefreshNetUsersImpl(TsinghuaNetHelper::UseregHelper const& helper);

        void RegisterBackgroundAutoLogin(bool reg);
        void RegisterBackgroundLiveTile(bool reg);
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct MainPage : MainPageT<MainPage, implementation::MainPage>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

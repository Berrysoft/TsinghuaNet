#pragma once
#include "MainPage.g.h"

#include "Arc.h"
#include "CurrencyStringConverter.h"
#include "DropUserCommand.h"
#include "FluxStringConverter.h"
#include "InternetStatusStringConverter.h"
#include "MainViewModel.h"
#include "NetHelper.h"
#include "NetStateStringConverter.h"
#include "NetUserModel.h"
#include "TimeSpanStringConverter.h"
#include <memory>

namespace winrt::TsinghuaNetUWP::implementation
{
    struct MainPage : MainPageT<MainPage>
    {
    public:
        MainPage();

        Windows::Foundation::IAsyncAction PageLoaded(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);

        void OpenSettings(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        Windows::Foundation::IAsyncAction Login(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        Windows::Foundation::IAsyncAction Logout(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        Windows::Foundation::IAsyncAction Refresh(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        Windows::Foundation::IAsyncAction DropUser(Windows::Foundation::IInspectable const& sender, winrt::hstring const& e);

        Windows::Foundation::IAsyncAction ShowChangeUser(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);

        void StateChanged(Windows::Foundation::IInspectable const& sender, NetState const& e);
        void Auth4Checked(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        void Auth6Checked(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        void NetChecked(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);

        void RefreshStatus(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        Windows::Foundation::IAsyncAction RefreshNetUsers(Windows::Foundation::IInspectable const& sender, Windows::UI::Xaml::RoutedEventArgs const& e);
        void AutoLoginChanged(Windows::Foundation::IInspectable const& sender, winrt::optional<bool> const& e);

    private:
        Windows::Foundation::IAsyncAction LoginImpl();
        Windows::Foundation::IAsyncAction LogoutImpl();
        Windows::Foundation::IAsyncAction RefreshImpl();
        concurrency::task<FluxUser> RefreshImpl(IConnect const& helper);
        Windows::Foundation::IAsyncAction DropImpl(std::wstring address);
        std::unique_ptr<IConnect> GetHelper();

        void RefreshStatusImpl();
        Windows::Foundation::IAsyncAction RefreshNetUsersImpl();
        Windows::Foundation::IAsyncAction RefreshNetUsersImpl(UseregHelper const& helper);
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct MainPage : MainPageT<MainPage, implementation::MainPage>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

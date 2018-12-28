#pragma once
#include "MainViewModel.g.h"

#include "../Shared/Utility.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct MainViewModel : MainViewModelT<MainViewModel>
    {
    public:
        MainViewModel();

        DEPENDENCY_PROPERTY(UserContent, Windows::UI::Xaml::UIElement)

        DEPENDENCY_PROPERTY(Response, TsinghuaNetHelper::LogResponse)

        DEPENDENCY_PROPERTY(Username, hstring)
        DEPENDENCY_PROPERTY(Password, hstring)

        DEPENDENCY_PROPERTY(State, TsinghuaNetHelper::NetState)

        DEPENDENCY_PROPERTY(AutoLogin, bool)
        EVENT_DECL(AutoLoginChanged, bool)
        DEPENDENCY_PROPERTY(BackgroundAutoLogin, bool)
        EVENT_DECL(BackgroundAutoLoginChanged, bool)
        DEPENDENCY_PROPERTY(BackgroundLiveTile, bool)
        EVENT_DECL(BackgroundLiveTileChanged, bool)

        DEPENDENCY_PROPERTY(NetStatus, TsinghuaNetHelper::InternetStatus)
        DEPENDENCY_PROPERTY(Ssid, hstring)
        DEPENDENCY_PROPERTY(SuggestState, TsinghuaNetHelper::NetState)

        DEPENDENCY_PROPERTY(Theme, Windows::UI::Xaml::ElementTheme)
        DEPENDENCY_PROPERTY(ContentType, TsinghuaNetHelper::UserContentType)

    public:
        Windows::Foundation::Collections::IObservableVector<Windows::Foundation::IInspectable> NetUsers() const { return m_NetUsers; }

    private:
        Windows::Foundation::Collections::IObservableVector<Windows::Foundation::IInspectable> m_NetUsers;

        static void OnAutoLoginPropertyChanged(Windows::UI::Xaml::DependencyObject const& d, Windows::UI::Xaml::DependencyPropertyChangedEventArgs const& e);
        static void OnBackgroundAutoLoginPropertyChanged(Windows::UI::Xaml::DependencyObject const& d, Windows::UI::Xaml::DependencyPropertyChangedEventArgs const& e);
        static void OnBackgroundLiveTilePropertyChanged(Windows::UI::Xaml::DependencyObject const& d, Windows::UI::Xaml::DependencyPropertyChangedEventArgs const& e);
        static void OnContentTypePropertyChanged(Windows::UI::Xaml::DependencyObject const& d, Windows::UI::Xaml::DependencyPropertyChangedEventArgs const& e);
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct MainViewModel : MainViewModelT<MainViewModel, implementation::MainViewModel>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

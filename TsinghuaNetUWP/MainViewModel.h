#pragma once

#include "MainViewModel.g.h"

#include "DependencyHelper.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct MainViewModel : MainViewModelT<MainViewModel>
    {
    public:
        MainViewModel();

        DEPENDENCY_PROPERTY(OnlineUser, hstring)
        DEPENDENCY_PROPERTY(Flux, std::uint64_t)
        DEPENDENCY_PROPERTY(OnlineTime, Windows::Foundation::TimeSpan)
        DEPENDENCY_PROPERTY(Balance, double)

        DEPENDENCY_PROPERTY(FluxPercent, double)
        DEPENDENCY_PROPERTY(FreePercent, double)

        DEPENDENCY_PROPERTY(Response, TsinghuaNetHelper::LogResponse)

        DEPENDENCY_PROPERTY(Username, hstring)
        DEPENDENCY_PROPERTY(Password, hstring)

        DEPENDENCY_PROPERTY(AutoLogin, bool)
        EVENT_DECL(AutoLoginChanged, bool)
        DEPENDENCY_PROPERTY(BackgroundAutoLogin, bool)
        EVENT_DECL(BackgroundAutoLoginChanged, bool)
        DEPENDENCY_PROPERTY(BackgroundLiveTile, bool)
        EVENT_DECL(BackgroundLiveTileChanged, bool)

        DEPENDENCY_PROPERTY(NetStatus, TsinghuaNetHelper::InternetStatus)
        DEPENDENCY_PROPERTY(Ssid, hstring)
        DEPENDENCY_PROPERTY(SuggestState, TsinghuaNetHelper::NetState)

        EVENT_DECL(StateChanged, TsinghuaNetHelper::NetState)

    public:
        Windows::Foundation::Collections::IObservableVector<Windows::Foundation::IInspectable> NetUsers() const { return m_NetUsers; }

        TsinghuaNetHelper::NetState State() const { return m_State; }
        void State(TsinghuaNetHelper::NetState value)
        {
            m_State = value;
            m_StateChangedEvent(*this, value);
        }

    private:
        Windows::Foundation::Collections::IObservableVector<Windows::Foundation::IInspectable> m_NetUsers;
        TsinghuaNetHelper::NetState m_State;

        static void OnAutoLoginPropertyChanged(Windows::UI::Xaml::DependencyObject const& d, Windows::UI::Xaml::DependencyPropertyChangedEventArgs const& e);
        static void OnBackgroundAutoLoginPropertyChanged(Windows::UI::Xaml::DependencyObject const& d, Windows::UI::Xaml::DependencyPropertyChangedEventArgs const& e);
        static void OnBackgroundLiveTilePropertyChanged(Windows::UI::Xaml::DependencyObject const& d, Windows::UI::Xaml::DependencyPropertyChangedEventArgs const& e);
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct MainViewModel : MainViewModelT<MainViewModel, implementation::MainViewModel>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

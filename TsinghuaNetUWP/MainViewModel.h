#pragma once

#include "MainViewModel.g.h"

#include "DependencyHelper.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct MainViewModel : MainViewModelT<MainViewModel>
    {
    public:
        MainViewModel() = default;

        DEPENDENCY_PROPERTY(OnlineUser, winrt::hstring)
        DEPENDENCY_PROPERTY(Flux, std::uint64_t)
        DEPENDENCY_PROPERTY(OnlineTime, Windows::Foundation::TimeSpan)
        DEPENDENCY_PROPERTY(Balance, double)

        DEPENDENCY_PROPERTY(FluxPercent, double)
        DEPENDENCY_PROPERTY(FreePercent, double)

        DEPENDENCY_PROPERTY(Username, winrt::hstring)
        DEPENDENCY_PROPERTY(Password, winrt::hstring)

        DEPENDENCY_PROPERTY(NetStatus, InternetStatus)
        DEPENDENCY_PROPERTY(Ssid, winrt::hstring)
        DEPENDENCY_PROPERTY(SuggestState, NetState)

        EVENT_DECL(StateChanged, NetState)

    public:
        NetState State() { return m_State; }
        void State(NetState value)
        {
            m_State = value;
            m_StateChangedEvent(*this, value);
        }
        void SyncState(NetState value) { m_State = value; }

    private:
        NetState m_State;
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct MainViewModel : MainViewModelT<MainViewModel, implementation::MainViewModel>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

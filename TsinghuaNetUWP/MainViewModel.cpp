#include "pch.h"

#include "MainViewModel.h"

using namespace std;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;

namespace winrt::TsinghuaNetUWP::implementation
{
    DEPENDENCY_PROPERTY_INIT(OnlineUser, hstring, MainViewModel, TsinghuaNetUWP::MainViewModel, box_value(hstring()))
    DEPENDENCY_PROPERTY_INIT(Flux, uint64_t, MainViewModel, TsinghuaNetUWP::MainViewModel, box_value<uint64_t>(0))
    DEPENDENCY_PROPERTY_INIT(OnlineTime, TimeSpan, MainViewModel, TsinghuaNetUWP::MainViewModel, box_value(TimeSpan()))
    DEPENDENCY_PROPERTY_INIT(Balance, double, MainViewModel, TsinghuaNetUWP::MainViewModel, box_value(0.0))

    DEPENDENCY_PROPERTY_INIT(FluxPercent, double, MainViewModel, TsinghuaNetUWP::MainViewModel, box_value(0.0))
    DEPENDENCY_PROPERTY_INIT(FreePercent, double, MainViewModel, TsinghuaNetUWP::MainViewModel, box_value(100.0))

    DEPENDENCY_PROPERTY_INIT(Username, hstring, MainViewModel, TsinghuaNetUWP::MainViewModel, box_value(hstring()))
    DEPENDENCY_PROPERTY_INIT(Password, hstring, MainViewModel, TsinghuaNetUWP::MainViewModel, box_value(hstring()))

    DEPENDENCY_PROPERTY_INIT(NetStatus, InternetStatus, MainViewModel, TsinghuaNetUWP::MainViewModel, box_value(InternetStatus::Unknown))
    DEPENDENCY_PROPERTY_INIT(Ssid, hstring, MainViewModel, TsinghuaNetUWP::MainViewModel, box_value(hstring()))
    DEPENDENCY_PROPERTY_INIT(SuggestState, NetState, MainViewModel, TsinghuaNetUWP::MainViewModel, box_value(NetState::Unknown))
} // namespace winrt::TsinghuaNetUWP::implementation

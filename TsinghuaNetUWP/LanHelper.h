#pragma once
#include "MainViewModel.h"
#include <tuple>

namespace winrt::TsinghuaNetUWP
{
    std::tuple<InternetStatus, winrt::hstring> GetCurrentInternetStatus();
    Windows::Foundation::IAsyncAction LoadStates();
    NetState GetLanState();
	NetState GetWwanState();
    NetState GetWlanState(winrt::hstring const& ssid);
} // namespace winrt::TsinghuaNetUWP

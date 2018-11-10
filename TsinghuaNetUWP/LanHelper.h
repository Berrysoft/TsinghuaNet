#pragma once
#include "MainViewModel.h"
#include <tuple>

namespace winrt::TsinghuaNetUWP
{
    std::tuple<InternetStatus, winrt::hstring> GetCurrentInternetStatus();
    NetState GetWlanState(winrt::hstring const& ssid);
} // namespace winrt::TsinghuaNetUWP

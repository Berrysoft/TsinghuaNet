#pragma once
#include "winrt/TsinghuaNetHelper.h"

namespace winrt::TsinghuaNetHelper
{
    NetState GetSuggestNetState(SettingsHelper const& settings);
    IConnect GetHelper(NetState state);
    IConnect GetHelper(NetState state, winrt::hstring const& username, winrt::hstring const& password);
} // namespace winrt::TsinghuaNetHelper

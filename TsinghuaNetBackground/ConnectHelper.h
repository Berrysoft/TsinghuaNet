#pragma once
#include "winrt/TsinghuaNetHelper.h"

namespace winrt::TsinghuaNetBackground
{
    TsinghuaNetHelper::NetState GetSuggestNetState(TsinghuaNetHelper::SettingsHelper const& settings);
    TsinghuaNetHelper::IConnect GetHelper(TsinghuaNetHelper::NetState state);
    TsinghuaNetHelper::IConnect GetHelper(TsinghuaNetHelper::NetState state, winrt::hstring const& username, winrt::hstring const& password);
} // namespace winrt::TsinghuaNetBackground

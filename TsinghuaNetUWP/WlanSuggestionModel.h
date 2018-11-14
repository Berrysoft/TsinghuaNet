#pragma once
#include "WlanSuggestionModel.g.h"

#include "DependencyHelper.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct WlanSuggestionModel : WlanSuggestionModelT<WlanSuggestionModel>
    {
        WlanSuggestionModel() = default;

        DEPENDENCY_PROPERTY(Ssid, winrt::hstring)
        DEPENDENCY_PROPERTY(State, NetState)
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct WlanSuggestionModel : WlanSuggestionModelT<WlanSuggestionModel, implementation::WlanSuggestionModel>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

#include "pch.h"

#include "WlanSuggestionModel.h"

using namespace winrt;

namespace winrt::TsinghuaNetUWP::implementation
{
    DEPENDENCY_PROPERTY_INIT(Ssid, hstring, WlanSuggestionModel, TsinghuaNetUWP::WlanSuggestionModel, box_value(hstring()))
    DEPENDENCY_PROPERTY_INIT(State, NetState, WlanSuggestionModel, TsinghuaNetUWP::WlanSuggestionModel, box_value(NetState::Unknown))
} // namespace winrt::TsinghuaNetUWP::implementation

#pragma once
#include "ConnectHelper.g.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct ConnectHelper
    {
        ConnectHelper() = delete;

        static TsinghuaNetHelper::NetState GetSuggestNetState(TsinghuaNetHelper::SettingsHelper const& settings);
        static TsinghuaNetHelper::IConnect GetHelper(TsinghuaNetHelper::NetState const& state);
        static TsinghuaNetHelper::IConnect GetHelper(TsinghuaNetHelper::NetState const& state, hstring const& username, hstring const& password);
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct ConnectHelper : ConnectHelperT<ConnectHelper, implementation::ConnectHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

#pragma once
#include "ConnectHelper.g.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct ConnectHelper
    {
        ConnectHelper() = delete;

        static IConnect GetHelper(NetState const& state);
        static IConnect GetHelper(NetState const& state, hstring const& username, hstring const& password);
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct ConnectHelper : ConnectHelperT<ConnectHelper, implementation::ConnectHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

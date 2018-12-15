#pragma once
#include "NetStateSsidBox.g.h"

#include "DependencyHelper.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct NetStateSsidBox : NetStateSsidBoxT<NetStateSsidBox>
    {
        NetStateSsidBox() = default;

        PROP_DECL_REF(Ssid, winrt::hstring)
        PROP_DECL(Value, int)
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct NetStateSsidBox : NetStateSsidBoxT<NetStateSsidBox, implementation::NetStateSsidBox>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

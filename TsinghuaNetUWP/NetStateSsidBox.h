#pragma once
#include "NetStateSsidBox.g.h"

#include "../Shared/Utility.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct NetStateSsidBox : NetStateSsidBoxT<NetStateSsidBox>
    {
        NetStateSsidBox() = default;

        PROP_DECL_REF(Ssid, hstring)
        PROP_DECL(Value, int)
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct NetStateSsidBox : NetStateSsidBoxT<NetStateSsidBox, implementation::NetStateSsidBox>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

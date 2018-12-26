#pragma once
#include "NetStateBox.g.h"

#include "../Shared/Utility.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct NetStateBox : NetStateBoxT<NetStateBox>
    {
        NetStateBox(TsinghuaNetHelper::NetState state = TsinghuaNetHelper::NetState::Unknown) : m_Value(state) {}

        PROP_DECL(Value, TsinghuaNetHelper::NetState)
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct NetStateBox : NetStateBoxT<NetStateBox, implementation::NetStateBox>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

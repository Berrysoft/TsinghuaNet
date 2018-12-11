#pragma once
#include "NetStateBox.g.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct NetStateBox : NetStateBoxT<NetStateBox>
    {
        NetStateBox(TsinghuaNetHelper::NetState state = TsinghuaNetHelper::NetState::Unknown) : m_Value(state) {}

        TsinghuaNetHelper::NetState Value() const noexcept { return m_Value; }
        void Value(TsinghuaNetHelper::NetState value) noexcept { m_Value = value; }

    private:
        TsinghuaNetHelper::NetState m_Value;
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct NetStateBox : NetStateBoxT<NetStateBox, implementation::NetStateBox>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

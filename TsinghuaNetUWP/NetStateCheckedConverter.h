#pragma once
#include "NetStateCheckedConverter.g.h"

#include "EnumCheckedConverter.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct NetStateCheckedConverter : NetStateCheckedConverterT<NetStateCheckedConverter>, EnumCheckedConverter<TsinghuaNetHelper::NetState>
    {
        NetStateCheckedConverter() = default;
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct NetStateCheckedConverter : NetStateCheckedConverterT<NetStateCheckedConverter, implementation::NetStateCheckedConverter>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

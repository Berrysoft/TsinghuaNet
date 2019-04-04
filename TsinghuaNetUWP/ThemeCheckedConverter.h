#pragma once
#include "ThemeCheckedConverter.g.h"

#include "EnumCheckedConverter.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct ThemeCheckedConverter : ThemeCheckedConverterT<ThemeCheckedConverter>, EnumCheckedConverter<TsinghuaNetHelper::UserTheme>
    {
        ThemeCheckedConverter() = default;
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct ThemeCheckedConverter : ThemeCheckedConverterT<ThemeCheckedConverter, implementation::ThemeCheckedConverter>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

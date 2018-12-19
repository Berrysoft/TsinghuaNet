#pragma once
#include "ThemeCheckedConverter.g.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct ThemeCheckedConverter : ThemeCheckedConverterT<ThemeCheckedConverter>
    {
        ThemeCheckedConverter() = default;

        Windows::Foundation::IInspectable Convert(Windows::Foundation::IInspectable const& value, Windows::UI::Xaml::Interop::TypeName const&, Windows::Foundation::IInspectable const& parameter, hstring const&);
        Windows::Foundation::IInspectable ConvertBack(Windows::Foundation::IInspectable const& value, Windows::UI::Xaml::Interop::TypeName const&, Windows::Foundation::IInspectable const& parameter, hstring const&);
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct ThemeCheckedConverter : ThemeCheckedConverterT<ThemeCheckedConverter, implementation::ThemeCheckedConverter>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

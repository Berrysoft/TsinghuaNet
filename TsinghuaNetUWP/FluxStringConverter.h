#pragma once

#include "FluxStringConverter.g.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct FluxStringConverter : FluxStringConverterT<FluxStringConverter>
    {
        FluxStringConverter() = default;

        Windows::Foundation::IInspectable Convert(Windows::Foundation::IInspectable const& value, Windows::UI::Xaml::Interop::TypeName const& targetType, Windows::Foundation::IInspectable const& parameter, hstring const& language);
        Windows::Foundation::IInspectable ConvertBack(Windows::Foundation::IInspectable const& value, Windows::UI::Xaml::Interop::TypeName const& targetType, Windows::Foundation::IInspectable const& parameter, hstring const& language);
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct FluxStringConverter : FluxStringConverterT<FluxStringConverter, implementation::FluxStringConverter>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

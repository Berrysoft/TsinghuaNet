#pragma once

#include "TimeSpanStringConverter.g.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct TimeSpanStringConverter : TimeSpanStringConverterT<TimeSpanStringConverter>
    {
        TimeSpanStringConverter() = default;

        Windows::Foundation::IInspectable Convert(Windows::Foundation::IInspectable const& value, Windows::UI::Xaml::Interop::TypeName const& targetType, Windows::Foundation::IInspectable const& parameter, hstring const& language) const;
        Windows::Foundation::IInspectable ConvertBack(Windows::Foundation::IInspectable const& value, Windows::UI::Xaml::Interop::TypeName const& targetType, Windows::Foundation::IInspectable const& parameter, hstring const& language) const;
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct TimeSpanStringConverter : TimeSpanStringConverterT<TimeSpanStringConverter, implementation::TimeSpanStringConverter>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

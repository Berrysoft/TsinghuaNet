#pragma once

#include "InternetStatusStringConverter.g.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct InternetStatusStringConverter : InternetStatusStringConverterT<InternetStatusStringConverter>
    {
        InternetStatusStringConverter() = default;

        Windows::Foundation::IInspectable Convert(Windows::Foundation::IInspectable const& value, Windows::UI::Xaml::Interop::TypeName const& targetType, Windows::Foundation::IInspectable const& parameter, hstring const& language);
        Windows::Foundation::IInspectable ConvertBack(Windows::Foundation::IInspectable const& value, Windows::UI::Xaml::Interop::TypeName const& targetType, Windows::Foundation::IInspectable const& parameter, hstring const& language);
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct InternetStatusStringConverter : InternetStatusStringConverterT<InternetStatusStringConverter, implementation::InternetStatusStringConverter>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

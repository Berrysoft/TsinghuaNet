#pragma once
#include "NetStateIndexConverter.g.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct NetStateIndexConverter : NetStateIndexConverterT<NetStateIndexConverter>
    {
        NetStateIndexConverter() = default;

        Windows::Foundation::IInspectable Convert(Windows::Foundation::IInspectable const& value, Windows::UI::Xaml::Interop::TypeName const&, Windows::Foundation::IInspectable const&, hstring const&);
        Windows::Foundation::IInspectable ConvertBack(Windows::Foundation::IInspectable const& value, Windows::UI::Xaml::Interop::TypeName const&, Windows::Foundation::IInspectable const&, hstring const&);
    };
} // namespace winrt::TsinghuaNetUWP::implementation
namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct NetStateIndexConverter : NetStateIndexConverterT<NetStateIndexConverter, implementation::NetStateIndexConverter>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

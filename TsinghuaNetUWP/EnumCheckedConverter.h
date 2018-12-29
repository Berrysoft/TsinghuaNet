#pragma once
#include "winrt/TsinghuaNetUWP.h"

namespace winrt::TsinghuaNetUWP
{
    template <typename T>
    struct EnumCheckedConverter
    {
        Windows::Foundation::IInspectable Convert(Windows::Foundation::IInspectable const& value, Windows::UI::Xaml::Interop::TypeName const&, Windows::Foundation::IInspectable const& parameter, hstring const&)
        {
            return box_value<bool>(unbox_value<T>(value) == unbox_value<T>(parameter));
        }

        Windows::Foundation::IInspectable ConvertBack(Windows::Foundation::IInspectable const& value, Windows::UI::Xaml::Interop::TypeName const&, Windows::Foundation::IInspectable const& parameter, hstring const&)
        {
            return unbox_value<bool>(value) ? parameter : Windows::UI::Xaml::DependencyProperty::UnsetValue();
        }
    };
} // namespace winrt::TsinghuaNetUWP

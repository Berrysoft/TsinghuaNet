#pragma once
#include "winrt/TsinghuaNetUWP.h"

namespace winrt::TsinghuaNetUWP
{
    template <typename T>
    struct EnumCheckedConverter
    {
        Windows::Foundation::IInspectable Convert(Windows::Foundation::IInspectable const& value, Windows::UI::Xaml::Interop::TypeName const&, Windows::Foundation::IInspectable const& parameter, hstring const&)
        {
            return optional<bool>(unbox_value<T>(value) == unbox_value<T>(parameter));
        }

        Windows::Foundation::IInspectable ConvertBack(Windows::Foundation::IInspectable const& value, Windows::UI::Xaml::Interop::TypeName const&, Windows::Foundation::IInspectable const& parameter, hstring const&)
        {
            // TODO: 这是个编译器Bug，以后要改回来
            optional<bool> v{ nullptr };
            value.as(v);
            return v.Value() ? parameter : Windows::UI::Xaml::DependencyProperty::UnsetValue();
        }
    };
} // namespace winrt::TsinghuaNetUWP

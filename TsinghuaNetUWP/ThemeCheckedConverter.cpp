#include "pch.h"

#include "ThemeCheckedConverter.h"

using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Interop;

namespace winrt::TsinghuaNetUWP::implementation
{
    IInspectable ThemeCheckedConverter::Convert(IInspectable const& value, TypeName const&, IInspectable const& parameter, hstring const&)
    {
        return optional<bool>(unbox_value<ElementTheme>(value) == unbox_value<ElementTheme>(parameter));
    }

    IInspectable ThemeCheckedConverter::ConvertBack(IInspectable const& value, TypeName const&, IInspectable const& parameter, hstring const&)
    {
        return value.as<optional<bool>>().Value() ? box_value(unbox_value<ElementTheme>(parameter)) : DependencyProperty::UnsetValue();
    }
} // namespace winrt::TsinghuaNetUWP::implementation

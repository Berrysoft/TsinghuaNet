#include "pch.h"

#include "NetStateCheckedConverter.h"
#include <map>

using namespace std;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Interop;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    IInspectable NetStateCheckedConverter::Convert(IInspectable const& value, TypeName const&, IInspectable const& parameter, hstring const&)
    {
        return optional<bool>(unbox_value<NetState>(value) == unbox_value<NetState>(parameter));
    }

    IInspectable NetStateCheckedConverter::ConvertBack(IInspectable const& value, TypeName const&, IInspectable const& parameter, hstring const&)
    {
        return value.as<optional<bool>>().Value() ? box_value(unbox_value<NetState>(parameter)) : DependencyProperty::UnsetValue();
    }
} // namespace winrt::TsinghuaNetUWP::implementation

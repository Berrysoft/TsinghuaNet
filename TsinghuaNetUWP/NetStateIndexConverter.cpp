#include "pch.h"

#include "NetStateIndexConverter.h"

using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml::Interop;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    IInspectable NetStateIndexConverter::Convert(IInspectable const& value, TypeName const&, IInspectable const&, hstring const&)
    {
        int index = (int)unbox_value<NetState>(value);
        if (index > 2) index = 0;
        return box_value(index);
    }

    IInspectable NetStateIndexConverter::ConvertBack(IInspectable const& value, TypeName const&, IInspectable const&, hstring const&)
    {
        return box_value((NetState)unbox_value<int>(value));
    }
} // namespace winrt::TsinghuaNetUWP::implementation

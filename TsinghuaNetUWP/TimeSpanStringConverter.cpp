#include "pch.h"

#include "TimeSpanStringConverter.h"

#include "winrt/TsinghuaNetHelper.h"

using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml::Interop;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    IInspectable TimeSpanStringConverter::Convert(IInspectable const& value, TypeName const& /*targetType*/, IInspectable const& /*parameter*/, hstring const& /*language*/) const
    {
        return box_value(UserHelper::GetTimeSpanString(unbox_value<TimeSpan>(value)));
    }

    IInspectable TimeSpanStringConverter::ConvertBack(IInspectable const& /*value*/, TypeName const& /*targetType*/, IInspectable const& /*parameter*/, hstring const& /*language*/) const
    {
        throw hresult_not_implemented();
    }
} // namespace winrt::TsinghuaNetUWP::implementation

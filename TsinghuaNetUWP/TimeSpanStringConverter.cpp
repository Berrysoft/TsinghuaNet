#include "pch.h"

#include "TimeSpanStringConverter.h"

using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml::Interop;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    IInspectable TimeSpanStringConverter::Convert(IInspectable const& value, TypeName const&, IInspectable const&, hstring const&) const
    {
        return box_value(UserHelper::GetTimeSpanString(unbox_value<TimeSpan>(value)));
    }

    IInspectable TimeSpanStringConverter::ConvertBack(IInspectable const&, TypeName const&, IInspectable const&, hstring const&) const
    {
        throw hresult_not_implemented();
    }
} // namespace winrt::TsinghuaNetUWP::implementation

#include "pch.h"

#include "TimeSpanStringConverter.h"
#include <sf/sformat.hpp>

using sf::sprint;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml::Interop;

namespace winrt::TsinghuaNetUWP::implementation
{
    IInspectable TimeSpanStringConverter::Convert(IInspectable const& value, TypeName const& /*targetType*/, IInspectable const& /*parameter*/, hstring const& /*language*/)
    {
        TimeSpan span = unbox_value<TimeSpan>(value);
        int64_t tsec = span.count() / 10000000;
        bool minus = tsec < 0;
        if (minus)
            tsec = -tsec;
        int64_t h = tsec / 3600;
        tsec -= h * 3600;
        int64_t min = tsec / 60;
        tsec -= min * 60;
        return box_value(sprint(L"{}{:d2}:{:d2}:{:d2}", minus ? L"-" : L"", h, min, tsec));
    }

    IInspectable TimeSpanStringConverter::ConvertBack(IInspectable const& /*value*/, TypeName const& /*targetType*/, IInspectable const& /*parameter*/, hstring const& /*language*/)
    {
        throw hresult_not_implemented();
    }
} // namespace winrt::TsinghuaNetUWP::implementation

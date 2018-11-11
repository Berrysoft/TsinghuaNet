#include "pch.h"

#include "CurrencyStringConverter.h"
#include "NotificationHelper.h"

using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml::Interop;

namespace winrt::TsinghuaNetUWP::implementation
{
    IInspectable CurrencyStringConverter::Convert(IInspectable const& value, TypeName const& /*targetType*/, IInspectable const& /*parameter*/, hstring const& /*language*/) const
    {
        return box_value(GetCurrencyString(unbox_value<double>(value)));
    }

    IInspectable CurrencyStringConverter::ConvertBack(IInspectable const& /*value*/, TypeName const& /*targetType*/, IInspectable const& /*parameter*/, hstring const& /*language*/) const
    {
        throw hresult_not_implemented();
    }
} // namespace winrt::TsinghuaNetUWP::implementation

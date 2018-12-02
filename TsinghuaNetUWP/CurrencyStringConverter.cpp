#include "pch.h"

#include "CurrencyStringConverter.h"

#include "winrt/TsinghuaNetHelper.h"

using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml::Interop;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    IInspectable CurrencyStringConverter::Convert(IInspectable const& value, TypeName const&, IInspectable const&, hstring const&) const
    {
        return box_value(UserHelper::GetCurrencyString(unbox_value<double>(value)));
    }

    IInspectable CurrencyStringConverter::ConvertBack(IInspectable const&, TypeName const&, IInspectable const&, hstring const&) const
    {
        throw hresult_not_implemented();
    }
} // namespace winrt::TsinghuaNetUWP::implementation

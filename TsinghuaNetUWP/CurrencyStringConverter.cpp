#include "pch.h"

#include "CurrencyStringConverter.h"
#include <sf/sformat.hpp>

using sf::sprint;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml::Interop;

namespace winrt::TsinghuaNetUWP::implementation
{
    IInspectable CurrencyStringConverter::Convert(IInspectable const& value, TypeName const& /*targetType*/, IInspectable const& /*parameter*/, hstring const& /*language*/)
    {
        return box_value(sprint(L"￥{:f2}", unbox_value<double>(value)));
    }

    IInspectable CurrencyStringConverter::ConvertBack(IInspectable const& /*value*/, TypeName const& /*targetType*/, IInspectable const& /*parameter*/, hstring const& /*language*/)
    {
        throw hresult_not_implemented();
    }
} // namespace winrt::TsinghuaNetUWP::implementation

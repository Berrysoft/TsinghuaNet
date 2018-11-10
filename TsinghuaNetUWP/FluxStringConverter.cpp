#include "pch.h"

#include "FluxStringConverter.h"
#include <sf/sformat.hpp>

using sf::sprint;
using namespace std;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml::Interop;

namespace winrt::TsinghuaNetUWP::implementation
{
    IInspectable FluxStringConverter::Convert(IInspectable const& value, TypeName const& /*targetType*/, IInspectable const& /*parameter*/, hstring const& /*language*/)
    {
        double flux = (double)unbox_value<uint64_t>(value);
        wstring result;
        if (flux < 1000)
        {
            result = sprint(L"{} B", flux);
        }
        else if ((flux /= 1000) < 1000)
        {
            result = sprint(L"{:f2} kB", flux);
        }
        else if ((flux /= 1000) < 1000)
        {
            result = sprint(L"{:f2} MB", flux);
        }
        else
        {
            flux /= 1000;
            result = sprint(L"{:f2} GB", flux);
        }
        return box_value(result);
    }

    IInspectable FluxStringConverter::ConvertBack(IInspectable const& /*value*/, TypeName const& /*targetType*/, IInspectable const& /*parameter*/, hstring const& /*language*/)
    {
        throw hresult_not_implemented();
    }
} // namespace winrt::TsinghuaNetUWP::implementation

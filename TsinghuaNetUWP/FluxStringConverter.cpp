#include "pch.h"

#include "FluxStringConverter.h"

using namespace std;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml::Interop;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    IInspectable FluxStringConverter::Convert(IInspectable const& value, TypeName const&, IInspectable const&, hstring const&) const
    {
        uint64_t flux = unbox_value<uint64_t>(value);
        hstring result = UserHelper::GetFluxString(flux);
        return box_value(result);
    }

    IInspectable FluxStringConverter::ConvertBack(IInspectable const&, TypeName const&, IInspectable const&, hstring const&) const
    {
        throw hresult_not_implemented();
    }
} // namespace winrt::TsinghuaNetUWP::implementation

#include "pch.h"

#include "FluxUserBox.h"

using namespace winrt;
using namespace Windows::Foundation;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    FluxUserBox::FluxUserBox(FluxUser const& flux)
    {
        Username(flux.Username);
        Flux(flux.Flux);
        OnlineTime(flux.OnlineTime);
        Balance(flux.Balance);
    }

    DEPENDENCY_PROPERTY_INIT(Username, hstring, FluxUserBox, box_value(hstring{}))
    DEPENDENCY_PROPERTY_INIT(Flux, uint64_t, FluxUserBox, box_value<uint64_t>(0))
    DEPENDENCY_PROPERTY_INIT(OnlineTime, TimeSpan, FluxUserBox, box_value(TimeSpan{}))
    DEPENDENCY_PROPERTY_INIT(Balance, double, FluxUserBox, box_value<double>(0.0))
} // namespace winrt::TsinghuaNetUWP::implementation

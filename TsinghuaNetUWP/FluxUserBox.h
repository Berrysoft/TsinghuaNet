#pragma once
#include "FluxUserBox.g.h"

#include "../Shared/Utility.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct FluxUserBox : FluxUserBoxT<FluxUserBox>
    {
        FluxUserBox(TsinghuaNetHelper::FluxUser const& flux = {});

        DEPENDENCY_PROPERTY(Username, hstring)
        DEPENDENCY_PROPERTY(Flux, uint64_t)
        DEPENDENCY_PROPERTY(OnlineTime, Windows::Foundation::TimeSpan)
        DEPENDENCY_PROPERTY(Balance, double)
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct FluxUserBox : FluxUserBoxT<FluxUserBox, implementation::FluxUserBox>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

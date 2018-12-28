#pragma once
#include "FluxUser.g.h"

#include "../Shared/Utility.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct FluxUser : FluxUserT<FluxUser>
    {
        FluxUser() = default;

        DEPENDENCY_PROPERTY(Username, hstring)
        DEPENDENCY_PROPERTY(Flux, uint64_t)
        DEPENDENCY_PROPERTY(OnlineTime, Windows::Foundation::TimeSpan)
        DEPENDENCY_PROPERTY(Balance, double)
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct FluxUser : FluxUserT<FluxUser, implementation::FluxUser>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

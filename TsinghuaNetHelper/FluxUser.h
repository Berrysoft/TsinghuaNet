#pragma once
#include "FluxUser.g.h"

#include "Utility.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct FluxUser : FluxUserT<FluxUser>
    {
        FluxUser() = default;

        static TsinghuaNetHelper::FluxUser Parse(hstring const& fluxstr);

        PROP_DECL_REF(Username, winrt::hstring)
        PROP_DECL(Flux, uint64_t)
        PROP_DECL_REF(OnlineTime, Windows::Foundation::TimeSpan)
        PROP_DECL(Balance, double)
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct FluxUser : FluxUserT<FluxUser, implementation::FluxUser>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

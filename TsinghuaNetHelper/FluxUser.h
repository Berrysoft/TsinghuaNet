#pragma once

#include "FluxUser.g.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct FluxUser : FluxUserT<FluxUser>
    {
        FluxUser() = default;

        hstring Username();
        void Username(hstring const& value);
        uint64_t Flux();
        void Flux(uint64_t value);
        Windows::Foundation::TimeSpan OnlineTime();
        void OnlineTime(Windows::Foundation::TimeSpan const& value);
        double Balance();
        void Balance(double value);

        static TsinghuaNetHelper::FluxUser Parse(hstring const& fluxstr);
    };
}

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct FluxUser : FluxUserT<FluxUser, implementation::FluxUser>
    {
    };
}

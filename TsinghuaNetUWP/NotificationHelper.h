#pragma once
#include "NetHelper.h"

namespace winrt::TsinghuaNetUWP
{
    winrt::hstring GetFluxString(std::uint64_t flux);
    winrt::hstring GetTimeSpanString(Windows::Foundation::TimeSpan const& time);
    winrt::hstring GetCurrencyString(double currency);
    constexpr std::uint64_t BaseFlux = 25000000000;
    std::uint64_t GetMaxFlux(FluxUser const& user);
    Windows::Foundation::IAsyncAction LoadTileTemplate();
    void UpdateTile(FluxUser const& user);
} // namespace winrt::TsinghuaNetUWP

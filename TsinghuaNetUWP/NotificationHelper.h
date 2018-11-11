#pragma once
#include "NetHelper.h"

namespace winrt::TsinghuaNetUWP
{
    winrt::hstring GetFluxString(std::uint64_t flux);
    winrt::hstring GetCurrencyString(double currency);
    void UpdateTile(FluxUser const& user);
}

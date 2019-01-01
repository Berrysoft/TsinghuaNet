#include "pch.h"

#include "FluxUser.h"

using namespace winrt;
using namespace Windows::Foundation;

namespace winrt::TsinghuaNetHelper::implementation
{
    DEPENDENCY_PROPERTY_INIT(Username, hstring, FluxUser, box_value(hstring{}))
    DEPENDENCY_PROPERTY_INIT(Flux, uint64_t, FluxUser, box_value<uint64_t>(0))
    DEPENDENCY_PROPERTY_INIT(OnlineTime, TimeSpan, FluxUser, box_value(TimeSpan{}))
    DEPENDENCY_PROPERTY_INIT(Balance, double, FluxUser, box_value<double>(0.0))
} // namespace winrt::TsinghuaNetHelper::implementation

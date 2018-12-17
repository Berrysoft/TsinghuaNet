#pragma once
#include "NetHelper.g.h"

#include "NetHelperBase.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct NetHelper : NetHelperT<NetHelper>, NetHelperBase
    {
        NetHelper() {}

        Windows::Foundation::IAsyncOperation<LogResponse> LoginAsync();
        Windows::Foundation::IAsyncOperation<LogResponse> LogoutAsync();
        Windows::Foundation::IAsyncOperation<FluxUser> FluxAsync();
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct NetHelper : NetHelperT<NetHelper, implementation::NetHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

#pragma once
#include "NetHelper.g.h"

#include "NetHelperBase.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct NetHelper : NetHelperT<NetHelper>, NetHelperBase
    {
        NetHelper(hstring const& username = {}, hstring const& password = {}) : NetHelperBase(username, password) {}

        Windows::Foundation::IAsyncOperation<LogResponse> LoginAsync();
        Windows::Foundation::IAsyncOperation<LogResponse> LogoutAsync();
        Windows::Foundation::IAsyncOperation<TsinghuaNetHelper::FluxUser> FluxAsync();
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct NetHelper : NetHelperT<NetHelper, implementation::NetHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

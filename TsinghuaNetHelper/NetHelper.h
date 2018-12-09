#pragma once
#include "NetHelper.g.h"

#include "NetHelperBase.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct NetHelper : NetHelperT<NetHelper>, NetHelperBase
    {
        NetHelper() {}

        Windows::Foundation::IAsyncOperation<hstring> LoginAsync();
        Windows::Foundation::IAsyncOperation<hstring> LogoutAsync();
        Windows::Foundation::IAsyncOperation<TsinghuaNetHelper::FluxUser> FluxAsync();
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct NetHelper : NetHelperT<NetHelper, implementation::NetHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

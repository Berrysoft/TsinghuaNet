#pragma once
#include "NetHelper.g.h"

#include "NetHelperBase.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct NetHelper : NetHelperT<NetHelper>
    {
        NetHelper() {}

        winrt::hstring Username() { return base.Username(); }
        void Username(winrt::hstring const& value) { base.Username(value); }
        winrt::hstring Password() { return base.Password(); }
        void Password(winrt::hstring const& value) { base.Password(value); }

        Windows::Foundation::IAsyncOperation<hstring> LoginAsync();
        Windows::Foundation::IAsyncOperation<hstring> LogoutAsync();
        Windows::Foundation::IAsyncOperation<TsinghuaNetHelper::FluxUser> FluxAsync();

    private:
        NetHelperBase base;
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct NetHelper : NetHelperT<NetHelper, implementation::NetHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

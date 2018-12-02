#pragma once
#include "Auth4Helper.g.h"

#include "AuthHelper.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct Auth4Helper : Auth4HelperT<Auth4Helper>
    {
        Auth4Helper() : base(4) {}

        winrt::hstring Username() { return base.Username(); }
        void Username(winrt::hstring const& value) { base.Username(value); }
        winrt::hstring Password() { return base.Password(); }
        void Password(winrt::hstring const& value) { base.Password(value); }

        Windows::Foundation::IAsyncOperation<hstring> LoginAsync() { return base.LoginAsync(); }
        Windows::Foundation::IAsyncOperation<hstring> LogoutAsync() { return base.LogoutAsync(); }
        Windows::Foundation::IAsyncOperation<TsinghuaNetHelper::FluxUser> FluxAsync() { return base.FluxAsync(); }

    private:
        AuthHelper base;
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct Auth4Helper : Auth4HelperT<Auth4Helper, implementation::Auth4Helper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

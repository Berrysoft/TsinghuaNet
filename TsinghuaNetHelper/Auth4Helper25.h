#pragma once
#include "Auth4Helper25.g.h"

#include "AuthHelper.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct Auth4Helper25 : Auth4Helper25T<Auth4Helper25>
    {
        Auth4Helper25() : base(4, 25) {}

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
    struct Auth4Helper25 : Auth4Helper25T<Auth4Helper25, implementation::Auth4Helper25>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

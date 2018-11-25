#pragma once

#include "AuthHelper.g.h"
#include "NetHelperBase.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct AuthHelper : AuthHelperT<AuthHelper, TsinghuaNetHelper::implementation::NetHelperBase>
    {
        AuthHelper() = delete;

        Windows::Foundation::IAsyncOperation<hstring> LoginAsync();
        Windows::Foundation::IAsyncOperation<hstring> LogoutAsync();
        Windows::Foundation::IAsyncOperation<TsinghuaNetHelper::FluxUser> FluxAsync();
    };
}

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct AuthHelper : AuthHelperT<AuthHelper, implementation::AuthHelper>
    {
    };
}

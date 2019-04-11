#pragma once
#include "Auth4Helper.g.h"

#include "AuthHelper.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct Auth4Helper : Auth4HelperT<Auth4Helper>, AuthHelper
    {
        Auth4Helper(hstring const& username = {}, hstring const& password = {}) : AuthHelper(4, username, password) {}
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct Auth4Helper : Auth4HelperT<Auth4Helper, implementation::Auth4Helper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

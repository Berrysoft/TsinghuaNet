#pragma once
#include "Auth4Helper25.g.h"

#include "AuthHelper.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct Auth4Helper25 : Auth4Helper25T<Auth4Helper25>, AuthHelper
    {
        Auth4Helper25(hstring const& username = {}, hstring const& password = {}) : AuthHelper(4, 33, username, password) {}
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct Auth4Helper25 : Auth4Helper25T<Auth4Helper25, implementation::Auth4Helper25>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

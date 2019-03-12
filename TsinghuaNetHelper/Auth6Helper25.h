#pragma once
#include "Auth6Helper25.g.h"

#include "AuthHelper.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct Auth6Helper25 : Auth6Helper25T<Auth6Helper25>, AuthHelper
    {
        Auth6Helper25(hstring const& username = {}, hstring const& password = {}) : AuthHelper(6, 25, username, password) {}
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct Auth6Helper25 : Auth6Helper25T<Auth6Helper25, implementation::Auth6Helper25>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

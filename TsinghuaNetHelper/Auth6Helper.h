#pragma once
#include "Auth6Helper.g.h"

#include "AuthHelper.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct Auth6Helper : Auth6HelperT<Auth6Helper>, AuthHelper
    {
        Auth6Helper() : AuthHelper(6) {}
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct Auth6Helper : Auth6HelperT<Auth6Helper, implementation::Auth6Helper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

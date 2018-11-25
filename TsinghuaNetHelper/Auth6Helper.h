#pragma once

#include "Auth6Helper.g.h"
#include "AuthHelper.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct Auth6Helper : Auth6HelperT<Auth6Helper, TsinghuaNetHelper::implementation::AuthHelper>
    {
        Auth6Helper() = default;

    };
}

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct Auth6Helper : Auth6HelperT<Auth6Helper, implementation::Auth6Helper>
    {
    };
}

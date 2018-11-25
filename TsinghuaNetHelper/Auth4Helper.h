#pragma once

#include "Auth4Helper.g.h"
#include "AuthHelper.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct Auth4Helper : Auth4HelperT<Auth4Helper, TsinghuaNetHelper::implementation::AuthHelper>
    {
        Auth4Helper() = default;

    };
}

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct Auth4Helper : Auth4HelperT<Auth4Helper, implementation::Auth4Helper>
    {
    };
}

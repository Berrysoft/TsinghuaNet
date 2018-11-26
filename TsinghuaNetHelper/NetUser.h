#pragma once
#include "NetUser.g.h"

#include "Utility.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct NetUser : NetUserT<NetUser>
    {
        NetUser() = default;

        PROP_DECL_REF(Address, winrt::hstring)
        PROP_DECL_REF(LoginTime, winrt::hstring)
        PROP_DECL_REF(Client, winrt::hstring)
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct NetUser : NetUserT<NetUser, implementation::NetUser>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

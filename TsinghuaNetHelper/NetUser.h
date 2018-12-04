#pragma once
#include "NetUser.g.h"

#include "Utility.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct NetUser : NetUserT<NetUser>
    {
        NetUser() = default;

        DEPENDENCY_PROPERTY(Address, winrt::hstring)
        DEPENDENCY_PROPERTY(LoginTime, winrt::hstring)
        DEPENDENCY_PROPERTY(Client, winrt::hstring)
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct NetUser : NetUserT<NetUser, implementation::NetUser>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

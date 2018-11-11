#pragma once

#include "NetUserModel.g.h"

#include "DependencyHelper.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct NetUserModel : NetUserModelT<NetUserModel>
    {
        NetUserModel() = default;

        DEPENDENCY_PROPERTY(Address, winrt::hstring)
        DEPENDENCY_PROPERTY(LoginTime, winrt::hstring)
        DEPENDENCY_PROPERTY(Client, winrt::hstring)
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct NetUserModel : NetUserModelT<NetUserModel, implementation::NetUserModel>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

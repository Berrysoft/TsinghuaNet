#pragma once

#include "NetUser.g.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct NetUser : NetUserT<NetUser>
    {
        NetUser() = default;

        hstring Address();
        void Address(hstring const& value);
        hstring LoginTime();
        void LoginTime(hstring const& value);
        hstring Client();
        void Client(hstring const& value);
    };
}

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct NetUser : NetUserT<NetUser, implementation::NetUser>
    {
    };
}

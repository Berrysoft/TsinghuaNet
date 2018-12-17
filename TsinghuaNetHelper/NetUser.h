#pragma once
#include "NetUser.g.h"

#include "Utility.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct NetUser : NetUserT<NetUser>
    {
        NetUser() = default;

        void Drop() { m_DropUserEvent(*this, Address()); }
        bool Equals(TsinghuaNetHelper::NetUser const& user);

        DEPENDENCY_PROPERTY(Address, hstring)
        DEPENDENCY_PROPERTY(LoginTime, hstring)
        DEPENDENCY_PROPERTY(Client, hstring)

        EVENT_DECL(DropUser, hstring)
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct NetUser : NetUserT<NetUser, implementation::NetUser>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

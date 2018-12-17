#include "pch.h"

#include "NetUser.h"
#include <winrt/Windows.UI.Xaml.Interop.h>

using namespace winrt;

namespace winrt::TsinghuaNetHelper::implementation
{
    bool NetUser::Equals(TsinghuaNetHelper::NetUser const& user)
    {
        return Address() == user.Address() && LoginTime() == user.LoginTime() && Client() == user.Client();
    }

    DEPENDENCY_PROPERTY_INIT(Address, hstring, NetUser, box_value(hstring()))
    DEPENDENCY_PROPERTY_INIT(LoginTime, hstring, NetUser, box_value(hstring()))
    DEPENDENCY_PROPERTY_INIT(Client, hstring, NetUser, box_value(hstring()))
} // namespace winrt::TsinghuaNetHelper::implementation

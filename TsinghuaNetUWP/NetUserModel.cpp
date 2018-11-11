#include "pch.h"

#include "NetUserModel.h"

using namespace winrt;

namespace winrt::TsinghuaNetUWP::implementation
{
    DEPENDENCY_PROPERTY_INIT(Address, hstring, NetUserModel, TsinghuaNetUWP::NetUserModel, box_value(hstring()))
    DEPENDENCY_PROPERTY_INIT(LoginTime, hstring, NetUserModel, TsinghuaNetUWP::NetUserModel, box_value(hstring()))
    DEPENDENCY_PROPERTY_INIT(Client, hstring, NetUserModel, TsinghuaNetUWP::NetUserModel, box_value(hstring()))
} // namespace winrt::TsinghuaNetUWP::implementation

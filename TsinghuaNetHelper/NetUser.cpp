#include "pch.h"

#include "NetUser.h"
#include <winrt/Windows.UI.Xaml.Interop.h>

using namespace winrt;

namespace winrt::TsinghuaNetHelper::implementation
{
    DEPENDENCY_PROPERTY_INIT(Address, hstring, NetUser, TsinghuaNetHelper, box_value(hstring()))
    DEPENDENCY_PROPERTY_INIT(LoginTime, hstring, NetUser, TsinghuaNetHelper, box_value(hstring()))
    DEPENDENCY_PROPERTY_INIT(Client, hstring, NetUser, TsinghuaNetHelper, box_value(hstring()))
} // namespace winrt::TsinghuaNetHelper::implementation

#pragma once
#include "SettingsHelper.g.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct SettingsHelper : SettingsHelperT<SettingsHelper>
    {
        SettingsHelper() = default;

        hstring StoredUsername();
        void StoredUsername(hstring const& value);
        bool AutoLogin();
        void AutoLogin(bool value);
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct SettingsHelper : SettingsHelperT<SettingsHelper, implementation::SettingsHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

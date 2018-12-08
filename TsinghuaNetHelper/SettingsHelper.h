#pragma once
#include "SettingsHelper.g.h"

#include <winrt/Windows.Data.Json.h>

namespace winrt::TsinghuaNetHelper::implementation
{
    struct SettingsHelper : SettingsHelperT<SettingsHelper>
    {
        SettingsHelper();
        virtual ~SettingsHelper();

        hstring StoredUsername();
        void StoredUsername(hstring const& value);
        bool AutoLogin();
        void AutoLogin(bool value);
        bool BackgroundAutoLogin();
        void BackgroundAutoLogin(bool value);
        bool BackgroundLiveTile();
        void BackgroundLiveTile(bool value);
        NetState LanState();
        void LanState(NetState value);
        NetState WwanState();
        void WwanState(NetState value);
        NetState WlanState(hstring const& ssid);
        void WlanState(hstring const& ssid, NetState value);

        bool InternetAvailable();
        InternetStatus InternetStatus(hstring& ssid);

    private:
        Windows::Data::Json::JsonObject wlanMap;
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct SettingsHelper : SettingsHelperT<SettingsHelper, implementation::SettingsHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

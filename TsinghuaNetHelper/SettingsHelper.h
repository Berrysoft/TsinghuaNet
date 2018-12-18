#pragma once
#include "SettingsHelper.g.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct SettingsHelper : SettingsHelperT<SettingsHelper>
    {
        SettingsHelper();

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
        Windows::Foundation::Collections::IMap<hstring, NetState> WlanStates();
        void WlanStates(Windows::Foundation::Collections::IMap<hstring, NetState> const& states);

        bool InternetAvailable();
        InternetStatus InternetStatus(hstring& ssid);

    private:
        Windows::Data::Json::JsonObject wlanMap;

        Windows::Data::Json::JsonObject WlanStateInternal();
        void WlanStateInternal(Windows::Data::Json::JsonObject const& value);
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct SettingsHelper : SettingsHelperT<SettingsHelper, implementation::SettingsHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

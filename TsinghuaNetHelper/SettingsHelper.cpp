#include "pch.h"

#include "SettingsHelper.h"
#include <winrt/Windows.Networking.Connectivity.h>
#include <winrt/Windows.Storage.h>

using namespace winrt;
using namespace Windows::Data::Json;
using namespace Windows::Networking::Connectivity;
using namespace Windows::Storage;

namespace winrt::TsinghuaNetHelper::implementation
{
    template <typename T>
    T GetValue(hstring const& key, T def)
    {
        auto settings = ApplicationData::Current().LocalSettings();
        auto values = settings.Values();
        auto value = values.TryLookup(key);
        if (value)
        {
            return unbox_value<T>(value);
        }
        return def;
    }

    template <typename T>
    void SetValue(hstring const& key, T value)
    {
        auto settings = ApplicationData::Current().LocalSettings();
        auto values = settings.Values();
        values.Insert(key, box_value(value));
    }

    constexpr wchar_t WlanStateKey[] = L"WlanState";
    constexpr wchar_t DefWlanState[] = LR"({
	"Tsinghua": 3,
	"Tsinghua-5G": 3,
	"Tsinghua-IPv4": 4,
	"Tsinghua-IPv6": 5,
	"Wifi.郑裕彤讲堂": 3,
	"DIVI": 6,
	"DIVI-2": 6,
	"IVI": 6
})";

    SettingsHelper::SettingsHelper()
    {
        hstring json = GetValue<hstring>(WlanStateKey, DefWlanState);
        wlanMap = JsonObject::Parse(json);
    }

    SettingsHelper::~SettingsHelper()
    {
        SetValue(WlanStateKey, wlanMap.ToString());
    }

#define SETTINGS_PROP_IMPL(name, key, type, def)                           \
    constexpr wchar_t name##Key[] = key;                                   \
    type SettingsHelper::name() { return GetValue<type>(name##Key, def); } \
    void SettingsHelper::name(type value) { SetValue(name##Key, value); }

#define SETTINGS_PROP_REF_IMPL(name, key, type, def)                       \
    constexpr wchar_t name##Key[] = key;                                   \
    type SettingsHelper::name() { return GetValue<type>(name##Key, def); } \
    void SettingsHelper::name(type const& value) { SetValue(name##Key, value); }

#define SETTINGS_PROP_CONV_IMPL(name, key, type, ctype, def)                      \
    constexpr wchar_t name##Key[] = key;                                          \
    type SettingsHelper::name() { return (type)GetValue<ctype>(name##Key, def); } \
    void SettingsHelper::name(type value) { SetValue<ctype>(name##Key, (ctype)value); }

    SETTINGS_PROP_REF_IMPL(StoredUsername, L"Username", hstring, {})
    SETTINGS_PROP_IMPL(AutoLogin, L"AutoLogin", bool, true)
    SETTINGS_PROP_IMPL(BackgroundAutoLogin, L"BackgroundAutoLogin", bool, true)
    SETTINGS_PROP_IMPL(BackgroundLiveTile, L"BackgroundLiveTile", bool, true)
    SETTINGS_PROP_CONV_IMPL(LanState, L"LanState", NetState, int, (int)NetState::Auth4)
    SETTINGS_PROP_CONV_IMPL(WwanState, L"WwanState", NetState, int, (int)NetState::Direct)

    NetState SettingsHelper::WlanState(hstring const& ssid)
    {
        if (wlanMap.HasKey(ssid))
        {
            return (NetState)(int)wlanMap.GetNamedNumber(ssid);
        }
        return NetState::Unknown;
    }

    void SettingsHelper::WlanState(hstring const& ssid, NetState value)
    {
        wlanMap.Insert(ssid, JsonValue::CreateNumberValue((int)value));
    }

    bool SettingsHelper::InternetAvailable()
    {
        auto profile = NetworkInformation::GetInternetConnectionProfile();
        if (!profile)
            return false;
        return profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel::InternetAccess;
    }

    InternetStatus SettingsHelper::InternetStatus(hstring& ssid)
    {
        auto profile = NetworkInformation::GetInternetConnectionProfile();
        if (!profile)
            return InternetStatus::Unknown;
        auto cl = profile.GetNetworkConnectivityLevel();
        if (cl == NetworkConnectivityLevel::None)
            return InternetStatus::None;
        if (profile.IsWwanConnectionProfile())
        {
            return InternetStatus::Wwan;
        }
        else if (profile.IsWlanConnectionProfile())
        {
            auto details = profile.WlanConnectionProfileDetails();
            ssid = details.GetConnectedSsid();
            return InternetStatus::Wlan;
        }
        else
        {
            return InternetStatus::Lan;
        }
    }
} // namespace winrt::TsinghuaNetHelper::implementation

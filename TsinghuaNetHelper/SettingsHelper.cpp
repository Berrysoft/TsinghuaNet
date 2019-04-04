#include "pch.h"

#include "SettingsHelper.h"
#include "winrt/Windows.Networking.Connectivity.h"
#include "winrt/Windows.Storage.h"

using namespace std;
using namespace winrt;
using namespace Windows::Data::Json;
using namespace Windows::Foundation::Collections;
using namespace Windows::Networking::Connectivity;
using namespace Windows::Storage;

namespace winrt::TsinghuaNetHelper::implementation
{
    template <typename T>
    T SettingsHelper::GetValue(wstring_view key, T def)
    {
        auto value{ values.TryLookup(key) };
        if (value)
        {
            return unbox_value<T>(value);
        }
        return def;
    }

    template <typename T>
    void SettingsHelper::SetValue(wstring_view key, T value)
    {
        values.Insert(key, box_value(value));
    }

    IMap<hstring, NetState> GetMapFromJson(JsonObject const& json)
    {
        auto result{ single_threaded_map<hstring, NetState>() };
        for (auto pair : json)
        {
            result.Insert(pair.Key(), (NetState)(int)pair.Value().GetNumber());
        }
        return result;
    }

    JsonObject GetJsonFromMap(IMap<hstring, NetState> const& map)
    {
        JsonObject result;
        for (auto pair : map)
        {
            result.Insert(pair.Key(), JsonValue::CreateNumberValue((int)pair.Value()));
        }
        return result;
    }

    constexpr wstring_view StoredUsernameKey{ L"Username" };
    constexpr wstring_view AutoLoginKey{ L"AutoLogin" };
    constexpr wstring_view BackgroundAutoLoginKey{ L"BackgroundAutoLogin" };
    constexpr wstring_view BackgroundLiveTileKey{ L"BackgroundLiveTile" };
    constexpr wstring_view LanStateKey{ L"LanState" };
    constexpr wstring_view WwanStateKey{ L"WwanState" };
    constexpr wstring_view WlanStateKey{ L"WlanState" };
    constexpr wstring_view ThemeKey{ L"Theme" };
    constexpr wstring_view ContentTypeKey{ L"UserContentType" };

    SettingsHelper::SettingsHelper() : values(ApplicationData::Current().LocalSettings().Values())
    {
        m_StoredUsername = GetValue<hstring>(StoredUsernameKey);
        m_AutoLogin = GetValue<bool>(AutoLoginKey, true);
        m_BackgroundAutoLogin = GetValue<bool>(BackgroundAutoLoginKey, true);
        m_BackgroundLiveTile = GetValue<bool>(BackgroundLiveTileKey, true);
        m_LanState = (NetState)GetValue<int>(LanStateKey, (int)NetState::Auth4);
        m_WwanState = (NetState)GetValue<int>(WwanStateKey, (int)NetState::Unknown);
        m_Theme = (UserTheme)GetValue<int>(ThemeKey, (int)UserTheme::Default);
        m_ContentType = (UserContentType)GetValue<int>(ContentTypeKey, (int)UserContentType::Ring);
        hstring json{ GetValue<hstring>(WlanStateKey) };
        if (json.empty())
        {
            wlanMap = GetJsonFromMap(DefWlanStates());
        }
        else
        {
            wlanMap = JsonObject::Parse(json);
        }
    }

    void SettingsHelper::SaveSettings()
    {
        SetValue(StoredUsernameKey, m_StoredUsername);
        SetValue(AutoLoginKey, m_AutoLogin);
        SetValue(BackgroundAutoLoginKey, m_BackgroundAutoLogin);
        SetValue(BackgroundLiveTileKey, m_BackgroundLiveTile);
        SetValue(LanStateKey, (int)m_LanState);
        SetValue(WwanStateKey, (int)m_WwanState);
        SetValue(ThemeKey, (int)m_Theme);
        SetValue(ContentTypeKey, (int)m_ContentType);
        SetValue(WlanStateKey, wlanMap.ToString());
    }

    NetState SettingsHelper::WlanState(hstring const& ssid)
    {
        if (wlanMap.HasKey(ssid))
        {
            return (NetState)(int)wlanMap.GetNamedNumber(ssid);
        }
        return NetState::Unknown;
    }

    IMap<hstring, NetState> SettingsHelper::WlanStates()
    {
        return GetMapFromJson(wlanMap);
    }

    void SettingsHelper::WlanStates(IMap<hstring, NetState> const& states)
    {
        wlanMap = GetJsonFromMap(states);
    }

    IMap<hstring, NetState> SettingsHelper::DefWlanStates()
    {
        return single_threaded_map(map<hstring, NetState>{
            { L"Tsinghua", NetState::Net },
            { L"Tsinghua-5G", NetState::Net },
            { L"Tsinghua-IPv4", NetState::Auth4_25 },
            { L"Wifi.郑裕彤讲堂", NetState::Net } });
    }

    bool SettingsHelper::InternetAvailable()
    {
        auto profile{ NetworkInformation::GetInternetConnectionProfile() };
        if (!profile)
            return false;
        return profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel::InternetAccess;
    }

    NetState SettingsHelper::SuggestNetState(TsinghuaNetHelper::InternetStatus status, hstring const& ssid)
    {
        switch (status)
        {
        case InternetStatus::Lan:
            return LanState();
        case InternetStatus::Wwan:
            return WwanState();
        case InternetStatus::Wlan:
            return WlanState(ssid);
        default:
            return NetState::Unknown;
        }
    }

    InternetStatus SettingsHelper::InternetStatus(hstring& ssid)
    {
        auto profile{ NetworkInformation::GetInternetConnectionProfile() };
        if (!profile)
            return InternetStatus::Unknown;
        auto cl{ profile.GetNetworkConnectivityLevel() };
        if (cl == NetworkConnectivityLevel::None)
            return InternetStatus::Unknown;
        if (profile.IsWwanConnectionProfile())
        {
            return InternetStatus::Wwan;
        }
        else if (profile.IsWlanConnectionProfile())
        {
            auto details{ profile.WlanConnectionProfileDetails() };
            ssid = details.GetConnectedSsid();
            return InternetStatus::Wlan;
        }
        else
        {
            return InternetStatus::Lan;
        }
    }
} // namespace winrt::TsinghuaNetHelper::implementation

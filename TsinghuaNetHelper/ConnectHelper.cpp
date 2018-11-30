#include "pch.h"

#include "ConnectHelper.h"

using namespace winrt;

namespace winrt::TsinghuaNetHelper
{
    NetState GetSuggestNetState(SettingsHelper const& lan)
    {
        hstring ssid;
        auto status = lan.GetCurrentInternetStatus(ssid);
        switch (status)
        {
        case InternetStatus::Lan:
            return lan.LanState();
        case InternetStatus::Wwan:
            return lan.WwanState();
        case InternetStatus::Wlan:
            return lan.WlanState(ssid);
        default:
            return NetState::Unknown;
        }
    }

    IConnect GetHelper(NetState state)
    {
        switch (state)
        {
        case NetState::Auth4:
            return Auth4Helper();
        case NetState::Auth6:
            return Auth6Helper();
        case NetState::Net:
            return NetHelper();
        default:
            return nullptr;
        }
    }

    template <typename T>
    T MakeHelper(hstring const& username, hstring const& password)
    {
        T result;
        result.Username(username);
        result.Password(password);
        return result;
    }
    IConnect GetHelper(NetState state, hstring const& username, hstring const& password)
    {
        switch (state)
        {
        case NetState::Auth4:
            return MakeHelper<Auth4Helper>(username, password);
        case NetState::Auth6:
            return MakeHelper<Auth6Helper>(username, password);
        case NetState::Net:
            return MakeHelper<NetHelper>(username, password);
        default:
            return nullptr;
        }
    }
} // namespace winrt::TsinghuaNetHelper

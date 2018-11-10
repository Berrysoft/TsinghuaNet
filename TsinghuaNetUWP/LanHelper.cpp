#include "pch.h"

#include "LanHelper.h"
#include <map>
#include <string>
#include <winrt/Windows.Networking.Connectivity.h>

using namespace std;
using namespace winrt;
using namespace Windows::Networking::Connectivity;

namespace winrt::TsinghuaNetUWP
{
    tuple<InternetStatus, hstring> GetCurrentInternetStatus()
    {
        auto profile = NetworkInformation::GetInternetConnectionProfile();
        if (profile == nullptr)
            return make_tuple(InternetStatus::Unknown, hstring());
        auto cl = profile.GetNetworkConnectivityLevel();
        if (cl == NetworkConnectivityLevel::None)
            return make_tuple(InternetStatus::None, hstring());
        if (profile.IsWwanConnectionProfile())
        {
            return make_tuple(InternetStatus::Wwan, hstring());
        }
        else if (profile.IsWlanConnectionProfile())
        {
            auto details = profile.WlanConnectionProfileDetails();
            return make_tuple(InternetStatus::Wlan, details.GetConnectedSsid());
        }
        else
        {
            return make_tuple(InternetStatus::Lan, hstring());
        }
    }

    const map<wstring, NetState> SsidStateMap =
        {
            { L"Tsinghua", NetState::Net },
            { L"Tsinghua-5G", NetState::Net },
            { L"Tsinghua-IPv4", NetState::Auth4 },
            { L"Tsinghua-IPv6", NetState::Auth6 }
        };

    NetState GetWlanState(hstring const& ssid)
    {
        auto it = SsidStateMap.find(wstring(ssid));
        if (it == SsidStateMap.end())
            return NetState::Unknown;
        return it->second;
    }
} // namespace winrt::TsinghuaNetUWP

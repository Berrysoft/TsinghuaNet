#include "pch.h"

#include "LanHelper.h"
#include <winrt/Windows.Data.Json.h>
#include <winrt/Windows.Networking.Connectivity.h>

using namespace std;
using namespace winrt;
using namespace Windows::Data::Json;
using namespace Windows::Foundation;
using namespace Windows::Networking::Connectivity;
using namespace Windows::Storage;

namespace winrt::TsinghuaNetHelper::implementation
{
    TsinghuaNetHelper::NetState LanHelper::WlanState(hstring const& ssid)
    {
        auto it = ssidStateMap.find(wstring(ssid));
        if (it == ssidStateMap.end())
            return NetState::Unknown;
        return it->second;
    }

    IAsyncOperation<TsinghuaNetHelper::LanHelper> LanHelper::LoadAsync()
    {
        auto result = make<LanHelper>();
        LanHelper* presult = get_self<LanHelper>(result);
        StorageFolder folder = ApplicationData::Current().RoamingFolder();
        bool create = false;
        StorageFile file(nullptr);
        try
        {
            file = co_await folder.GetFileAsync(L"states.json");
            create = false;
        }
        catch (hresult_error const&)
        {
            create = true;
        }
        hstring json;
        if (file)
            json = co_await FileIO::ReadTextAsync(file);
        else
            json = co_await FileIO::ReadTextAsync(co_await StorageFile::StorageFile::GetFileFromApplicationUriAsync(Uri(L"ms-appx:///TsinghuaNetHelper/states.json")));
        auto obj = JsonObject::Parse(json);
        presult->lanState = (NetState)(int32_t)(obj.GetNamedNumber(L"lan"));
        presult->wwanState = (NetState)(int32_t)(obj.GetNamedNumber(L"wwan"));
        auto arr = obj.GetNamedObject(L"wlan");
        for (auto pair : arr)
        {
            presult->ssidStateMap.emplace(wstring(pair.Key()), (NetState)(int32_t)(pair.Value().GetNumber()));
        }
        if (create)
        {
            file = co_await folder.CreateFileAsync(L"states.json");
            co_await FileIO::WriteTextAsync(file, json);
        }
        return result;
    }

    TsinghuaNetHelper::InternetStatus LanHelper::GetCurrentInternetStatus(hstring& ssid)
    {
        auto profile = NetworkInformation::GetInternetConnectionProfile();
        if (profile == nullptr)
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

#pragma once
#include "LanHelper.g.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct LanHelper : LanHelperT<LanHelper>
    {
        LanHelper() = default;

        TsinghuaNetHelper::NetState LanState() { return lanState; }
        TsinghuaNetHelper::NetState WwanState() { return wwanState; }
        TsinghuaNetHelper::NetState WlanState(hstring const& ssid);

        static Windows::Foundation::IAsyncOperation<TsinghuaNetHelper::LanHelper> LoadAsync();
        static TsinghuaNetHelper::InternetStatus GetCurrentInternetStatus(hstring& ssid);

    private:
        NetState lanState;
        NetState wwanState;
        std::map<std::wstring, NetState> ssidStateMap;
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct LanHelper : LanHelperT<LanHelper, implementation::LanHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

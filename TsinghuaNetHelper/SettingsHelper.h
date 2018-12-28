#pragma once
#include "SettingsHelper.g.h"

#include "../Shared/Utility.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct SettingsHelper : SettingsHelperT<SettingsHelper>
    {
        SettingsHelper();

        void SaveSettings();

        PROP_DECL_REF(StoredUsername, hstring)
        PROP_DECL(AutoLogin, bool)
        PROP_DECL(BackgroundAutoLogin, bool)
        PROP_DECL(BackgroundLiveTile, bool)
        PROP_DECL_REF(LanState, NetState)
        PROP_DECL_REF(WwanState, NetState)

        PROP_DECL_REF(Theme, Windows::UI::Xaml::ElementTheme)
        PROP_DECL_REF(ContentType, UserContentType)

    public:
        NetState WlanState(hstring const& ssid);
        Windows::Foundation::Collections::IMap<hstring, NetState> WlanStates();
        void WlanStates(Windows::Foundation::Collections::IMap<hstring, NetState> const& states);

        NetState SuggestNetState(InternetStatus status, hstring const& ssid);

        static Windows::Foundation::Collections::IMap<hstring, NetState> DefWlanStates();

        static bool InternetAvailable();
        static InternetStatus InternetStatus(hstring& ssid);

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

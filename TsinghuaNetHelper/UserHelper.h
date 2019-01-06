#pragma once
#include "UserHelper.g.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct UserHelper
    {
        UserHelper() = delete;

        static hstring GetFluxString(uint64_t flux);
        static hstring GetTimeSpanString(Windows::Foundation::TimeSpan const& time);
        static hstring GetCurrencyString(double currency);
        static hstring GetNetStateString(NetState state);
        static hstring GetInternetStatusString(InternetStatus status);
        static uint64_t GetMaxFlux(uint64_t flux, double balance);
        static FluxUser GetFluxUser(hstring const& str);
        static LogResponse GetLogResponse(hstring const& str);
        static LogResponse GetAuthLogResponse(hstring const& str, bool login);
        static hstring GetResponseString(LogResponse const& response);
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct UserHelper : UserHelperT<UserHelper, implementation::UserHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

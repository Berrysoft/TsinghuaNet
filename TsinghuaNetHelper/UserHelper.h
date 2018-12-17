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
        static uint64_t GetMaxFlux(FluxUser const& user);
        static FluxUser GetFluxUser(winrt::hstring const& str);
        static LogResponse GetLogResponse(winrt::hstring const& str);
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct UserHelper : UserHelperT<UserHelper, implementation::UserHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

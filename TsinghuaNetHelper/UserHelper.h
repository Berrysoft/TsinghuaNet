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
        static uint64_t GetMaxFlux(TsinghuaNetHelper::FluxUser const& user);
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct UserHelper : UserHelperT<UserHelper, implementation::UserHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

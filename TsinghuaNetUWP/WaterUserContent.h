#pragma once
#include "WaterUserContent.g.h"

#include "../Shared/Utility.h"
#include "UserContentHelper.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct WaterUserContent : WaterUserContentT<WaterUserContent>
    {
        WaterUserContent();

        double Max(double d1, double d2) const { return std::max(d1, d2); }
        bool IsProgressActive() { return Progress().IsActive(); }
        void IsProgressActive(bool value) { Progress().IsActive(value); }
        void BeginAnimation() { FluxStoryboard().Begin(); }
        bool AddOneSecond() { return AddOneSecondH(*this); }

        DEPENDENCY_PROPERTY(User, TsinghuaNetHelper::FluxUser)
        DEPENDENCY_PROPERTY(OnlineTime, Windows::Foundation::TimeSpan)
        DEPENDENCY_PROPERTY(FreeOffset, double)
        DEPENDENCY_PROPERTY(FluxOffset, double)
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct WaterUserContent : WaterUserContentT<WaterUserContent, implementation::WaterUserContent>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

#pragma once
#include "LineUserContent.g.h"

#include "../Shared/Utility.h"
#include "UserContentHelper.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct LineUserContent : LineUserContentT<LineUserContent>
    {
        LineUserContent();

        double Max(double d1, double d2) const { return std::max(d1, d2); }
        bool IsProgressActive() { return Progress().IsIndeterminate(); }
        void IsProgressActive(bool value);
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
    struct LineUserContent : LineUserContentT<LineUserContent, implementation::LineUserContent>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

#pragma once
#include "WaterUserContent.g.h"

#include "../Shared/Utility.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct WaterUserContent : WaterUserContentT<WaterUserContent>
    {
        WaterUserContent();

        double Max(double d1, double d2) { return std::max(d1, d2); }

        DEPENDENCY_PROPERTY(User, TsinghuaNetHelper::FluxUser)
        DEPENDENCY_PROPERTY(OnlineTime, Windows::Foundation::TimeSpan)
        DEPENDENCY_PROPERTY(FreePercent, double)
        DEPENDENCY_PROPERTY(FluxPercent, double)
        DEPENDENCY_PROPERTY(FreeOffset, double)
        DEPENDENCY_PROPERTY(FluxOffset, double)

    public:
        bool IsProgressActive() { return Progress().IsActive(); }
        void IsProgressActive(bool value) { Progress().IsActive(value); }
        void BeginAnimation() { FluxStoryboard().Begin(); }
        bool AddOneSecond();

    private:
        static void OnUserPropertyChanged(Windows::UI::Xaml::DependencyObject const& d, Windows::UI::Xaml::DependencyPropertyChangedEventArgs const& e);
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct WaterUserContent : WaterUserContentT<WaterUserContent, implementation::WaterUserContent>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

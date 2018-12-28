#pragma once
#include "WaterUserContent.g.h"

#include "../Shared/Utility.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct WaterUserContent : WaterUserContentT<WaterUserContent>
    {
        WaterUserContent();

        DEPENDENCY_PROPERTY(User, TsinghuaNetHelper::FluxUser)
        DEPENDENCY_PROPERTY(OnlineTime, Windows::Foundation::TimeSpan)
        DEPENDENCY_PROPERTY(FluxPercent, double)

    public:
        bool IsProgressActive() { return false; }
        void IsProgressActive(bool value) {}
        void BeginAnimation() {}
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

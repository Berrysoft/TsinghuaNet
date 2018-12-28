#pragma once
#include "LineUserContent.g.h"

#include "../Shared/Utility.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct LineUserContent : LineUserContentT<LineUserContent>
    {
        LineUserContent();

        DEPENDENCY_PROPERTY(User, TsinghuaNetHelper::FluxUser)
        DEPENDENCY_PROPERTY(OnlineTime, Windows::Foundation::TimeSpan)
        DEPENDENCY_PROPERTY(FluxPercent, double)

    public:
        bool IsProgressActive() { return Progress().IsIndeterminate(); }
        void IsProgressActive(bool value) { Progress().IsIndeterminate(value); }
        void BeginAnimation() {}
        bool AddOneSecond();

    private:
        static void OnUserPropertyChanged(Windows::UI::Xaml::DependencyObject const& d, Windows::UI::Xaml::DependencyPropertyChangedEventArgs const& e);
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct LineUserContent : LineUserContentT<LineUserContent, implementation::LineUserContent>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

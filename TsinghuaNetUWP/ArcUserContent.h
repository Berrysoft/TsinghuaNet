#pragma once
#include "ArcUserContent.g.h"

#include "../Shared/Utility.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct ArcUserContent : ArcUserContentT<ArcUserContent>
    {
        ArcUserContent();

        DEPENDENCY_PROPERTY(User, TsinghuaNetHelper::FluxUser)
        DEPENDENCY_PROPERTY(OnlineTime, Windows::Foundation::TimeSpan)
        DEPENDENCY_PROPERTY(FreePercent, double)
        DEPENDENCY_PROPERTY(FluxPercent, double)

    public:
        bool IsProgressActive() { return Progress().IsActive(); }
        void IsProgressActive(bool value) { Progress().IsActive(value); }
        void BeginAnimation() { FluxStoryboard().Begin(); }
        bool AddOneSecond() { return false; }

    private:
        static void OnUserPropertyChanged(Windows::UI::Xaml::DependencyObject const& d, Windows::UI::Xaml::DependencyPropertyChangedEventArgs const& e);
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct ArcUserContent : ArcUserContentT<ArcUserContent, implementation::ArcUserContent>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

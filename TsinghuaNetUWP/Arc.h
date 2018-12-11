#pragma once
#include "Arc.g.h"

#include "DependencyHelper.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct Arc : ArcT<Arc>
    {
    public:
        Arc();

        double Radius() const { return m_Radius; }
        void DrawArc();

        DEPENDENCY_PROPERTY(Thickness, double)
        DEPENDENCY_PROPERTY(Fill, Windows::UI::Xaml::Media::Brush)
        DEPENDENCY_PROPERTY(Value, double)

    private:
        double m_Radius;

        static void OnSizePropertyChanged(Windows::UI::Xaml::DependencyObject const& d, Windows::UI::Xaml::DependencyPropertyChangedEventArgs const&);

        double GetAngle();
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct Arc : ArcT<Arc, implementation::Arc>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

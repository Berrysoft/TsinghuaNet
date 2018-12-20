#pragma once
#include "Arc.g.h"

#include "../Shared/Utility.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct Arc : ArcT<Arc>
    {
    public:
        Arc();

        double Radius() const { return m_Radius; }

        void OnLoaded(Windows::Foundation::IInspectable const&, Windows::UI::Xaml::RoutedEventArgs const&) { DrawArc(); }
        void OnSizeChanged(Windows::Foundation::IInspectable const&, Windows::UI::Xaml::SizeChangedEventArgs const&) { DrawArc(); }

        DEPENDENCY_PROPERTY(Thickness, double)
        DEPENDENCY_PROPERTY(Fill, Windows::UI::Xaml::Media::Brush)
        DEPENDENCY_PROPERTY(Value, double)

    private:
        double m_Radius;

        static void OnSizePropertyChanged(Windows::UI::Xaml::DependencyObject const& d, Windows::UI::Xaml::DependencyPropertyChangedEventArgs const&);

        void DrawArc();
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct Arc : ArcT<Arc, implementation::Arc>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

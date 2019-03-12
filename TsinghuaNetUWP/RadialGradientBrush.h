#pragma once
#include "RadialGradientBrush.g.h"

#include "../Shared/Utility.h"
#include "winrt/Microsoft.Graphics.Canvas.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct RadialGradientBrush : RadialGradientBrushT<RadialGradientBrush>
    {
        RadialGradientBrush();

        void OnConnected();
        void OnDisconnected();

    private:
        Microsoft::Graphics::Canvas::CanvasDevice _device{ nullptr };
        Windows::UI::Composition::CompositionGraphicsDevice _graphics{ nullptr };

        bool OnDraw(Microsoft::Graphics::Canvas::CanvasDevice const& device, Microsoft::Graphics::Canvas::CanvasDrawingSession const& session, Windows::Foundation::Numerics::float2 size);
        void ReDraw();

        void CanvasDevice_RenderingDeviceReplaced(Windows::UI::Composition::CompositionGraphicsDevice const&, Windows::Foundation::IInspectable const&) { ReDraw(); }
        event_token token_RenderingDeviceReplaced;
        void CanvasDevice_DeviceLost(Microsoft::Graphics::Canvas::CanvasDevice const&, Windows::Foundation::IInspectable const&) { ReDraw(); }
        event_token token_DeviceLost;
        void OnVectorChanged(Windows::Foundation::Collections::IObservableVector<Windows::UI::Xaml::Media::GradientStop> const& v, Windows::Foundation::Collections::IVectorChangedEventArgs const& e);
        void OnGradientStopPropertyChanged(Windows::UI::Xaml::DependencyObject const&, Windows::UI::Xaml::DependencyProperty const&) { ReDraw(); }

        static void OnPropertyChanged(Windows::UI::Xaml::DependencyObject const& d, Windows::UI::Xaml::DependencyPropertyChangedEventArgs const&);

        PROP_DECL(SurfaceWidth, float)
        PROP_DECL(SurfaceHeight, float)

        DEPENDENCY_PROPERTY(AlphaMode, Microsoft::Graphics::Canvas::CanvasAlphaMode)
        DEPENDENCY_PROPERTY(ColorInterpolationMode, Windows::UI::Xaml::Media::ColorInterpolationMode)
        DEPENDENCY_PROPERTY(GradientStops, Windows::Foundation::Collections::IObservableVector<Windows::UI::Xaml::Media::GradientStop>)
        DEPENDENCY_PROPERTY(Center, Windows::Foundation::Point)
        DEPENDENCY_PROPERTY(GradientOrigin, Windows::Foundation::Point)
        DEPENDENCY_PROPERTY(RadiusX, double)
        DEPENDENCY_PROPERTY(RadiusY, double)
        DEPENDENCY_PROPERTY(SpreadMethod, Windows::UI::Xaml::Media::GradientSpreadMethod)
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct RadialGradientBrush : RadialGradientBrushT<RadialGradientBrush, implementation::RadialGradientBrush>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

#include "pch.h"

#include "../Shared/LinqHelper.h"
#include "RadialGradientBrush.h"
#include "winrt/Microsoft.Graphics.Canvas.Brushes.h"
#include "winrt/Microsoft.Graphics.Canvas.UI.Composition.h"
#include <linq/to_container.hpp>

using namespace linq;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Foundation::Numerics;
using namespace Windows::Graphics::DirectX;
using namespace Windows::UI::Composition;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Media;
using namespace Microsoft::Graphics::Canvas;
using namespace Microsoft::Graphics::Canvas::Brushes;
using namespace Microsoft::Graphics::Canvas::UI::Composition;

namespace winrt::TsinghuaNetUWP::implementation
{
    constexpr CanvasEdgeBehavior ToEdgeBehavior(GradientSpreadMethod method)
    {
        switch (method)
        {
        case GradientSpreadMethod::Reflect:
            return CanvasEdgeBehavior::Mirror;
        case GradientSpreadMethod::Repeat:
            return CanvasEdgeBehavior::Wrap;
        case GradientSpreadMethod::Pad:
        default:
            return CanvasEdgeBehavior::Clamp;
        }
    }

    constexpr CanvasColorSpace ToCanvasColorSpace(ColorInterpolationMode colorspace)
    {
        switch (colorspace)
        {
        case ColorInterpolationMode::ScRgbLinearInterpolation:
            return CanvasColorSpace::ScRgb;
        case ColorInterpolationMode::SRgbLinearInterpolation:
            return CanvasColorSpace::Srgb;
        }
        return CanvasColorSpace::Custom;
    }

    RadialGradientBrush::RadialGradientBrush()
    {
        SurfaceWidth(512);
        SurfaceHeight(512);
        GradientStops(single_threaded_observable_vector<GradientStop>());
        GradientStops().VectorChanged({ this, &RadialGradientBrush::OnVectorChanged });
    }

    void RadialGradientBrush::OnConnected()
    {
        base_type::OnConnected();
        if (_device && token_DeviceLost)
        {
            _device.DeviceLost(token_DeviceLost);
            token_DeviceLost.value = 0;
        }
        _device = CanvasDevice::GetSharedDevice();
        token_DeviceLost = _device.DeviceLost({ this, &RadialGradientBrush::CanvasDevice_DeviceLost });
        if (_graphics && token_RenderingDeviceReplaced)
        {
            _graphics.RenderingDeviceReplaced(token_RenderingDeviceReplaced);
            token_RenderingDeviceReplaced.value = 0;
        }
        _graphics = CanvasComposition::CreateCompositionGraphicsDevice(Window::Current().Compositor(), _device);
        token_RenderingDeviceReplaced = _graphics.RenderingDeviceReplaced({ this, &RadialGradientBrush::CanvasDevice_RenderingDeviceReplaced });
        ReDraw();
    }

    void RadialGradientBrush::OnDisconnected()
    {
        base_type::OnDisconnected();
        if (_device && token_DeviceLost)
        {
            _device.DeviceLost(token_DeviceLost);
            token_DeviceLost.value = 0;
            _device = nullptr;
        }
        if (_graphics && token_RenderingDeviceReplaced)
        {
            _graphics.RenderingDeviceReplaced(token_RenderingDeviceReplaced);
            token_RenderingDeviceReplaced.value = 0;
            _graphics = nullptr;
        }
        if (CompositionBrush())
        {
            CompositionBrush().Close();
            CompositionBrush(nullptr);
        }
    }

    bool RadialGradientBrush::OnDraw(CanvasDevice const& device, CanvasDrawingSession const& session, float2 size)
    {
        if (GradientStops() && GradientStops().Size() > 0)
        {
            auto stops{
                GradientStops() >>
                select([](GradientStop stop) {
                    return CanvasGradientStop{ (float)stop.Offset(), stop.Color() };
                }) >>
                to_vector<CanvasGradientStop>()
            };

            CanvasRadialGradientBrush gradientBrush{
                device,
                stops,
                ToEdgeBehavior(SpreadMethod()),
                AlphaMode(),
                ToCanvasColorSpace(ColorInterpolationMode()),
                CanvasColorSpace::Srgb,
                CanvasBufferPrecision::Precision8UIntNormalized
            };
            gradientBrush.RadiusX(size.x * (float)RadiusX());
            gradientBrush.RadiusY(size.y * (float)RadiusY());
            gradientBrush.Center(size * Center());
            gradientBrush.OriginOffset(size * (GradientOrigin() - Center()));

            session.FillRectangle(Rect{ 0, 0, size.x, size.y }, gradientBrush);

            gradientBrush.Close();
            return true;
        }
        return false;
    }

    void RadialGradientBrush::ReDraw()
    {
        if (_device && _graphics)
        {
            if (CompositionBrush())
            {
                CompositionBrush().Close();
                CompositionBrush(nullptr);
            }
            if (!CompositionCapabilities::GetForCurrentView().AreEffectsSupported())
                return;
            float2 size{ SurfaceWidth(), SurfaceHeight() };
            auto surface{ _graphics.CreateDrawingSurface(size, DirectXPixelFormat::B8G8R8A8UIntNormalized, DirectXAlphaMode::Premultiplied) };
            {
                auto session{ CanvasComposition::CreateDrawingSession(surface) };
                if (!OnDraw(_device, session, size))
                    return;
            }
            auto _surfaceBrush{ Window::Current().Compositor().CreateSurfaceBrush(surface) };
            _surfaceBrush.Stretch(CompositionStretch::Fill);
            CompositionBrush(_surfaceBrush);
        }
    }

    void RadialGradientBrush::OnVectorChanged(IObservableVector<GradientStop> const& v, IVectorChangedEventArgs const& e)
    {
        switch (e.CollectionChange())
        {
        case CollectionChange::ItemInserted:
        {
            auto item{ v.GetAt(e.Index()) };
            item.RegisterPropertyChangedCallback(item.OffsetProperty(), { this, &RadialGradientBrush::OnGradientStopPropertyChanged });
            item.RegisterPropertyChangedCallback(item.ColorProperty(), { this, &RadialGradientBrush::OnGradientStopPropertyChanged });
        }
        }
    }

    void RadialGradientBrush::OnPropertyChanged(DependencyObject const& d, DependencyPropertyChangedEventArgs const&)
    {
        if (auto brush{ d.try_as<class_type>() })
        {
            auto pb{ get_self<RadialGradientBrush>(brush) };
            pb->ReDraw();
        }
    }

    DEPENDENCY_PROPERTY_INIT(AlphaMode, CanvasAlphaMode, RadialGradientBrush, box_value(CanvasAlphaMode::Straight), &RadialGradientBrush::OnPropertyChanged)
    DEPENDENCY_PROPERTY_INIT(ColorInterpolationMode, Windows::UI::Xaml::Media::ColorInterpolationMode, RadialGradientBrush, box_value(ColorInterpolationMode::SRgbLinearInterpolation), &RadialGradientBrush::OnPropertyChanged)
    DEPENDENCY_PROPERTY_INIT(GradientStops, IObservableVector<GradientStop>, RadialGradientBrush, nullptr, &RadialGradientBrush::OnPropertyChanged)
    DEPENDENCY_PROPERTY_INIT(Center, Point, RadialGradientBrush, box_value<Point>({ 0.5, 0.5 }), &RadialGradientBrush::OnPropertyChanged)
    DEPENDENCY_PROPERTY_INIT(GradientOrigin, Point, RadialGradientBrush, box_value<Point>({ 0.5, 0.5 }), &RadialGradientBrush::OnPropertyChanged)
    DEPENDENCY_PROPERTY_INIT(RadiusX, double, RadialGradientBrush, box_value(0.5), &RadialGradientBrush::OnPropertyChanged)
    DEPENDENCY_PROPERTY_INIT(RadiusY, double, RadialGradientBrush, box_value(0.5), &RadialGradientBrush::OnPropertyChanged)
    DEPENDENCY_PROPERTY_INIT(SpreadMethod, GradientSpreadMethod, RadialGradientBrush, box_value(GradientSpreadMethod::Pad), &RadialGradientBrush::OnPropertyChanged)
} // namespace winrt::TsinghuaNetUWP::implementation

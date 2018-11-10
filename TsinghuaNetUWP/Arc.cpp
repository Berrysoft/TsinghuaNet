#include "pch.h"

#include "Arc.h"
#include <cmath>
#include <corecrt_math_defines.h>
#include <winrt/Windows.UI.Xaml.Media.h>
#include <winrt/Windows.UI.Xaml.Shapes.h>

using namespace std;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Shapes;
using namespace Windows::UI::Xaml::Media;

namespace winrt::TsinghuaNetUWP::implementation
{
    DEPENDENCY_PROPERTY_INIT(Thickness, double, Arc, TsinghuaNetUWP::Arc, box_value(30.0), &Arc::OnSizePropertyChanged)
    DEPENDENCY_PROPERTY_INIT(Fill, Brush, Arc, TsinghuaNetUWP::Arc, Brush(nullptr))
    DEPENDENCY_PROPERTY_INIT(PercentValue, double, Arc, TsinghuaNetUWP::Arc, box_value(0.0), &Arc::OnSizePropertyChanged)

    Arc::Arc()
    {
        Loaded({ this, &Arc::OnLoaded });
        SizeChanged({ this, &Arc::OnSizeChanged });
    }
    void Arc::OnSizePropertyChanged(DependencyObject const& d, DependencyPropertyChangedEventArgs const& /*e*/)
    {
        if (TsinghuaNetUWP::Arc tarc{ d.try_as<TsinghuaNetUWP::Arc>() })
        {
            Arc* parc(get_self<Arc>(tarc));
            parc->SetControlSize();
            parc->Draw();
        }
    }

    void Arc::OnLoaded(IInspectable const& /*sender*/, RoutedEventArgs const& /*e*/)
    {
        SetControlSize();
        Draw();
    }
    void Arc::OnSizeChanged(IInspectable const& /*sender*/, SizeChangedEventArgs const& /*e*/)
    {
        SetControlSize();
        Draw();
    }

    void Arc::SetControlSize()
    {
        Size size = RenderSize();
        double length = min(size.Width, size.Height);
        m_Radius = (length - Thickness()) / 2;
    }

    Point Arc::GetCenterPoint()
    {
        Size size = RenderSize();
        return { (float)size.Width / 2, (float)size.Height / 2 };
    }
    double Arc::GetAngle()
    {
        double angle = PercentValue() / 100 * 360;
        if (angle >= 360)
        {
            angle = 359.999;
        }
        return angle;
    }

    constexpr double RADIANS = M_PI / 180;
    Point ScaleUnitCirclePoint(Point origin, double angle, double radius)
    {
        return { (float)(origin.X - sin(RADIANS * angle) * radius), (float)(origin.Y + cos(RADIANS * angle) * radius) };
    }
    Path GetCircleSegment(Point centerPoint, double radius, double angle)
    {
        Path path;
        PathGeometry pathGeometry;
        Point circleStart = { centerPoint.X, centerPoint.Y + (float)radius };
        ArcSegment arcSegment;
        arcSegment.IsLargeArc(angle > 180.0);
        arcSegment.Point(ScaleUnitCirclePoint(centerPoint, angle, radius));
        arcSegment.Size({ (float)radius, (float)radius });
        arcSegment.SweepDirection(SweepDirection::Clockwise);
        PathFigure pathFigure;
        pathFigure.StartPoint(circleStart);
        pathFigure.IsClosed(false);
        pathFigure.Segments().Append(arcSegment);
        pathGeometry.Figures().Append(pathFigure);
        path.Data(pathGeometry);
        return path;
    }

    void Arc::Draw()
    {
        Children().Clear();
        Path radialStrip = GetCircleSegment(GetCenterPoint(), Radius(), GetAngle());
        radialStrip.Stroke(Fill());
        radialStrip.StrokeThickness(Thickness());
        Children().Append(radialStrip);
    }
} // namespace winrt::TsinghuaNetUWP::implementation

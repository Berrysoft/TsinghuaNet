#include "pch.h"

#include "Arc.h"

#include <cmath>
#include <corecrt_math_defines.h>

using namespace std;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Media;

namespace winrt::TsinghuaNetUWP::implementation
{
    DEPENDENCY_PROPERTY_INIT(Thickness, double, Arc, TsinghuaNetUWP::Arc, box_value(30.0), &Arc::OnSizePropertyChanged)
    DEPENDENCY_PROPERTY_INIT(Fill, Brush, Arc, TsinghuaNetUWP::Arc, Brush(nullptr))
    DEPENDENCY_PROPERTY_INIT(Value, double, Arc, TsinghuaNetUWP::Arc, box_value(0.0), &Arc::OnSizePropertyChanged)

    Arc::Arc()
    {
        InitializeComponent();
    }

    void Arc::OnSizePropertyChanged(DependencyObject const& d, DependencyPropertyChangedEventArgs const&)
    {
        if (TsinghuaNetUWP::Arc tarc{ d.try_as<TsinghuaNetUWP::Arc>() })
        {
            Arc* parc(get_self<Arc>(tarc));
            parc->DrawArc();
        }
    }

    double Arc::GetAngle()
    {
        double angle = Value() * 360;
        if (angle >= 360)
        {
            angle = 359.999;
        }
        return angle;
    }

    constexpr double RADIANS = M_PI / 180;
    Point ScaleUnitCirclePoint(Point origin, double radius, double angle)
    {
        return { (float)(origin.X - sin(RADIANS * angle) * radius), (float)(origin.Y + cos(RADIANS * angle) * radius) };
    }

    void Arc::DrawArc()
    {
        Size size = RenderSize();
        double length = min(size.Width, size.Height);
        m_Radius = (length - Thickness()) / 2;
        Point centerPoint = { (float)size.Width / 2, (float)size.Height / 2 };
        double angle = GetAngle();
        Point circleStart = { centerPoint.X, centerPoint.Y + (float)m_Radius };
        ArcFigureSegment().IsLargeArc(angle > 180.0);
        ArcFigureSegment().Point(ScaleUnitCirclePoint(centerPoint, m_Radius, angle));
        ArcFigureSegment().Size({ (float)m_Radius, (float)m_Radius });
        ArcFigure().StartPoint(circleStart);
    }
} // namespace winrt::TsinghuaNetUWP::implementation

#include "pch.h"

#include "Arc.h"
#include <cmath>
#include <corecrt_math_defines.h>

using namespace std;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Numerics;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Media;

namespace winrt::TsinghuaNetUWP::implementation
{
    DEPENDENCY_PROPERTY_INIT(Thickness, double, Arc, box_value(30.0), &Arc::OnSizePropertyChanged)
    DEPENDENCY_PROPERTY_INIT(Fill, Brush, Arc, Brush(nullptr))
    DEPENDENCY_PROPERTY_INIT(Value, double, Arc, box_value(0.0), &Arc::OnSizePropertyChanged)

    Arc::Arc()
    {
        InitializeComponent();
    }

    void Arc::OnSizePropertyChanged(DependencyObject const& d, DependencyPropertyChangedEventArgs const&)
    {
        if (auto tarc{ d.try_as<class_type>() })
        {
            Arc* parc(get_self<Arc>(tarc));
            parc->DrawArc();
        }
    }

    float GetAngle(double value)
    {
        double angle = value * 2 * M_PI;
        if (angle >= 2 * M_PI) // 要比2π稍微小一点
        {
            angle = 2 * M_PI - 0.000001;
        }
        return (float)angle;
    }

    Point ScaleUnitCirclePoint(Point origin, float radius, float angle)
    {
        float2 dir = { -sin(angle), cos(angle) };
        return dir * radius + origin;
    }

    void Arc::DrawArc()
    {
        Size size = RenderSize();
        float length = min(size.Width, size.Height);
        m_Radius = (length - Thickness()) / 2;
        float fr = (float)m_Radius;
        Point centerPoint = (float2)size / 2;
        float angle = GetAngle(Value());
        Point circleStart = { centerPoint.X, centerPoint.Y + fr };
        ArcFigureSegment().IsLargeArc(angle > M_PI);
        ArcFigureSegment().Point(ScaleUnitCirclePoint(centerPoint, fr, angle));
        ArcFigureSegment().Size({ fr, fr });
        ArcFigure().StartPoint(circleStart);
    }
} // namespace winrt::TsinghuaNetUWP::implementation

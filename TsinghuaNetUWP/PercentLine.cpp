#include "pch.h"

#include "PercentLine.h"

using namespace winrt;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Media;

namespace winrt::TsinghuaNetUWP::implementation
{
    PercentLine::PercentLine()
    {
        InitializeComponent();
    }

    double PercentLine::ValueToWidth(double value)
    {
        return value * ActualWidth();
    }

    CornerRadius PercentLine::ThicknessToRadius(double thickness)
    {
        double r = thickness / 2;
        return { r, r, r, r };
    }

    DEPENDENCY_PROPERTY_INIT(Thickness, double, PercentLine, box_value(30.0))
    DEPENDENCY_PROPERTY_INIT(Fill, Brush, PercentLine, nullptr)
    DEPENDENCY_PROPERTY_INIT(Value, double, PercentLine, box_value(0.0))
} // namespace winrt::TsinghuaNetUWP::implementation

#pragma once
#include "PercentLine.g.h"

#include "../Shared/Utility.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct PercentLine : PercentLineT<PercentLine>
    {
        PercentLine();

        DEPENDENCY_PROPERTY(Thickness, double)
        DEPENDENCY_PROPERTY(Fill, Windows::UI::Xaml::Media::Brush)
        DEPENDENCY_PROPERTY(Value, double)

    public:
        double ValueToWidth(double value);
        Windows::UI::Xaml::CornerRadius ThicknessToRadius(double thickness);
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct PercentLine : PercentLineT<PercentLine, implementation::PercentLine>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation

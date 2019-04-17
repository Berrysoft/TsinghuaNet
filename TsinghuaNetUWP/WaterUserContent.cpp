#include "pch.h"

#include "WaterUserContent.h"

using namespace std;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Media;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    WaterUserContent::WaterUserContent()
    {
        InitializeComponent();
    }

    DEPENDENCY_PROPERTY_INIT(User, TsinghuaNetUWP::FluxUserBox, WaterUserContent, nullptr, OnUserPropertyChangedH<WaterUserContent>)
    DEPENDENCY_PROPERTY_INIT(OnlineTime, TimeSpan, WaterUserContent, box_value(TimeSpan{}))
    DEPENDENCY_PROPERTY_INIT(FreeOffset, double, WaterUserContent, box_value(1.0))
    DEPENDENCY_PROPERTY_INIT(FluxOffset, double, WaterUserContent, box_value(0.0))
} // namespace winrt::TsinghuaNetUWP::implementation

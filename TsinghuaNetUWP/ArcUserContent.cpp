#include "pch.h"

#include "ArcUserContent.h"

using namespace std::chrono_literals;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    ArcUserContent::ArcUserContent()
    {
        InitializeComponent();
    }

    DEPENDENCY_PROPERTY_INIT(User, TsinghuaNetUWP::FluxUserBox, ArcUserContent, nullptr, OnUserPropertyChangedH<ArcUserContent>)
    DEPENDENCY_PROPERTY_INIT(OnlineTime, TimeSpan, ArcUserContent, box_value(TimeSpan{}))
    DEPENDENCY_PROPERTY_INIT(FreeOffset, double, ArcUserContent, box_value(1.0))
    DEPENDENCY_PROPERTY_INIT(FluxOffset, double, ArcUserContent, box_value(0.0))
} // namespace winrt::TsinghuaNetUWP::implementation

#include "pch.h"

#include "GraphUserContent.h"

using namespace std;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    GraphUserContent::GraphUserContent()
    {
        InitializeComponent();
    }

    DEPENDENCY_PROPERTY_INIT(User, TsinghuaNetUWP::FluxUserBox, GraphUserContent, nullptr, OnUserPropertyChangedH<GraphUserContent>)
    DEPENDENCY_PROPERTY_INIT(OnlineTime, TimeSpan, GraphUserContent, box_value(TimeSpan{}))
    DEPENDENCY_PROPERTY_INIT(FreeOffset, double, GraphUserContent, box_value(1.0))
    DEPENDENCY_PROPERTY_INIT(FluxOffset, double, GraphUserContent, box_value(0.0))

    void GraphUserContent::IsProgressActive(bool value)
    {
        Progress().IsIndeterminate(value);
        Progress().Visibility(value ? Visibility::Visible : Visibility::Collapsed);
        MainRect().Visibility(value ? Visibility::Collapsed : Visibility::Visible);
    }
} // namespace winrt::TsinghuaNetUWP::implementation

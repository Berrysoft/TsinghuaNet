#include "pch.h"

#include "LineUserContent.h"

using namespace std;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    LineUserContent::LineUserContent()
    {
        InitializeComponent();
    }

    DEPENDENCY_PROPERTY_INIT(User, FluxUser, LineUserContent, nullptr, [](DependencyObject const& d, DependencyPropertyChangedEventArgs const& e) { OnUserPropertyChangedH<LineUserContent>(d, e); })
    DEPENDENCY_PROPERTY_INIT(OnlineTime, TimeSpan, LineUserContent, box_value(TimeSpan()))
    DEPENDENCY_PROPERTY_INIT(FreeOffset, double, LineUserContent, box_value(1.0))
    DEPENDENCY_PROPERTY_INIT(FluxOffset, double, LineUserContent, box_value(0.0))

    void LineUserContent::IsProgressActive(bool value)
    {
        Progress().IsIndeterminate(value);
        Progress().Visibility(value ? Visibility::Visible : Visibility::Collapsed);
        MainRect().Visibility(value ? Visibility::Collapsed : Visibility::Visible);
    }
} // namespace winrt::TsinghuaNetUWP::implementation

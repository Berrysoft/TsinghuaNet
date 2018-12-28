#include "pch.h"

#include "WaterUserContent.h"

using namespace std::chrono_literals;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    WaterUserContent::WaterUserContent()
    {
        InitializeComponent();
    }

    DEPENDENCY_PROPERTY_INIT(User, FluxUser, WaterUserContent, nullptr, &WaterUserContent::OnUserPropertyChanged)
    DEPENDENCY_PROPERTY_INIT(OnlineTime, TimeSpan, WaterUserContent, box_value(TimeSpan()))
    DEPENDENCY_PROPERTY_INIT(FluxPercent, double, WaterUserContent, box_value(100.0))

    bool WaterUserContent::AddOneSecond()
    {
        if (User().Username().empty())
            return false;
        OnlineTime(OnlineTime() + 1s);
        return true;
    }

    constexpr uint64_t BaseFlux = 25000000000;

    void WaterUserContent::OnUserPropertyChanged(DependencyObject const& d, DependencyPropertyChangedEventArgs const& e)
    {
        if (auto content{ d.try_as<TsinghuaNetUWP::WaterUserContent>() })
        {
            WaterUserContent* pc = get_self<WaterUserContent>(content);
            auto flux = e.NewValue().try_as<FluxUser>();
            pc->OnlineTime(flux.OnlineTime());
            double maxf = (double)UserHelper::GetMaxFlux(flux);
            pc->FluxPercent(flux.Flux() / maxf);
        }
    }
} // namespace winrt::TsinghuaNetUWP::implementation

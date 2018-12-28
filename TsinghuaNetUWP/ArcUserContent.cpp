#include "pch.h"

#include "ArcUserContent.h"

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

    DEPENDENCY_PROPERTY_INIT(User, FluxUser, ArcUserContent, nullptr, PropertyChangedCallback(&ArcUserContent::OnUserPropertyChanged))
    DEPENDENCY_PROPERTY_INIT(OnlineTime, TimeSpan, ArcUserContent, box_value(TimeSpan()))
    DEPENDENCY_PROPERTY_INIT(FreePercent, double, ArcUserContent, box_value(0.0))
    DEPENDENCY_PROPERTY_INIT(FluxPercent, double, ArcUserContent, box_value(100.0))

    /// <summary>
    /// 免费流量25G
    /// </summary>
    constexpr uint64_t BaseFlux = 25000000000;

    void ArcUserContent::OnUserPropertyChanged(DependencyObject const& d, DependencyPropertyChangedEventArgs const& e)
    {
        if (auto content{ d.try_as<TsinghuaNetUWP::ArcUserContent>() })
        {
            ArcUserContent* pc = get_self<ArcUserContent>(content);
            auto flux = e.NewValue().try_as<FluxUser>();
            pc->OnlineTime(flux.OnlineTime());
            double maxf = (double)UserHelper::GetMaxFlux(flux);
            pc->FluxPercent(flux.Flux() / maxf);
            pc->FreePercent(BaseFlux / maxf);
        }
    }
} // namespace winrt::TsinghuaNetUWP::implementation

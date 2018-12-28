#include "pch.h"

#include "LineUserContent.h"

using namespace std::chrono_literals;
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

    DEPENDENCY_PROPERTY_INIT(User, FluxUser, LineUserContent, nullptr, &LineUserContent::OnUserPropertyChanged)
    DEPENDENCY_PROPERTY_INIT(OnlineTime, TimeSpan, LineUserContent, box_value(TimeSpan()))
    DEPENDENCY_PROPERTY_INIT(FluxPercent, double, LineUserContent, box_value(100.0))

    bool LineUserContent::AddOneSecond()
    {
        if (User().Username().empty())
            return false;
        OnlineTime(OnlineTime() + 1s);
        return true;
    }

    /// <summary>
    /// 免费流量25G
    /// </summary>
    constexpr uint64_t BaseFlux = 25000000000;

    void LineUserContent::OnUserPropertyChanged(DependencyObject const& d, DependencyPropertyChangedEventArgs const& e)
    {
        if (auto content{ d.try_as<TsinghuaNetUWP::LineUserContent>() })
        {
            LineUserContent* pc = get_self<LineUserContent>(content);
            auto flux = e.NewValue().try_as<FluxUser>();
            pc->OnlineTime(flux.OnlineTime());
            double maxf = (double)UserHelper::GetMaxFlux(flux);
            pc->FluxPercent(flux.Flux() / maxf);
        }
    }
} // namespace winrt::TsinghuaNetUWP::implementation

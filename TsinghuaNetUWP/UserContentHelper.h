#pragma once
#include "winrt/TsinghuaNetUWP.h"

namespace winrt::TsinghuaNetUWP
{
    template <typename D>
    bool AddOneSecondH(D const& self)
    {
        using namespace std::chrono_literals;
        if (self.User().Username().empty())
            return false;
        self.OnlineTime(self.OnlineTime() + 1s);
        return true;
    }

    template <typename D>
    void OnUserPropertyChangedH(Windows::UI::Xaml::DependencyObject const& d, Windows::UI::Xaml::DependencyPropertyChangedEventArgs const& e)
    {
        constexpr std::uint64_t BaseFlux{ 25000000000 };
        if (auto content{ d.try_as<typename D::class_type>() })
        {
            D* pc{ get_self<D>(content) };
            auto flux{ e.NewValue().try_as<FluxUserBox>() };
            pc->OnlineTime(flux.OnlineTime());
            double maxf{ (double)TsinghuaNetHelper::UserHelper::GetMaxFlux(flux.Flux(), flux.Balance()) };
            pc->FluxAnimation().To(flux.Flux() / maxf);
            pc->FreeAnimation().To(BaseFlux / maxf);
        }
    }

    struct UserContentManager
    {
        UserContentManager(IUserContent const& content) : content(content)
        {
            if (content)
                content.IsProgressActive(true);
        }
        ~UserContentManager()
        {
            if (content)
                content.IsProgressActive(false);
        }

    private:
        IUserContent content;
    };
} // namespace winrt::TsinghuaNetUWP

#include "pch.h"

#include "MainViewModel.h"

using namespace std;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    MainViewModel::MainViewModel()
    {
        m_NetUsers = single_threaded_observable_vector<IInspectable>();
    }

    void MainViewModel::OnAutoLoginPropertyChanged(DependencyObject const& d, DependencyPropertyChangedEventArgs const& e)
    {
        if (auto model{ d.try_as<class_type>() })
        {
            MainViewModel* pm(get_self<MainViewModel>(model));
            pm->m_AutoLoginChangedEvent(model, unbox_value<bool>(e.NewValue()));
        }
    }

    void MainViewModel::OnBackgroundAutoLoginPropertyChanged(DependencyObject const& d, DependencyPropertyChangedEventArgs const& e)
    {
        if (auto model{ d.try_as<class_type>() })
        {
            MainViewModel* pm(get_self<MainViewModel>(model));
            pm->m_BackgroundAutoLoginChangedEvent(model, unbox_value<bool>(e.NewValue()));
        }
    }

    void MainViewModel::OnBackgroundLiveTilePropertyChanged(DependencyObject const& d, DependencyPropertyChangedEventArgs const& e)
    {
        if (auto model{ d.try_as<class_type>() })
        {
            MainViewModel* pm(get_self<MainViewModel>(model));
            pm->m_BackgroundLiveTileChangedEvent(model, unbox_value<bool>(e.NewValue()));
        }
    }

    DEPENDENCY_PROPERTY_INIT(OnlineUser, hstring, MainViewModel, box_value(hstring()))
    DEPENDENCY_PROPERTY_INIT(Flux, uint64_t, MainViewModel, box_value<uint64_t>(0))
    DEPENDENCY_PROPERTY_INIT(OnlineTime, TimeSpan, MainViewModel, box_value(TimeSpan()))
    DEPENDENCY_PROPERTY_INIT(Balance, double, MainViewModel, box_value(0.0))

    DEPENDENCY_PROPERTY_INIT(FluxPercent, double, MainViewModel, box_value(0.0))
    DEPENDENCY_PROPERTY_INIT(FreePercent, double, MainViewModel, box_value(100.0))

    DEPENDENCY_PROPERTY_INIT(Response, LogResponse, MainViewModel, box_value(LogResponse{}))

    DEPENDENCY_PROPERTY_INIT(Username, hstring, MainViewModel, box_value(hstring()))
    DEPENDENCY_PROPERTY_INIT(Password, hstring, MainViewModel, box_value(hstring()))

    DEPENDENCY_PROPERTY_INIT(State, NetState, MainViewModel, box_value(NetState::Unknown))

    DEPENDENCY_PROPERTY_INIT(AutoLogin, bool, MainViewModel, box_value(true), &MainViewModel::OnAutoLoginPropertyChanged)
    DEPENDENCY_PROPERTY_INIT(BackgroundAutoLogin, bool, MainViewModel, box_value(true), &MainViewModel::OnBackgroundAutoLoginPropertyChanged)
    DEPENDENCY_PROPERTY_INIT(BackgroundLiveTile, bool, MainViewModel, box_value(true), &MainViewModel::OnBackgroundLiveTilePropertyChanged)

    DEPENDENCY_PROPERTY_INIT(NetStatus, InternetStatus, MainViewModel, box_value(InternetStatus::Unknown))
    DEPENDENCY_PROPERTY_INIT(Ssid, hstring, MainViewModel, box_value(hstring()))
    DEPENDENCY_PROPERTY_INIT(SuggestState, NetState, MainViewModel, box_value(NetState::Unknown))
} // namespace winrt::TsinghuaNetUWP::implementation

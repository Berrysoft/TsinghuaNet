#pragma once
#include "App.xaml.g.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct App : AppT<App>
    {
        App();

        void OnLaunched(Windows::ApplicationModel::Activation::LaunchActivatedEventArgs const& e);
        void OnActivated(Windows::ApplicationModel::Activation::IActivatedEventArgs const& e);
        void OnSuspending(IInspectable const& sender, Windows::ApplicationModel::SuspendingEventArgs const& e);
        void OnNavigationFailed(IInspectable const& sender, Windows::UI::Xaml::Navigation::NavigationFailedEventArgs const& e);
    };
} // namespace winrt::TsinghuaNetUWP::implementation

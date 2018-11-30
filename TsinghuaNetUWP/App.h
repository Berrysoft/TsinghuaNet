#pragma once
#include "App.xaml.g.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct App : AppT<App>
    {
        App();

        Windows::Foundation::IAsyncAction OnLaunched(Windows::ApplicationModel::Activation::LaunchActivatedEventArgs const&);
        void OnSuspending(IInspectable const&, Windows::ApplicationModel::SuspendingEventArgs const&);
        void OnNavigationFailed(IInspectable const&, Windows::UI::Xaml::Navigation::NavigationFailedEventArgs const&);
    };
} // namespace winrt::TsinghuaNetUWP::implementation

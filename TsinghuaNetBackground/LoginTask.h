#pragma once
#include "LoginTask.g.h"

#include "winrt/TsinghuaNetHelper.h"

namespace winrt::TsinghuaNetBackground::implementation
{
    struct LoginTask : LoginTaskT<LoginTask>
    {
        LoginTask() = default;

        Windows::Foundation::IAsyncAction Run(Windows::ApplicationModel::Background::IBackgroundTaskInstance const& taskInstance);

    private:
        TsinghuaNetHelper::SettingsHelper settings;
        TsinghuaNetHelper::NotificationHelper notification;
    };
} // namespace winrt::TsinghuaNetBackground::implementation

namespace winrt::TsinghuaNetBackground::factory_implementation
{
    struct LoginTask : LoginTaskT<LoginTask, implementation::LoginTask>
    {
    };
} // namespace winrt::TsinghuaNetBackground::factory_implementation

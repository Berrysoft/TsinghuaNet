#pragma once
#include "LoginTask.g.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct LoginTask : LoginTaskT<LoginTask>
    {
        LoginTask() = default;

        Windows::Foundation::IAsyncAction Run(Windows::ApplicationModel::Background::IBackgroundTaskInstance const taskInstance);

    private:
        SettingsHelper settings;
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct LoginTask : LoginTaskT<LoginTask, implementation::LoginTask>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

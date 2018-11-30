#pragma once
#include "LiveTileTask.g.h"

#include "winrt/TsinghuaNetHelper.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct LiveTileTask : LiveTileTaskT<LiveTileTask>
    {
        LiveTileTask() = default;

        Windows::Foundation::IAsyncAction Run(Windows::ApplicationModel::Background::IBackgroundTaskInstance const& taskInstance);

    private:
        NotificationHelper notification;
        SettingsHelper settings;
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct LiveTileTask : LiveTileTaskT<LiveTileTask, implementation::LiveTileTask>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

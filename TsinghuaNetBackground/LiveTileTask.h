#pragma once
#include "LiveTileTask.g.h"

#include "winrt/TsinghuaNetHelper.h"

namespace winrt::TsinghuaNetBackground::implementation
{
    struct LiveTileTask : LiveTileTaskT<LiveTileTask>
    {
        LiveTileTask() = default;

        Windows::Foundation::IAsyncAction Run(Windows::ApplicationModel::Background::IBackgroundTaskInstance const& taskInstance);

    private:
        TsinghuaNetHelper::NotificationHelper notification;
        TsinghuaNetHelper::SettingsHelper settings;
    };
} // namespace winrt::TsinghuaNetBackground::implementation

namespace winrt::TsinghuaNetBackground::factory_implementation
{
    struct LiveTileTask : LiveTileTaskT<LiveTileTask, implementation::LiveTileTask>
    {
    };
} // namespace winrt::TsinghuaNetBackground::factory_implementation

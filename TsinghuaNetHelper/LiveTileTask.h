#pragma once
#include "LiveTileTask.g.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct LiveTileTask : LiveTileTaskT<LiveTileTask>
    {
        LiveTileTask() = default;

        fire_and_forget Run(Windows::ApplicationModel::Background::IBackgroundTaskInstance const taskInstance);

    private:
        SettingsHelper settings;
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct LiveTileTask : LiveTileTaskT<LiveTileTask, implementation::LiveTileTask>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation

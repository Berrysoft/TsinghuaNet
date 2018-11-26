#pragma once
#include "LiveTileTask.g.h"

#include "winrt/TsinghuaNetHelper.h"

namespace winrt::TsinghuaNetBackground::implementation
{
    struct LiveTileTask : LiveTileTaskT<LiveTileTask>
    {
        LiveTileTask() = default;

        void Run(Windows::ApplicationModel::Background::IBackgroundTaskInstance const& taskInstance);

    private:
        Windows::Foundation::IAsyncAction RunAsync();
    };
} // namespace winrt::TsinghuaNetBackground::implementation

namespace winrt::TsinghuaNetBackground::factory_implementation
{
    struct LiveTileTask : LiveTileTaskT<LiveTileTask, implementation::LiveTileTask>
    {
    };
} // namespace winrt::TsinghuaNetBackground::factory_implementation

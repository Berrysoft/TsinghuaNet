#include "pch.h"

#include "ConnectHelper.h"
#include "LiveTileTask.h"

using namespace winrt;
using namespace Windows::ApplicationModel::Background;

namespace winrt::TsinghuaNetHelper::implementation
{
    fire_and_forget LiveTileTask::Run(IBackgroundTaskInstance const taskInstance)
    {
        auto deferral = taskInstance.GetDeferral();
        try
        {
            NetState state = ConnectHelper::GetSuggestNetState(settings);
            IConnect helper = ConnectHelper::GetHelper(state);
            if (helper)
            {
                FluxUser user = co_await helper.FluxAsync();
                NotificationHelper::UpdateTile(user);
            }
        }
        catch (hresult_error const&)
        {
        }
        deferral.Complete();
    }
} // namespace winrt::TsinghuaNetHelper::implementation

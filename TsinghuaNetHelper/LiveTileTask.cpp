#include "pch.h"

#include "LiveTileTask.h"

#include "ConnectHelper.h"

using namespace winrt;
using namespace Windows::ApplicationModel::Background;
using namespace Windows::Foundation;

namespace winrt::TsinghuaNetHelper::implementation
{
    IAsyncAction LiveTileTask::Run(IBackgroundTaskInstance const& taskInstance)
    {
        auto deferral = taskInstance.GetDeferral();
        try
        {
            NetState state = GetSuggestNetState(settings);
            IConnect helper = GetHelper(state);
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

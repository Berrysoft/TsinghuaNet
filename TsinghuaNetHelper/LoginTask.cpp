#include "pch.h"

#include "LoginTask.h"

#include "ConnectHelper.h"

using namespace winrt;
using namespace Windows::ApplicationModel::Background;
using namespace Windows::Foundation;

namespace winrt::TsinghuaNetHelper::implementation
{
    IAsyncAction LoginTask::Run(IBackgroundTaskInstance const& taskInstance)
    {
        auto deferral = taskInstance.GetDeferral();
        if (settings.AutoLogin())
        {
            try
            {
                NetState state = GetSuggestNetState(settings);
                hstring un = settings.StoredUsername();
                hstring pw = CredentialHelper::GetCredential(un);
                IConnect helper = GetHelper(state, un, pw);
                if (helper)
                {
                    co_await helper.LoginAsync();
                    FluxUser user = co_await helper.FluxAsync();
                    notification.UpdateTile(user);
                    notification.SendToast(user);
                }
            }
            catch (hresult_error const&)
            {
            }
        }
        deferral.Complete();
    }
} // namespace winrt::TsinghuaNetHelper::implementation

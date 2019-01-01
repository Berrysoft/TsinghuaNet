#include "pch.h"

#include "LoginTask.h"

using namespace winrt;
using namespace Windows::ApplicationModel::Background;

namespace winrt::TsinghuaNetHelper::implementation
{
    fire_and_forget LoginTask::Run(IBackgroundTaskInstance const taskInstance)
    {
        auto deferral{ taskInstance.GetDeferral() };
        if (settings.AutoLogin())
        {
            try
            {
                hstring ssid;
                auto status{ SettingsHelper::InternetStatus(ssid) };
                NetState state{ settings.SuggestNetState(status, ssid) };
                hstring un{ settings.StoredUsername() };
                hstring pw{ CredentialHelper::GetCredential(un) };
                IConnect helper{ ConnectHelper::GetHelper(state, un, pw) };
                if (helper)
                {
                    co_await helper.LoginAsync();
                    FluxUser user{ co_await helper.FluxAsync() };
                    NotificationHelper::UpdateTile(user);
                    NotificationHelper::SendToast(user);
                }
            }
            catch (hresult_error const&)
            {
            }
        }
        deferral.Complete();
    }
} // namespace winrt::TsinghuaNetHelper::implementation

#include "pch.h"

#include "LiveTileTask.h"

using namespace winrt;
using namespace Windows::ApplicationModel::Background;
using namespace Windows::Foundation;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetBackground::implementation
{
    NetState GetSuggestNetState(SettingsHelper const& lan)
    {
        hstring ssid;
        auto status = lan.GetCurrentInternetStatus(ssid);
        switch (status)
        {
        case InternetStatus::Lan:
            return lan.LanState();
        case InternetStatus::Wwan:
            return lan.WwanState();
        case InternetStatus::Wlan:
            return lan.WlanState(ssid);
        default:
            return NetState::Unknown;
        }
    }

    IConnect GetHelper(NetState state)
    {
        switch (state)
        {
        case NetState::Auth4:
            return Auth4Helper();
        case NetState::Auth6:
            return Auth6Helper();
        case NetState::Net:
            return NetHelper();
        default:
            return nullptr;
        }
    }

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
                notification.UpdateTile(user);
            }
        }
        catch (hresult_error const&)
        {
        }
        deferral.Complete();
    }
} // namespace winrt::TsinghuaNetBackground::implementation

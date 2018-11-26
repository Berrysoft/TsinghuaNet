#include "pch.h"

#include "NotificationHelper.h"
#include "UserHelper.h"
#include <winrt/Windows.Data.Xml.Dom.h>
#include <winrt/Windows.UI.Notifications.h>

using namespace std;
using sf::sprint;
using namespace winrt;
using namespace Windows::Data::Xml::Dom;
using namespace Windows::Foundation;
using namespace Windows::Storage;
using namespace Windows::UI::Notifications;

namespace winrt::TsinghuaNetHelper::implementation
{
    void NotificationHelper::UpdateTile(TsinghuaNetHelper::FluxUser const& user)
    {
        XmlDocument dom;
        dom.LoadXml(sprint(tile_t,
                           wstring_view(user.Username()),
                           wstring_view(UserHelper::GetFluxString(user.Flux())),
                           wstring_view(UserHelper::GetTimeSpanString(user.OnlineTime())),
                           wstring_view(UserHelper::GetCurrencyString(user.Balance())),
                           wstring_view(UserHelper::GetFluxString(UserHelper::GetMaxFlux(user) - user.Flux()))));
        TileNotification notification(dom);
        auto time = clock::now();
        time += 10min;
        notification.ExpirationTime(time);
        TileUpdateManager::CreateTileUpdaterForApplication().Update(notification);
    }

    void NotificationHelper::SendToast(TsinghuaNetHelper::FluxUser const& user)
    {
        throw hresult_not_implemented();
    }

    IAsyncOperation<TsinghuaNetHelper::NotificationHelper> NotificationHelper::LoadAsync()
    {
        auto result = make<NotificationHelper>();
        NotificationHelper* presult = get_self<NotificationHelper>(result);
        presult->tile_t = co_await FileIO::ReadTextAsync(co_await StorageFile::GetFileFromApplicationUriAsync(Uri(L"ms-appx:///TsinghuaNetHelper/tile.xml")));
        presult->toast_t = co_await FileIO::ReadTextAsync(co_await StorageFile::GetFileFromApplicationUriAsync(Uri(L"ms-appx:///TsinghuaNetHelper/toast.xml")));
        return result;
    }
} // namespace winrt::TsinghuaNetHelper::implementation

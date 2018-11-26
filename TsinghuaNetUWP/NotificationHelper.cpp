#include "pch.h"

#include "NotificationHelper.h"

#include <cmath>
#include <sf/sformat.hpp>
#include <winrt/Windows.Data.Xml.Dom.h>
#include <winrt/Windows.UI.Notifications.h>

using namespace std;
using sf::sprint;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Notifications;
using namespace Windows::Data::Xml::Dom;
using namespace Windows::Storage;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP
{
    hstring GetFluxString(uint64_t f)
    {
        double flux = (double)f;
        wstring result;
        if (flux < 1000)
        {
            result = sprint(L"{} B", flux);
        }
        else if ((flux /= 1000) < 1000)
        {
            result = sprint(L"{:f2} kB", flux);
        }
        else if ((flux /= 1000) < 1000)
        {
            result = sprint(L"{:f2} MB", flux);
        }
        else
        {
            flux /= 1000;
            result = sprint(L"{:f2} GB", flux);
        }
        return hstring(result);
    }

    hstring GetTimeSpanString(TimeSpan const& time)
    {
        int64_t tsec = time.count() / 10000000;
        bool minus = tsec < 0;
        if (minus)
            tsec = -tsec;
        int64_t h = tsec / 3600;
        tsec -= h * 3600;
        int64_t min = tsec / 60;
        tsec -= min * 60;
        return hstring(sprint(L"{}{:d2}:{:d2}:{:d2}", minus ? L"-" : L"", h, min, tsec));
    }

    hstring GetCurrencyString(double currency)
    {
        return hstring(sprint(L"гд{:f2}", currency));
    }

    uint64_t GetMaxFlux(FluxUser const& user)
    {
        return max(user.Flux(), BaseFlux) + (uint64_t)(user.Balance() * 2 * 1000 * 1000 * 1000);
    }

    wstring xmlt;
    IAsyncAction LoadTileTemplate()
    {
        xmlt = co_await FileIO::ReadTextAsync(co_await StorageFile::GetFileFromApplicationUriAsync(Uri(L"ms-appx:///tile.xml")));
    }

    void UpdateTile(FluxUser const& user)
    {
        XmlDocument dom;
        dom.LoadXml(sprint(xmlt,
                           wstring_view(user.Username()),
                           wstring_view(GetFluxString(user.Flux())),
                           wstring_view(GetTimeSpanString(user.OnlineTime())),
                           wstring_view(GetCurrencyString(user.Balance())),
                           wstring_view(GetFluxString(GetMaxFlux(user) - user.Flux()))));
        TileNotification notification(dom);
        auto time = clock::now();
        time += 10min;
        notification.ExpirationTime(time);
        TileUpdateManager::CreateTileUpdaterForApplication().Update(notification);
    }
} // namespace winrt::TsinghuaNetUWP

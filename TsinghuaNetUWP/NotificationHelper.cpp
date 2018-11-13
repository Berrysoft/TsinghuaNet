#include "pch.h"

#include "NotificationHelper.h"

#include "winrt/Microsoft.Toolkit.Uwp.Notifications.h"
#include <cmath>
#include <sf/sformat.hpp>
#include <winrt/Windows.UI.Notifications.h>

using namespace std;
using namespace sf;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Notifications;
using namespace Microsoft::Toolkit::Uwp::Notifications;

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
        return hstring(sprint(L"￥{:f2}", currency));
    }

    uint64_t GetMaxFlux(FluxUser const& user)
    {
        return max(user.flux, BaseFlux) + (uint64_t)(user.balance * 2 * 1000 * 1000 * 1000);
    }

    TileBinding GetTile(FluxUser const& user, bool large)
    {
        TileBinding result;
        TileBindingContentAdaptive content;
        {
            AdaptiveText text;
            text.Text(user.username);
            text.HintStyle(large ? AdaptiveTextStyle::Subtitle : AdaptiveTextStyle::Base);
            content.Children().Append(text);
        }
        {
            AdaptiveText text;
            hstring fs = GetFluxString(user.flux);
            text.Text(large ? L"流量：" + fs : fs);
            text.HintStyle(AdaptiveTextStyle::BodySubtle);
            content.Children().Append(text);
        }
        {
            AdaptiveText text;
            hstring cs = GetCurrencyString(user.balance);
            text.Text(large ? L"余额：" + cs : cs);
            text.HintStyle(AdaptiveTextStyle::BodySubtle);
            content.Children().Append(text);
        }
        result.Content(content);
        return result;
    }

    TileBinding GetLargeTile(FluxUser const& user)
    {
        TileBinding result;
        TileBindingContentAdaptive content;
        {
            AdaptiveText text;
            text.Text(user.username);
            text.HintStyle(AdaptiveTextStyle::Title);
            content.Children().Append(text);
        }
        {
            AdaptiveText text;
            hstring fs = GetFluxString(user.flux);
            text.Text(L"流量：" + fs);
            text.HintStyle(AdaptiveTextStyle::SubtitleSubtle);
            content.Children().Append(text);
        }
        {
            AdaptiveText text;
            hstring fs = GetTimeSpanString(user.online_time);
            text.Text(L"时长：" + fs);
            text.HintStyle(AdaptiveTextStyle::SubtitleSubtle);
            content.Children().Append(text);
        }
        {
            AdaptiveText text;
            hstring cs = GetCurrencyString(user.balance);
            text.Text(L"余额：" + cs);
            text.HintStyle(AdaptiveTextStyle::SubtitleSubtle);
            content.Children().Append(text);
        }
        {
            AdaptiveText text;
            uint64_t maxf = GetMaxFlux(user);
            text.Text(L"剩余：" + GetFluxString(maxf - user.flux));
            text.HintStyle(AdaptiveTextStyle::SubtitleSubtle);
            content.Children().Append(text);
        }
        result.Content(content);
        return result;
    }

    void UpdateTile(FluxUser const& user)
    {
        TileContent content;
        TileVisual visual;
        visual.Branding(TileBranding::NameAndLogo);
        visual.TileMedium(GetTile(user, false));
        visual.TileWide(GetTile(user, true));
        visual.TileLarge(GetLargeTile(user));
        content.Visual(visual);
        TileNotification notification(content.GetXml());
        auto time = clock::now();
        time += 10min;
        notification.ExpirationTime(time);
        TileUpdateManager::CreateTileUpdaterForApplication().Update(notification);
    }
} // namespace winrt::TsinghuaNetUWP

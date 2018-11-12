#include "pch.h"

#include "NotificationHelper.h"

#include "winrt/Microsoft.Toolkit.Uwp.Notifications.h"
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

    hstring GetCurrencyString(double currency)
    {
        return hstring(sprint(L"￥{:f2}", currency));
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

    void UpdateTile(FluxUser const& user)
    {
        TileContent content;
        TileVisual visual;
        visual.Branding(TileBranding::NameAndLogo);
        visual.TileMedium(GetTile(user, false));
        visual.TileWide(GetTile(user, true));
        content.Visual(visual);
        TileNotification notification(content.GetXml());
        auto time = clock::now();
        time += 10min;
        notification.ExpirationTime(time);
        TileUpdateManager::CreateTileUpdaterForApplication().Update(notification);
    }

    ToastBindingGeneric GetToast(FluxUser const& user, bool logout)
    {
        ToastBindingGeneric result;
        {
            AdaptiveText text;
            text.Text(logout ? L"注销成功" : L"登录成功");
            result.Children().Append(text);
        }
        {
            AdaptiveText text;
            text.Text(L"用户名：" + user.username);
            result.Children().Append(text);
        }
        {
            AdaptiveText text;
            wstring fs(GetFluxString(user.flux));
            wstring cs(GetCurrencyString(user.balance));
            text.Text(sprint(L"流量：{}\t余额：{}", fs, cs));
            result.Children().Append(text);
        }
        return result;
    }

    void SendToast(FluxUser const& user, bool logout)
    {
        ToastContent content;
        content.Launch(L"TsinghuaNetUWP");
        ToastVisual visual;
        visual.BindingGeneric(GetToast(user, logout));
        content.Visual(visual);
        ToastNotification notification(content.GetXml());
        ToastNotificationManager::CreateToastNotifier().Show(notification);
    }
} // namespace winrt::TsinghuaNetUWP

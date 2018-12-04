﻿#include "pch.h"

#include "NotificationHelper.h"
#include "UserHelper.h"
#include <winrt/Windows.Data.Xml.Dom.h>
#include <winrt/Windows.UI.Notifications.h>

using namespace std;
using sf::sprint;
using namespace winrt;
using namespace Windows::Data::Xml::Dom;
using namespace Windows::Foundation;
using namespace Windows::UI::Notifications;

namespace winrt::TsinghuaNetHelper::implementation
{
    constexpr wchar_t tile_t[] = LR"(<?xml version="1.0" encoding="utf-8"?>
<tile>
  <visual branding="nameAndLogo">
    <binding template="TileMedium">
      <text hint-style="base">{0}</text>
      <text hint-style="bodySubtle">{1}</text>
      <text hint-style="bodySubtle">{3}</text>
    </binding>
    <binding template="TileWide">
      <text hint-style="subtitle">{0}</text>
      <text hint-style="bodySubtle">流量：{1}</text>
      <text hint-style="bodySubtle">余额：{3}</text>
    </binding>
    <binding template="TileLarge">
      <text hint-style="title">{0}</text>
      <text hint-style="subtitleSubtle">流量：{1}</text>
      <text hint-style="subtitleSubtle">时长：{2}</text>
      <text hint-style="subtitleSubtle">余额：{3}</text>
      <text hint-style="subtitleSubtle">剩余：{4}</text>
    </binding>
  </visual>
</tile>
)";

    constexpr wchar_t toast_t[] = LR"(<?xml version="1.0" encoding="utf-8"?>
<toast>
  <visual>
    <binding template="ToastGeneric">
      <text hint-maxLines="1">登录成功：{0}</text>
      <text>流量：{1}</text>
      <text>余额：{2}</text>
    </binding>
  </visual>
</toast>
)";

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
        notification.ExpirationTime(clock::now() + 15min);
        TileUpdateManager::CreateTileUpdaterForApplication().Update(notification);
    }

    void NotificationHelper::SendToast(TsinghuaNetHelper::FluxUser const& user)
    {
        XmlDocument dom;
        dom.LoadXml(sprint(toast_t,
                           wstring_view(user.Username()),
                           wstring_view(UserHelper::GetFluxString(user.Flux())),
                           wstring_view(UserHelper::GetCurrencyString(user.Balance()))));
        ToastNotification notification(dom);
        notification.ExpirationTime(clock::now() + 1min);
        ToastNotificationManager::CreateToastNotifier().Show(notification);
    }
} // namespace winrt::TsinghuaNetHelper::implementation
Imports TsinghuaNet.Helper
Imports Windows.Data.Xml.Dom
Imports Windows.UI.Notifications

Public Module NotificationHelper
    Private tileText As String
    Private toastText As String
    Private toastWarningText As String

    Sub New()
        tileText = <![CDATA[<?xml version="1.0" encoding="utf-8"?>
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
            <text hint-style="subtitleSubtle">剩余：{4}</text>
            <text hint-style="subtitleSubtle">时长：{2}</text>
            <text hint-style="subtitleSubtle">余额：{3}</text>
        </binding>
    </visual>
</tile>]]>.Value
        toastText = <![CDATA[<?xml version="1.0" encoding="utf-8"?>
<toast>
    <visual>
        <binding template="ToastGeneric">
            <text hint-maxLines="1">登录成功：{0}</text>
            <text>流量：{1}</text>
            <text>余额：{2}</text>
        </binding>
    </visual>
</toast>]]>.Value
        toastWarningText = <![CDATA[<?xml version="1.0" encoding="utf-8"?>
<toast>
    <visual>
        <binding template="ToastGeneric">
            <text hint-maxLines="1">流量预警：{0}</text>
            <text>流量：{1}</text>
            <text>剩余：{2}</text>
        </binding>
    </visual>
</toast>]]>.Value
    End Sub

    Public Sub UpdateTile(user As FluxUser)
        Dim dom As New XmlDocument
        dom.LoadXml(String.Format(
                    tileText,
                    user.Username,
                    StringHelper.GetFluxString(user.Flux),
                    user.OnlineTime.ToString(),
                    StringHelper.GetCurrencyString(user.Balance),
                    StringHelper.GetFluxString(FluxHelper.GetMaxFlux(user.Flux, user.Balance) - user.Flux)))
        Dim notification As New TileNotification(dom)
        notification.ExpirationTime = DateTimeOffset.Now + TimeSpan.FromMinutes(15)
        TileUpdateManager.CreateTileUpdaterForApplication().Update(notification)
    End Sub

    Public Sub SendToast(user As FluxUser)
        Dim dom As New XmlDocument
        dom.LoadXml(String.Format(
                    toastText,
                    user.Username,
                    StringHelper.GetFluxString(user.Flux),
                    StringHelper.GetCurrencyString(user.Balance)))
        Dim notification As New ToastNotification(dom)
        notification.ExpirationTime = DateTimeOffset.Now + TimeSpan.FromMinutes(1)
        ToastNotificationManager.CreateToastNotifier().Show(notification)
    End Sub

    Public Sub SendWarningToast(user As FluxUser, limit As Long)
        If user.Flux > limit Then
            Dim dom As New XmlDocument
            dom.LoadXml(String.Format(
                        toastWarningText,
                        user.Username,
                        StringHelper.GetFluxString(user.Flux),
                        StringHelper.GetFluxString(FluxHelper.GetMaxFlux(user.Flux, user.Balance) - user.Flux)))
            Dim notification As New ToastNotification(dom)
            notification.ExpirationTime = DateTimeOffset.Now + TimeSpan.FromMinutes(1)
            ToastNotificationManager.CreateToastNotifier().Show(notification)
        End If
    End Sub
End Module

using System;
using System.Globalization;
using TsinghuaNet.Models;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace TsinghuaNet.XF.UWP.Helpers
{
    public static class NotificationHelper
    {
        private static readonly string tileText;
        private static readonly string toastText;
        private static readonly string toastWarningText;

        static NotificationHelper()
        {
            tileText = @"<?xml version=""1.0"" encoding=""utf-8""?>
<tile>
    <visual branding=""nameAndLogo"">
        <binding template=""TileMedium"">
            <text hint-style=""base"">{0}</text>
            <text hint-style=""bodySubtle"">{1}</text>
            <text hint-style=""bodySubtle"">{3:C2}</text>
        </binding>
        <binding template=""TileWide"">
            <text hint-style=""subtitle"">{0}</text>
            <text hint-style=""bodySubtle"">流量：{1}</text>
            <text hint-style=""bodySubtle"">余额：{3:C2}</text>
        </binding>
        <binding template=""TileLarge"">
            <text hint-style=""title"">{0}</text>
            <text hint-style=""subtitleSubtle"">流量：{1}</text>
            <text hint-style=""subtitleSubtle"">剩余：{4}</text>
            <text hint-style=""subtitleSubtle"">时长：{2}</text>
            <text hint-style=""subtitleSubtle"">余额：{3:C2}</text>
        </binding>
    </visual>
</tile>";
            toastText = @"<?xml version=""1.0"" encoding=""utf-8""?>
<toast>
    <visual>
        <binding template=""ToastGeneric"">
            <text hint-maxLines=""1"">登录成功：{0}</text>
            <text>流量：{1}</text>
            <text>余额：{2:C2}</text>
        </binding>
    </visual>
</toast>";
            toastWarningText = @"<?xml version=""1.0"" encoding=""utf-8""?>
<toast>
    <visual>
        <binding template=""ToastGeneric"">
            <text hint-maxLines=""1"">流量预警：{0}</text>
            <text>流量：{1}</text>
            <text>剩余：{2}</text>
        </binding>
    </visual>
</toast>";
        }

        private static readonly CultureInfo zhCulture = CultureInfo.GetCultureInfo("zh-CN");

        public static void UpdateTile(FluxUser user)
        {
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(string.Format(tileText, user.Username, user.Flux, user.OnlineTime, user.Balance, FluxHelper.GetMaxFlux(user.Flux, user.Balance) - user.Flux));
            TileNotification notification = new TileNotification(dom);
            notification.ExpirationTime = DateTimeOffset.Now + TimeSpan.FromMinutes(15);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }

        public static void SendToast(FluxUser user)
        {
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(string.Format(zhCulture, toastText, user.Username, user.Flux, user.Balance));
            ToastNotification notification = new ToastNotification(dom);
            notification.ExpirationTime = DateTimeOffset.Now + TimeSpan.FromMinutes(1);
            ToastNotificationManager.CreateToastNotifier().Show(notification);
        }

        public static void SendWarningToast(FluxUser user, ByteSize limit)
        {
            if (user.Flux > limit)
            {
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(string.Format(zhCulture, toastWarningText, user.Username, user.Flux, FluxHelper.GetMaxFlux(user.Flux, user.Balance) - user.Flux));
                ToastNotification notification = new ToastNotification(dom);
                notification.ExpirationTime = DateTimeOffset.Now + TimeSpan.FromMinutes(1);
                ToastNotificationManager.CreateToastNotifier().Show(notification);
            }
        }
    }
}

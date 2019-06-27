using System.Globalization;
using TsinghuaNet.Models;

namespace TsinghuaNet.Helpers
{
    public static class StringHelper
    {
        private static CultureInfo zhCulture = new CultureInfo("zh-CN");
        public static string GetCurrencyString(decimal currency)
            => currency.ToString("C2", zhCulture);

        public static string GetNetStateString(NetState state)
        {
            switch (state)
            {
                case NetState.Net:
                    return "Net";
                case NetState.Auth4:
                    return "Auth4";
                case NetState.Auth6:
                    return "Auth6";
                default:
                    return "不登录";
            }
        }

        public static string GetNetStatusString(NetStatus status)
        {
            switch (status)
            {
                case NetStatus.Wwan:
                    return "移动流量";
                case NetStatus.Wlan:
                    return "无线网络";
                case NetStatus.Lan:
                    return "有线网络";
                default:
                    return "未连接";
            }
        }

        public static string GetNetStatusString(NetStatus status, string ssid)
        {
            if (string.IsNullOrEmpty(ssid))
                return GetNetStatusString(status);
            else
                return $"{GetNetStatusString(status)} - {ssid}";
        }
    }
}

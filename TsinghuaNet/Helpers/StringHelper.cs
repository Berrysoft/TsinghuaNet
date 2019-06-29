using System.Globalization;

namespace TsinghuaNet.Helpers
{
    public static class StringHelper
    {
        private static readonly CultureInfo zhCulture = new CultureInfo("zh-CN");
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
    }
}

using System.Globalization;

namespace TsinghuaNet
{
    public static class StringHelper
    {
        private static readonly CultureInfo zhCulture = new CultureInfo("zh-CN");
        public static string GetCurrencyString(decimal currency)
            => currency.ToString("C2", zhCulture);

        public static string GetNetStateString(NetState state)
        {
            return state switch
            {
                NetState.Net => "Net",
                NetState.Auth4 => "Auth4",
                NetState.Auth6 => "Auth6",
                _ => "不登录",
            };
        }
    }
}

using System.Globalization;

namespace TsinghuaNet
{
    public static class StringHelper
    {
        public static string GetFluxString(long flux)
        {
            double f = flux;
            if (f < 1000)
            {
                return $"{f} B";
            }
            else if ((f /= 1000) < 1000)
            {
                return $"{f:F2} KB";
            }
            else if ((f /= 1000) < 1000)
            {
                return $"{f:F2} MB";
            }
            else
            {
                f /= 1000;
                return $"{f:F2} GB";
            }
        }

        private static readonly CultureInfo zhCulture = new CultureInfo("zh-CN");
        public static string GetCurrencyString(decimal currency) => currency.ToString("C2", zhCulture);
    }
}
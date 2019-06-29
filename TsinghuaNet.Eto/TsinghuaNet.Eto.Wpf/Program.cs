using System;
using System.Globalization;
using System.Text;
using System.Threading;
using Eto;

namespace TsinghuaNet.Eto.Wpf
{
    class Program
    {
        [STAThread]
        static void Main()
        {
#if NETCOREAPP3_0
            // 规避.NET Core 3.0.0-preview6的bug
            var culture = CultureInfo.CreateSpecificCulture("en");
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            using (App app = new App(Platforms.Wpf))
            {
                app.Run();
            }
        }
    }
}

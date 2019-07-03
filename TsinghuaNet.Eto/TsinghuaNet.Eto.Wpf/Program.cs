using System;
using System.Text;
using Eto;

namespace TsinghuaNet.Eto.Wpf
{
    class Program
    {
        [STAThread]
        static void Main()
        {
#if NETCOREAPP
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            using (App app = new App(Platforms.Wpf))
            {
                app.Run();
            }
        }
    }
}

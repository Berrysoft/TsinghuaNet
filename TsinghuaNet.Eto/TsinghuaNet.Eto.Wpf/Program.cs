using System;
using System.Text;
using Eto.Forms.Controls.SkiaSharp;
using Eto.Forms.Controls.SkiaSharp.WPF;
using Eto.Wpf;
using TsinghuaNet.Eto.Controls;
using TsinghuaNet.Eto.Wpf.Controls;

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
            var platform = new Platform();
            platform.Add<SortableGridColumn.IHandler>(() => new SortableGridColumnHandler());
            platform.Add<SKControl.ISKControl>(() => new SKControlHandler());
            using (App app = new App(platform))
            {
                app.Run();
            }
        }
    }
}

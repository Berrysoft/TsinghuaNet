using System.Text;
using Eto.Forms.Controls.SkiaSharp;
using Eto.Forms.Controls.SkiaSharp.GTK;
using Eto.GtkSharp;
using TsinghuaNet.Eto.Controls;
using TsinghuaNet.Eto.Gtk.Controls;

namespace TsinghuaNet.Eto.Gtk
{
    class Program
    {
        static void Main()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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

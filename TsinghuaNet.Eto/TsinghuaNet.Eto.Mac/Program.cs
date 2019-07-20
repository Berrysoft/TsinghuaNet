using Eto.Forms.Controls.SkiaSharp;
using Eto.Forms.Controls.SkiaSharp.Mac;
using Eto.Mac;
using Eto.Mac.Forms;
using TsinghuaNet.Eto.Controls;
using TsinghuaNet.Eto.Mac.Controls;

namespace TsinghuaNet.Eto.Mac
{
    class Program
    {
        static void Main()
        {
            var platform = new Platform();
            platform.Add<SortableGridColumn.IHandler>(() => new SortableGridColumnHandler());
            platform.Add<SKControl.ISKControl>(() => new SKControlHandler());
            using (App app = new App(platform))
            {
                var handler = (ApplicationHandler)app.Handler;
                handler.AllowClosingMainForm = true;
                app.Run();
            }
        }
    }
}

using System.Text;
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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var platform = new Platform();
            platform.Add<SortableGridColumn.IHandler>(() => new SortableGridColumnHandler());
            using (App app = new App(platform))
            {
                var handler = (ApplicationHandler)app.Handler;
                handler.AllowClosingMainForm = true;
                app.Run();
            }
        }
    }
}

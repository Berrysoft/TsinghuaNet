using System.Reflection;
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
            // https://forums.xamarin.com/discussion/comment/263758/#comment_263758
            try
            {
                Assembly.Load("System.Configuration")?.GetType("System.Configuration.ConfigurationManager")?.GetMethod("GetSection", BindingFlags.Static | BindingFlags.Public)?.Invoke(null, new[] { "configuration" });
            }
            catch { }
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

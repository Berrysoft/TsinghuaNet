using System.Text;
using Eto;

namespace TsinghuaNet.Eto.Gtk
{
    class Program
    {
        static void Main()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (App app = new App(Platforms.Gtk))
            {
                app.Run();
            }
        }
    }
}

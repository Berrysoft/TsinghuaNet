using Eto;

namespace TsinghuaNet.Eto.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main()
        {
            using (App app = new App(Platforms.Ios))
            using (var form = new MainForm())
            {
                app.Run(form);
            }
        }
    }
}
using Eto;

namespace TsinghuaNet.Eto.Mac
{
    class Program
    {
        static void Main()
        {
            using (App app = new App(Platforms.Wpf))
            {
                app.Run();
            }
        }
    }
}

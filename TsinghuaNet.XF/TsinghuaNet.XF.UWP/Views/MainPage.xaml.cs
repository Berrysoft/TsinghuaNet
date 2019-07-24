using Windows.UI.Xaml;
using Xamarin.Forms.DataGrid;
using Xamarin.Forms.Platform.UWP;
using xf = Xamarin.Forms;

namespace TsinghuaNet.XF.UWP.Views
{
    public sealed partial class MainPage : WindowsPage
    {
        public MainPage()
        {
            InitializeComponent();
            var xfApp = new TsinghuaNet.XF.App();
            SetColorResource(xfApp);
            LoadApplication(xfApp);
        }

        private void WindowsPage_ActualThemeChanged(FrameworkElement sender, object args)
        {
            SetColorResource(xf.Application.Current);
        }

        private void SetColorResource(xf.Application app)
        {
            ((PaletteCollection)app.Resources["DataGridBackgroundPalette"])[0] = ActualTheme == ElementTheme.Dark ? xf.Color.Black : xf.Color.White;
            ((PaletteCollection)app.Resources["DataGridForegroundPalette"])[0] = ActualTheme == ElementTheme.Dark ? xf.Color.White : xf.Color.Black;
            app.Resources["DefaultPageBackground"] = ActualTheme == ElementTheme.Dark ? xf.Color.Black : xf.Color.White;
        }
    }
}

using Xamarin.Forms;
using Xamarin.Forms.DataGrid;
using Xamarin.Forms.Platform.UWP;

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

        private void WindowsPage_ActualThemeChanged(Windows.UI.Xaml.FrameworkElement sender, object args)
        {
            SetColorResource(Application.Current);
        }

        private void SetColorResource(Application app)
        {
            ((PaletteCollection)app.Resources["DataGridBackgroundPalette"])[0] = ActualTheme == Windows.UI.Xaml.ElementTheme.Dark ? Color.Black : Color.White;
            ((PaletteCollection)app.Resources["DataGridForegroundPalette"])[0] = ActualTheme == Windows.UI.Xaml.ElementTheme.Dark ? Color.White : Color.Black;
            app.Resources["DefaultPageBackground"] = ActualTheme == Windows.UI.Xaml.ElementTheme.Dark ? Color.Black : Color.White;
        }
    }
}

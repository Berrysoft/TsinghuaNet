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
            SetDataGridForeground(xfApp);
            LoadApplication(xfApp);
        }

        private void WindowsPage_ActualThemeChanged(Windows.UI.Xaml.FrameworkElement sender, object args)
        {
            SetDataGridForeground(Application.Current);
        }

        private void SetDataGridForeground(Application app)
        {
            ((PaletteCollection)app.Resources["DataGridBackgroundPalette"])[0] = ActualTheme == Windows.UI.Xaml.ElementTheme.Dark ? Color.Black : Color.White;
            ((PaletteCollection)app.Resources["DataGridForegroundPalette"])[0] = ActualTheme == Windows.UI.Xaml.ElementTheme.Dark ? Color.White : Color.Black;
        }
    }
}

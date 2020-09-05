using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
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
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
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
            var foreground = ActualTheme == ElementTheme.Dark ? xf.Color.White : xf.Color.Black;
            var background = ActualTheme == ElementTheme.Dark ? xf.Color.Black : xf.Color.WhiteSmoke;
            ((PaletteCollection)app.Resources["DataGridForegroundPalette"])[0] = foreground;
            app.Resources["PopupBackground"] = background;
        }
    }
}

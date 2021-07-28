using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
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
            app.Resources["PopupBackground"] = background;
            app.Resources["SystemAccentColor"] = ((Color)Application.Current.Resources["SystemAccentColor"]).ToFormsColor();
            app.Resources["SystemAccentColorDark1"] = ((Color)Application.Current.Resources["SystemAccentColorDark1"]).ToFormsColor();
            app.Resources["SystemAccentColorDark2"] = ((Color)Application.Current.Resources["SystemAccentColorDark2"]).ToFormsColor();
        }
    }
}

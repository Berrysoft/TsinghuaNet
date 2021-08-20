using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.Helpers;
using Syncfusion.XForms.Themes;
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
            var xfApp = new TsinghuaNet.XF.App(GetColorTheme(), GetOtherResources());
            LoadApplication(xfApp);
        }

        private void WindowsPage_ActualThemeChanged(FrameworkElement sender, object args)
        {
            SetColorResource(xf.Application.Current);
        }

        private void SetColorResource(xf.Application app)
        {
            ((TsinghuaNet.XF.App)app).UpdateResources(GetOtherResources());

            var mergedDictionaries = app.Resources.MergedDictionaries;
            mergedDictionaries.Clear();
            mergedDictionaries.Add(GetColorTheme());
        }

        private xf.ResourceDictionary GetColorTheme()
        {
            if (ActualTheme == ElementTheme.Dark)
            {
                return new DarkTheme();
            }
            else
            {
                return new LightTheme();
            }
        }

        private Dictionary<string, object> GetOtherResources()
        {
            var res = new Dictionary<string, object>
            {
                ["SystemAccentColor"] = ((Color)Application.Current.Resources["SystemAccentColor"]).ToFormsColor(),
                ["SystemAccentColorDark1"] = ((Color)Application.Current.Resources["SystemAccentColorDark1"]).ToFormsColor(),
                ["SystemAccentColorDark2"] = ((Color)Application.Current.Resources["SystemAccentColorDark2"]).ToFormsColor()
            };
            if (SystemInformation.Instance.OperatingSystemVersion.Build < 22000)
            {
                res.Add("SymbolFont", "Segoe MDL2 Assets");
            }
            return res;
        }
    }
}

using System.Collections.Generic;
using Syncfusion.Licensing;
using TsinghuaNet.XF.Views;
using Xamarin.Forms;

namespace TsinghuaNet.XF
{
    public partial class App : Application
    {
        internal Color SystemAccentColor => (Color)Resources["SystemAccentColor"];
        internal Color SystemAccentColorDark1 => (Color)Resources["SystemAccentColorDark1"];
        internal Color SystemAccentColorDark2 => (Color)Resources["SystemAccentColorDark2"];

        public App(ResourceDictionary dict, IDictionary<string, object> res = null) : base()
        {
            SyncfusionLicenseProvider.RegisterLicense("NjE2NjMyQDMyMzAyZTMxMmUzMEYvR3dQOUszZTN2bTVwalZVdVhXWTZpYjloQ1c3SHRMWVl5RjFGcVVBbDg9;NjE2NjMzQDMyMzAyZTMxMmUzMFlTOEpqRzNETHpMMllRYTdTc3YxMkF3Qlo5eXlOb2c0OUxYckFFQk1GK2M9;NjE2NjM0QDMyMzAyZTMxMmUzMEJYaFBJb0phMDFCTFZHamRXSXdlVVdYbjFiWnlsZ2dlRHdFRlVJTkd6YTg9");
            InitializeComponent();
            Resources.MergedDictionaries.Add(dict);
            if (res != null)
            {
                UpdateResources(res);
            }
            MainPage = new MainPage();
        }

        public void UpdateResources(IDictionary<string, object> res)
        {
            foreach (var p in res)
            {
                Resources[p.Key] = p.Value;
            }
        }

        protected override void OnSleep()
        {
            ((MainPage)MainPage).SaveSettings();
            base.OnSleep();
        }
    }
}

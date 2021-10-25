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
            SyncfusionLicenseProvider.RegisterLicense("NTI0MzE4QDMxMzkyZTMzMmUzME4zQXdDNUtBQlJEZUhNL001V05jZGFWcHVMNVNiRCtPNGN3MmU5VjVtVlE9;NTI0MzE5QDMxMzkyZTMzMmUzMEZDOGNrVDdCRXZ4aXRpbUlMR1EwM1Y5Z081YmFtc2lucEg5ZGUzVDlFdjQ9;NTI0MzIwQDMxMzkyZTMzMmUzMEhtQVVmaFFSbW84bjlsK3hlRlRETUdFQWEzNmNSejFPYlhVc0JZTEtaVmc9;NTI0MzIxQDMxMzkyZTMzMmUzMGtFVzJHTXc5UlE4YlZyVDBtbVRYV093SFpZS0Fra0RnYmt0ZllJL1RJQXM9;NTI0MzIyQDMxMzkyZTMzMmUzMGZzOWFBY0phSnRDYTlEbndSOVUxdGNOSW9Fd01GeHhXNHZzaVpwNG5VVGc9");
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

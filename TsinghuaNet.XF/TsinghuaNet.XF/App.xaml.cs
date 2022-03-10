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
            SyncfusionLicenseProvider.RegisterLicense("NTkzOTk3QDMxMzkyZTM0MmUzMGNJY3BIN2F0L1dCMVZmOWZObUgwNHYwd1pQbUNVbGRSYXUzNURaMk1mcmc9;NTkzOTk4QDMxMzkyZTM0MmUzMFBpRWdZTnJUWUxyVENRUU41RzYyTHIvZTcrUXh1TVBqZmpuays1UWRRREU9;NTkzOTk5QDMxMzkyZTM0MmUzMEt2TnQvajVjekFtM0JvMjUwRS9LQk9JdVVDeTBSTXZ1NWY2Z1FkemdVUzQ9");
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

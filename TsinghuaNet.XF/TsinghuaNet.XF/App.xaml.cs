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

        public App(ResourceDictionary res) : base()
        {
            SyncfusionLicenseProvider.RegisterLicense("NDgwNzY2QDMxMzkyZTMyMmUzMERkRzJrcXZwd1pSWnhIOGRUTUQ0TWhkY0R5Y01ZUHhGY1Z5VEloVmJkSjQ9");
            InitializeComponent();
            Resources.MergedDictionaries.Add(res);
            MainPage = new MainPage();
        }

        protected override void OnSleep()
        {
            ((MainPage)MainPage).SaveSettings();
            base.OnSleep();
        }
    }
}

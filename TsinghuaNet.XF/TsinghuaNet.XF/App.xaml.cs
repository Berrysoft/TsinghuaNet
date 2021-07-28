using TsinghuaNet.XF.Views;
using Xamarin.Forms;
using Xamarin.Forms.DataGrid;
using Syncfusion.Licensing;

namespace TsinghuaNet.XF
{
    public partial class App : Application
    {
        internal Color SystemAccentColor => (Color)Resources["SystemAccentColor"];
        internal Color SystemAccentColorDark1 => (Color)Resources["SystemAccentColorDark1"];
        internal Color SystemAccentColorDark2 => (Color)Resources["SystemAccentColorDark2"];

        public App()
        {
            SyncfusionLicenseProvider.RegisterLicense("NDgwNzY2QDMxMzkyZTMyMmUzMERkRzJrcXZwd1pSWnhIOGRUTUQ0TWhkY0R5Y01ZUHhGY1Z5VEloVmJkSjQ9");
            InitializeComponent();
            DataGridComponent.Init();
            MainPage = new MainPage();
        }

        protected override void OnSleep()
        {
            ((MainPage)MainPage).SaveSettings();
            base.OnSleep();
        }
    }
}

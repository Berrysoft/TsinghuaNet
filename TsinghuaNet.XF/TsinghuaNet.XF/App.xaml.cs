using TsinghuaNet.XF.Views;
using Xamarin.Forms;
using Xamarin.Forms.DataGrid;

namespace TsinghuaNet.XF
{
    public partial class App : Application
    {
        internal static readonly Color SystemAccentColor = Color.FromUint(0xFF0078D7);
        internal static readonly Color SystemAccentColorDark1 = Color.FromUint(0xFF005A9E);
        internal static readonly Color SystemAccentColorDark2 = Color.FromUint(0xFF004275);

        public App()
        {
            InitializeComponent();
            DataGridComponent.Init();
            MainPage = new MainPage();
        }

        protected override async void OnSleep()
        {
            await ((MainPage)MainPage).SaveSettingsAsync();
            base.OnSleep();
        }
    }
}

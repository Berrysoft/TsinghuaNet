using TsinghuaNet.XF.Views;
using Xamarin.Forms;
using Xamarin.Forms.DataGrid;

namespace TsinghuaNet.XF
{
    public partial class App : Application
    {
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

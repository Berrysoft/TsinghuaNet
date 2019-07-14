using TsinghuaNet.Models;
using TsinghuaNet.XF.Services;
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
            DependencyService.Register<INetXFSettings>();
            DependencyService.Register<INetStatus>();
            DependencyService.Register<ICredentialStore>();
            DataGridComponent.Init();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

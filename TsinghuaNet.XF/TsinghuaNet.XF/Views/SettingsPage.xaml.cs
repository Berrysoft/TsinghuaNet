using System;
using System.Net;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TsinghuaNet.XF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void ShowChangeUser(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new ChangeUserPage());
        }

        private async void DropUser(object sender, IPAddress e)
        {
            await Model.ConnectionModel.DropAsync(e);
        }

        protected override void OnAppearing()
        {
            Model.ConnectionModel.RefreshNetUsers();
            base.OnAppearing();
        }
    }
}
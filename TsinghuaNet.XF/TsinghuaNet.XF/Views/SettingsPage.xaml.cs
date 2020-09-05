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

        private async void ShowConnectIP(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new ConnectIPPage(Model.ConnectionModel));
        }

        private async void DropUser(object sender, IPAddress e)
        {
            await Model.ConnectionModel.DropAsync(e);
        }

        protected override void OnAppearing()
        {
            if (!Model.ConnectionModel.IsBusy && (Model.ConnectionModel.NetUsers == null || Model.ConnectionModel.NetUsers.Count == 0))
                Model.ConnectionModel.RefreshNetUsers();
            base.OnAppearing();
        }
    }
}
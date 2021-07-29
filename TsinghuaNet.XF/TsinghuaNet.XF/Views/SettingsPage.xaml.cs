using System;
using System.Net;
using Syncfusion.XForms.PopupLayout;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TsinghuaNet.XF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private readonly SfPopupLayout layout = new SfPopupLayout() { OverlayMode = OverlayMode.Blur };

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void ShowChangeUser(object sender, EventArgs e)
        {
            layout.SetValue(SfPopupLayout.PopupViewProperty, new ChangeUserPage());
            layout.Show();
        }

        private void ShowConnectIP(object sender, EventArgs e)
        {
            layout.SetValue(SfPopupLayout.PopupViewProperty, new ConnectIPPage(Model.ConnectionModel));
            layout.Show();
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
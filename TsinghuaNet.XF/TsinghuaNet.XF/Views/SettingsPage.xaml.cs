using System;
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
    }
}
using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace TsinghuaNet.XF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangeUserPage : PopupPage
    {
        public ChangeUserPage()
        {
            InitializeComponent();
        }

        private async void ConfirmChangeUser(object sender, EventArgs e)
        {
            Model.Confirm();
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
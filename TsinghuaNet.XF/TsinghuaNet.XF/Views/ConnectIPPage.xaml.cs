using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using TsinghuaNet.ViewModels;
using Xamarin.Forms.Xaml;

namespace TsinghuaNet.XF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectIPPage : PopupPage
    {
        public ConnectIPPage(ConnectionViewModel viewModel)
        {
            InitializeComponent();
            Model.ConnectionModel = viewModel;
        }

        private async void ConfirmConnectIP(object sender, EventArgs e)
        {
            await Model.ConfirmAsync();
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
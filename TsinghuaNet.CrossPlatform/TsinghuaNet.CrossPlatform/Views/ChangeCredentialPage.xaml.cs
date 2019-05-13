using System;
using System.ComponentModel;
using TsinghuaNet.CrossPlatform.ViewModels;
using Xamarin.Forms;

namespace TsinghuaNet.CrossPlatform.Views
{
    [DesignTimeVisible(true)]
    public partial class ChangeCredentialPage : ContentPage
    {
        private ChangeCredentialViewModel viewModel;
        public ChangeCredentialPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ChangeCredentialViewModel();
        }

        async void Confirm(object sender, EventArgs e)
        {
            viewModel.Confirm();
            await Navigation.PopAsync();
        }
    }
}
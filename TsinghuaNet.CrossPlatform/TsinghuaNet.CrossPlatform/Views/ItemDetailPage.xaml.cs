using System;
using System.ComponentModel;
using TsinghuaNet.CrossPlatform.ViewModels;
using Xamarin.Forms;

namespace TsinghuaNet.CrossPlatform.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        private async void DropSelf(object sender, EventArgs e)
        {
            await viewModel.DropAsync();
            await Navigation.PopAsync();
        }
    }
}
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace TsinghuaNet.CrossPlatform.Views
{
    [DesignTimeVisible(true)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        async void ChangeCredential(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangeCredentialPage());
        }
    }
}
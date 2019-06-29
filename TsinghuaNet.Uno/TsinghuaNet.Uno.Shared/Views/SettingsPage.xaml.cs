using System.Net;
using TsinghuaNet.Uno.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TsinghuaNet.Uno.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            if (Resources["DropUser"] is DropCommand c)
            {
                c.DropUser += DropUser;
            }
        }

        private async void PageLoaded(object sender, RoutedEventArgs e)
        {
            await ConnectionModel.RefreshNetUsersAsync();
        }

        private async void DropUser(object sender, IPAddress e) => await ConnectionModel.DropAsync(e);
    }
}

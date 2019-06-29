using System.Net;
using TsinghuaNet.Uno.Helpers;
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

        private void ChangeUserOpened(object sender, object e)
        {
            UnBox.Text = Model.Credential.Username ?? string.Empty;
            UsernameChanged();
        }

        private void UsernameChanged()
        {
            string un = UnBox.Text;
            string pw = CredentialHelper.GetCredential(un) ?? string.Empty;
            PwBox.Password = pw;
            SaveBox.IsChecked = !string.IsNullOrEmpty(pw);
        }

        private void ConfirmChangeUser(object sender, RoutedEventArgs e)
        {
            ChangeUserFlyout.Hide();
            string un = UnBox.Text;
            string pw = PwBox.Password;
            // 不管是否保存，都需要先删除
            CredentialHelper.RemoveCredential(un);
            if (SaveBox.IsChecked.Value)
                CredentialHelper.SaveCredential(un, pw);
            // 同步
            Model.Credential.Username = un;
            Model.Credential.Password = pw;
        }
    }
}

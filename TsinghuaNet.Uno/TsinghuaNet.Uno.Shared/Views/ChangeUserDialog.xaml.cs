using TsinghuaNet.Uno.Helpers;
using Windows.UI.Xaml.Controls;

namespace TsinghuaNet.Uno.Views
{
    public sealed partial class ChangeUserDialog : ContentDialog
    {
        public ChangeUserDialog()
        {
            InitializeComponent();
        }

        public ChangeUserDialog(string username)
        {
            InitializeComponent();
            UnBox.Text = username ?? string.Empty;
            UsernameChanged();
        }

        private void UsernameChanged()
        {
            string un = UnBox.Text;
            string pw = CredentialHelper.GetCredential(un) ?? string.Empty;
            IsPrimaryButtonEnabled = !(string.IsNullOrEmpty(un) || string.IsNullOrEmpty(pw));
            PwBox.Password = pw;
            SaveBox.IsChecked = !string.IsNullOrEmpty(pw);
        }

        private void PasswordChanged()
        {
            IsPrimaryButtonEnabled = !(string.IsNullOrEmpty(UnBox.Text) || string.IsNullOrEmpty(PwBox.Password));
        }
    }
}

namespace TsinghuaNet.CrossPlatform.ViewModels
{
    public class ChangeCredentialViewModel : BaseViewModel
    {
        public ChangeCredentialViewModel()
        {
            Title = "更改用户";
            Username = Credential.Username;
            Password = Credential.Password;
        }

        private string username;
        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public void Confirm()
        {
            Credential.Username = Username;
            Credential.Password = Password;
        }
    }
}

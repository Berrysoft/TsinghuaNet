using System.Windows.Input;
using TsinghuaNet.Models;
using TsinghuaNet.ViewModels;

namespace TsinghuaNet.XF.ViewModels
{
    class ChangeUserViewModel : NetViewModelBase
    {
        public ChangeUserViewModel() : base()
        {
            NewUsername = Credential.Username;
            NewPassword = Credential.Password;
            ConfirmCommand = new Command(this, Confirm);
        }

        public string NewUsername { get; set; }
        public string NewPassword { get; set; }

        public void Confirm()
        {
            Credential.Username = NewUsername;
            Credential.Password = NewPassword;
        }

        public ICommand ConfirmCommand { get; }
    }
}

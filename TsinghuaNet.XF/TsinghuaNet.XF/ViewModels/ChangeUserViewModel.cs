using System;
using System.Collections.Generic;
using System.Text;
using TsinghuaNet.ViewModels;

namespace TsinghuaNet.XF.ViewModels
{
    class ChangeUserViewModel : NetViewModelBase
    {
        public ChangeUserViewModel() : base()
        {
            NewUsername = Credential.Username;
            NewPassword = Credential.Password;
        }

        public string NewUsername { get; set; }
        public string NewPassword { get; set; }

        public void Confirm()
        {
            Credential.Username = NewUsername;
            Credential.Password = NewPassword;
        }
    }
}

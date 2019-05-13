using MvvmHelpers;

namespace TsinghuaNet.CrossPlatform.Services
{
    public class NetSettings : ObservableObject
    {
        private bool autoLogin;
        public bool AutoLogin
        {
            get => autoLogin;
            set => SetProperty(ref autoLogin, value);
        }
    }
}

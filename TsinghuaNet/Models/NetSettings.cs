using MvvmHelpers;

namespace TsinghuaNet.Models
{
    public class NetSettings : ObservableObject
    {
        private bool autoLogin;
        public bool AutoLogin
        {
            get => autoLogin;
            set => SetProperty(ref autoLogin, value);
        }

        private bool enableFluxLimit;
        public bool EnableFluxLimit
        {
            get => enableFluxLimit;
            set => SetProperty(ref enableFluxLimit, value);
        }

        private ByteSize fluxLimit;
        public ByteSize FluxLimit
        {
            get => fluxLimit;
            set => SetProperty(ref fluxLimit, value);
        }
    }
}

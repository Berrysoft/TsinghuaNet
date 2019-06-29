using MvvmHelpers;

namespace TsinghuaNet.Models
{
    public class NetObservableBase : ObservableObject
    {
        private static readonly NetCredential credential = new NetCredential();
        public NetCredential Credential => credential;

        private static NetSettings settings;
        public NetSettings Settings
        {
            get => settings;
            protected set => settings = value;
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value, onChanged: OnIsBusyChanged);
        }
        protected virtual void OnIsBusyChanged()
        {
        }
    }
}

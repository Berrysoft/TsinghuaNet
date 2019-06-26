using MvvmHelpers;

namespace TsinghuaNet.Model
{
    public class NetObservableBase : ObservableObject
    {
        private static readonly NetCredential credential = new NetCredential();

        public NetCredential Credential => credential;

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

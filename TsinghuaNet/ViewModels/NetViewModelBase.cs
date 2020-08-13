using System.ComponentModel;
using TsinghuaNet.Models;

namespace TsinghuaNet.ViewModels
{
    public class NetViewModelBase : INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        private static readonly NetCredential credential = new NetCredential();
        public NetCredential Credential => credential;

        private static NetSettingsBase settings;

        public NetSettingsBase Settings
        {
            get => settings;
            protected set => settings = value;
        }

        private static INetStatus status;

        public INetStatus Status
        {
            get => status;
            protected set => status = value;
        }
        protected virtual void OnStatusChanged() { }

        public bool IsBusy { get; set; }
    }
}

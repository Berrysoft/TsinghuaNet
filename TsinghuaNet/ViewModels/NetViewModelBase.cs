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

        private static NetSettings settings;

        public NetSettings Settings
        {
            get => settings;
            protected set => settings = value;
        }

        public bool IsBusy { get; set; }
    }
}

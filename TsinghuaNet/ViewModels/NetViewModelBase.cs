using System.ComponentModel;
using TsinghuaNet.Models;

namespace TsinghuaNet.ViewModels
{
    public class NetViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

using PropertyChanged;
using TsinghuaNet.Uno.Helpers;
using TsinghuaNet.ViewModels;

namespace TsinghuaNet.Uno.ViewModels
{
    class SettingsViewModel : NetViewModelBase
    {
        [DoNotNotify]
        public new NetUnoSettings Settings
        {
            get => (NetUnoSettings)base.Settings;
            set => base.Settings = value;
        }
    }
}

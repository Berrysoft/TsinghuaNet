using PropertyChanged;
using TsinghuaNet.ViewModels;

namespace TsinghuaNet.Uno.ViewModels
{
    class SettingsViewModel : NetViewModelBase
    {
        [DoNotNotify]
        public new TsinghuaNet.Uno.Helpers.NetSettings Settings
        {
            get => (TsinghuaNet.Uno.Helpers.NetSettings)base.Settings;
            set => base.Settings = value;
        }
    }
}

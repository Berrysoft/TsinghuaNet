using TsinghuaNet.Models;

namespace TsinghuaNet.Uno.ViewModels
{
    class SettingsViewModel : NetObservableBase
    {
        public new TsinghuaNet.Uno.Helpers.NetSettings Settings
        {
            get => (TsinghuaNet.Uno.Helpers.NetSettings)base.Settings;
            set => base.Settings = value;
        }
    }
}

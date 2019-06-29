using TsinghuaNet.Models;

namespace TsinghuaNet.Uno.ViewModels
{
    class SettingsViewModel : NetObservableBase
    {
        public new NetSettings Settings
        {
            get => (NetSettings)base.Settings;
            set => base.Settings = value;
        }
    }
}

using TsinghuaNet.Models;

namespace TsinghuaNet.Eto.ViewModels
{
    public class SettingsViewModel : NetObservableBase
    {
        public new NetSettings Settings
        {
            get => (NetSettings)base.Settings;
            set => base.Settings = value;
        }
    }
}

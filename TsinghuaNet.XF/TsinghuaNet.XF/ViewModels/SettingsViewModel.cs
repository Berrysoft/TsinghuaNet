using PropertyChanged;
using TsinghuaNet.ViewModels;
using TsinghuaNet.XF.Models;
using Xamarin.Forms;

namespace TsinghuaNet.XF.ViewModels
{
    class SettingsViewModel : NetViewModelBase
    {
        [DoNotNotify]
        public new NetXFSettings Settings
        {
            get => (NetXFSettings)base.Settings;
            set => base.Settings = value;
        }

        public ConnectionViewModel ConnectionModel { get; } = new ConnectionViewModel();

        public bool IsUWP => Device.RuntimePlatform == Device.UWP;
    }
}

using System.Threading.Tasks;
using TsinghuaNet.Models;
using TsinghuaNet.ViewModels;
using TsinghuaNet.XF.Models;
using TsinghuaNet.XF.Services;
using Xamarin.Forms;

namespace TsinghuaNet.XF.ViewModels
{
    class MainViewModel : MainViewModelBase
    {
        public new NetXFSettings Settings
        {
            get => (NetXFSettings)base.Settings;
            set => base.Settings = value;
        }

        public override Task LoadSettingsAsync()
        {
            Settings = new NetXFSettings();
            Settings.LoadSettings();
            var store = DependencyService.Get<ICredentialStore>();
            if (store.CredentialExists())
                store.LoadCredential(Credential);
            Status = DependencyService.Get<INetStatus>();
            return Task.CompletedTask;
        }

        public override Task SaveSettingsAsync()
        {
            Settings.SaveSettings();
            DependencyService.Get<ICredentialStore>().SaveCredential(Credential);
            return Task.CompletedTask;
        }
    }
}

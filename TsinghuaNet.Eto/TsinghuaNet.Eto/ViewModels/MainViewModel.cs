using System;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;
using PropertyChanged;
using TsinghuaNet.Eto.Models;
using TsinghuaNet.ViewModels;
using TsinghuaNet.Models;

namespace TsinghuaNet.Eto.ViewModels
{
    public class MainViewModel : MainViewModelBase
    {
        public MainViewModel() : base()
        {
            Status = new NetPingStatus();
            ReceivedResponse += Model_ReceivedResponse;
            timer = new UITimer(OnlineTimerTick);
            timer.Interval = 1;
            LoadSettings();
        }

        [DoNotNotify]
        public new NetEtoSettings Settings
        {
            get => (NetEtoSettings)base.Settings;
            set => base.Settings = value;
        }

        public override async void LoadSettings()
        {
            Settings = (await SettingsHelper.Helper.ReadSettingsAsync<NetEtoSettings>()) ?? new NetEtoSettings();
            Credential.Username = Settings.Username ?? string.Empty;
            Credential.Password = Encoding.UTF8.GetString(Convert.FromBase64String(Settings.Password ?? string.Empty));

            if (Settings.AutoLogin)
                await LoginAsync();
        }

        public override void SaveSettings()
        {
            if (Settings.DeleteSettingsOnExit)
            {
                SettingsHelper.Helper.DeleteSettings();
            }
            else
            {
                Settings.Username = Credential.Username;
                Settings.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(Credential.Password));
                SettingsHelper.Helper.WriteSettings(Settings);
            }
        }

        private UITimer timer;
        private void OnlineTimerTick(object sender, EventArgs e)
        {
            OnlineTime += TimeSpan.FromSeconds(1);
        }

        protected override async Task<LogResponse> RefreshAsync(IConnect helper)
        {
            var res = await base.RefreshAsync(helper);
            timer.Stop();
            OnlineTime = OnlineUser.OnlineTime;
            if (Settings.UseTimer && !string.IsNullOrEmpty(OnlineUser.Username))
                timer.Start();
            var remainFlux = FluxHelper.GetMaxFlux(OnlineUser.Flux, OnlineUser.Balance) - OnlineUser.Flux;
            if (Settings.EnableFluxLimit && remainFlux < Settings.FluxLimit)
                res = new LogResponse(false, $"流量仅剩余{remainFlux}");
            return res;
        }

        private void Model_ReceivedResponse(object sender, LogResponse res) => Response = res.Message;
    }
}

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

        public float FluxOffset { get; set; }
        public float FreeOffset { get; set; }

        public event EventHandler Refreshed;

        protected override async Task<LogResponse> RefreshAsync(IConnect helper)
        {
            var res = await base.RefreshAsync(helper);
            timer.Stop();
            OnlineTime = OnlineUser.OnlineTime;
            if (Settings.UseTimer && !string.IsNullOrEmpty(OnlineUser.Username))
                timer.Start();
            if (Settings.EnableFluxLimit && OnlineUser.Flux > Settings.FluxLimit)
                res = new LogResponse(false, $"流量已使用超过{Settings.FluxLimit}");
            var maxf = FluxHelper.GetMaxFlux(OnlineUser.Flux, OnlineUser.Balance);
            FluxOffset = (float)(OnlineUser.Flux / maxf);
            FreeOffset = (float)Math.Max(FluxHelper.BaseFlux / maxf, FluxOffset);
            Refreshed?.Invoke(this, EventArgs.Empty);
            return res;
        }

        private void Model_ReceivedResponse(object sender, LogResponse res) => Response = res.Message;
    }
}

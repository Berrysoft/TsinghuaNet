using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Timers;
using PropertyChanged;
using TsinghuaNet.Models;
using TsinghuaNet.ViewModels;
using TsinghuaNet.XF.Models;
using TsinghuaNet.XF.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TsinghuaNet.XF.ViewModels
{
    class MainViewModel : MainViewModelBase
    {
        private readonly Timer mainTimer = new Timer();

        [DoNotNotify]
        public new NetXFSettings Settings
        {
            get => (NetXFSettings)base.Settings;
            set => base.Settings = value;
        }

        public MainViewModel() : base()
        {
            Status = DependencyService.Get<INetStatus>();
            ReceivedResponse += MainViewModel_ReceivedResponse;
            mainTimer.Interval = 1000;
            mainTimer.Elapsed += MainTimerTick;
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
            LoadSettings();
        }

        public override async void LoadSettings()
        {
            Settings = new NetXFSettings();
            Settings.PropertyChanged += OnSettingsPropertyChanged;
            Settings.LoadSettings();
            var store = new CredentialStore();
            if (store.CredentialExists())
                await store.LoadCredentialAsync(Credential);
            if (Settings.AutoLogin && !string.IsNullOrEmpty(Credential.Username))
                await LoginAsync();
        }

        public override async void SaveSettings()
        {
            Settings.SaveSettings();
            if (!string.IsNullOrEmpty(Credential.Username))
            {
                var store = new CredentialStore();
                await store.SaveCredentialAsync(Credential);
            }
        }

        private async void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var helper = DependencyService.Get<IBackgroundManager>();
            switch (e.PropertyName)
            {
                case "BackgroundAutoLogin":
                    if (await helper.RequestAccessAsync())
                        helper.RegisterLogin(Settings.BackgroundAutoLogin);
                    break;
                case "BackgroundLiveTile":
                    if (await helper.RequestAccessAsync())
                        helper.RegisterLiveTile(Settings.BackgroundLiveTile);
                    break;
            }
        }

        private async void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            await RefreshStatusAsync();
            if (!string.IsNullOrEmpty(Credential.Username))
                await LoginAsync();
            else
                await RefreshAsync();
        }

        public float FluxOffset { get; set; }
        public float FreeOffset { get; set; }

        public event EventHandler Refreshed;

        protected override async Task<LogResponse> RefreshAsync(IConnect helper)
        {
            var res = await base.RefreshAsync(helper);
            mainTimer.Stop();
            OnlineTime = OnlineUser.OnlineTime;
            if (!string.IsNullOrEmpty(OnlineUser.Username))
                mainTimer.Start();
            if (Settings.EnableFluxLimit && OnlineUser.Flux > Settings.FluxLimit)
                res = new LogResponse(false, $"流量已使用超过{Settings.FluxLimit}");
            var maxf = FluxHelper.GetMaxFlux(OnlineUser.Flux, OnlineUser.Balance);
            FluxOffset = (float)(OnlineUser.Flux / maxf);
            FreeOffset = (float)Math.Max(FluxHelper.BaseFlux / maxf, FluxOffset);
            Refreshed?.Invoke(this, EventArgs.Empty);
            return res;
        }

        private void MainTimerTick(object sender, ElapsedEventArgs e) => OnlineTime += TimeSpan.FromSeconds(1);

        private void MainViewModel_ReceivedResponse(object sender, LogResponse e) => Response = e.Message;
    }
}

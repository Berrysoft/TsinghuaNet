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
            mainTimer.Interval = 1000;
            mainTimer.Elapsed += MainTimerTick;
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        public event EventHandler SettingsLoaded;
        protected void OnSettingsLoaded(EventArgs e) => SettingsLoaded?.Invoke(this, e);

        public override async void LoadSettings()
        {
            Settings = new NetXFSettings();
            Settings.PropertyChanged += OnSettingsPropertyChanged;
            Settings.LoadSettings();
            (Credential.Username, Credential.Password) = await CredentialStore.LoadCredentialAsync();
            Credential.UseProxy = Settings.UseProxy;
            if (Settings.AutoLogin && !string.IsNullOrEmpty(Credential.Username))
                await LoginAsync();
            OnSettingsLoaded(EventArgs.Empty);
        }

        public override async void SaveSettings()
        {
            Settings.UseProxy = Credential.UseProxy;
            Settings.SaveSettings();
            if (!string.IsNullOrEmpty(Credential.Username))
            {
                await CredentialStore.SaveCredentialAsync((Credential.Username, Credential.Password));
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
        protected void OnRefreshed(EventArgs e) => Refreshed?.Invoke(this, e);

        protected override async Task<LogResponse> RefreshAsync(IConnect helper)
        {
            var res = await base.RefreshAsync(helper);
            mainTimer.Stop();
            OnlineTime = OnlineUser.OnlineTime;
            if (!string.IsNullOrEmpty(OnlineUser.Username))
                mainTimer.Start();
            var maxf = FluxHelper.GetMaxFlux(OnlineUser.Flux, OnlineUser.Balance);
            FluxOffset = (float)(OnlineUser.Flux / maxf);
            FreeOffset = (float)Math.Max(FluxHelper.BaseFlux / maxf, FluxOffset);
            OnRefreshed(EventArgs.Empty);
            return res;
        }

        private void MainTimerTick(object sender, ElapsedEventArgs e) => OnlineTime += TimeSpan.FromSeconds(1);
    }
}

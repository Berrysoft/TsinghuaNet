using System;
using System.Threading.Tasks;
using System.Timers;
using TsinghuaNet.Models;
using TsinghuaNet.ViewModels;
using TsinghuaNet.XF.Models;
using TsinghuaNet.XF.Services;
using Xamarin.Forms;

namespace TsinghuaNet.XF.ViewModels
{
    class MainViewModel : MainViewModelBase
    {
        private readonly Timer mainTimer = new Timer();

        public new NetXFSettings Settings
        {
            get => (NetXFSettings)base.Settings;
            set => base.Settings = value;
        }

        public MainViewModel() : base()
        {
            mainTimer.Interval = 1000;
            mainTimer.Elapsed += MainTimerTick;
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

        protected override async Task<LogResponse> RefreshAsync(IConnect helper)
        {
            var res = await base.RefreshAsync(helper);
            mainTimer.Stop();
            // 更新磁贴
            //NotificationHelper.UpdateTile(OnlineUser);
            if (!string.IsNullOrEmpty(OnlineUser.Username))
                mainTimer.Start();
            if (Settings.EnableFluxLimit && OnlineUser.Flux > Settings.FluxLimit)
                res = new LogResponse(false, $"流量已使用超过{Settings.FluxLimit}");
            // 设置内容
            OnlineTime = OnlineUser.OnlineTime;
            //if (Window.Current.Content is Frame rootFrame)
            //{
            //    if (rootFrame.Content is MainPage mainPage)
            //    {
            //        mainPage.BeginFluxAnimation();
            //    }
            //}
            return res;
        }

        private void MainTimerTick(object sender, ElapsedEventArgs e)
        {
            OnlineTime += TimeSpan.FromSeconds(1);
        }
    }
}

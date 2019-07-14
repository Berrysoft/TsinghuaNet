using System;
using System.Threading.Tasks;
using System.Timers;
using PropertyChanged;
using TsinghuaNet.Models;
using TsinghuaNet.ViewModels;
using TsinghuaNet.XF.Models;
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
            mainTimer.Interval = 1000;
            mainTimer.Elapsed += MainTimerTick;
            ReceivedResponse += MainViewModel_ReceivedResponse;
        }

        public override async Task LoadSettingsAsync()
        {
            Settings = new NetXFSettings();
            Settings.LoadSettings();
            var store = new CredentialStore();
            if (store.CredentialExists())
                await store.LoadCredentialAsync(Credential);
            Status = DependencyService.Get<INetStatus>();
        }

        public override async Task SaveSettingsAsync()
        {
            Settings.SaveSettings();
            if (!string.IsNullOrEmpty(Credential.Username))
            {
                var store = new CredentialStore();
                await store.SaveCredentialAsync(Credential);
            }
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

        private void MainViewModel_ReceivedResponse(object sender, LogResponse e)
        {
            Response = e.Message;
        }
    }
}

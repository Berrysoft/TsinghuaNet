using System;
using System.Threading.Tasks;
using TsinghuaNet.Models;
using TsinghuaNet.Uno.Helpers;
using Windows.UI.Xaml;
using System.Windows.Input;
using TsinghuaNet.Helpers;
using Windows.UI.Xaml.Controls;
using TsinghuaNet.Uno.Views;

#if WINDOWS_UWP
using TsinghuaNet.Uno.UWP.Background;
#endif

namespace TsinghuaNet.Uno.ViewModels
{
    class MainViewModel : NetViewModel
    {
        private readonly DispatcherTimer mainTimer = new DispatcherTimer();

        public MainViewModel() : base()
        {
            Status = new InternetStatus();
            ChangeStateCommand = new ChangeEnumCommand<NetState>(s => Credential.State = s);
            // 设置计时器
            mainTimer.Interval = TimeSpan.FromSeconds(1);
            mainTimer.Tick += MainTimerTick;
        }

        public override void LoadSettings()
        {
            // 上一次登录的用户名
            var un = SettingsHelper.StoredUsername;
            // 设置为当前用户名并获取密码
            Credential.Username = un;
            Credential.Password = CredentialHelper.GetCredential(un);
            // 自动登录
            AutoLogin = SettingsHelper.AutoLogin;
            // 后台任务
            BackgroundAutoLogin = SettingsHelper.BackgroundAutoLogin;
            BackgroundLiveTile = SettingsHelper.BackgroundLiveTile;
            // 流量限制
            EnableFluxLimit = SettingsHelper.EnableFluxLimit;
            FluxLimit = SettingsHelper.FluxLimit;
        }

        public override void SaveSettings()
        {
            SettingsHelper.StoredUsername = Credential.Username;
            SettingsHelper.AutoLogin = AutoLogin;
            SettingsHelper.EnableFluxLimit = EnableFluxLimit;
            SettingsHelper.FluxLimit = FluxLimit;
        }

        public ICommand ChangeStateCommand { get; }

        protected override async Task<LogResponse> RefreshAsync(IConnect helper)
        {
            var res = await base.RefreshAsync(helper);
            // 先启动计时器
            mainTimer?.Start();
            // 更新磁贴
            NotificationHelper.UpdateTile(OnlineUser);
            if (EnableFluxLimit)
                NotificationHelper.SendWarningToast(OnlineUser, FluxLimit);
            // 设置内容
            OnlineTime = OnlineUser.OnlineTime;
            if (Window.Current.Content is Frame rootFrame)
            {
                if (rootFrame.Content is MainPage mainPage)
                {
                    mainPage.BeginFluxAnimation();
                }
            }
            return res;
        }

        private TimeSpan onlineTime;
        public TimeSpan OnlineTime
        {
            get => onlineTime;
            set => SetProperty(ref onlineTime, value);
        }

        private void MainTimerTick(object sender, object e)
        {
            if (OnlineUser.Username == null || string.IsNullOrEmpty(OnlineUser.Username))
                mainTimer.Stop();
            else
                OnlineTime += TimeSpan.FromSeconds(1);
        }

        private string response;
        public string Response
        {
            get => response;
            set => SetProperty(ref response, value);
        }

        private bool backgroundAutoLogin;
        public bool BackgroundAutoLogin
        {
            get => backgroundAutoLogin;
            set => SetProperty(ref backgroundAutoLogin, value, onChanged: OnBackgroundAutoLoginChanged);
        }
        private async void OnBackgroundAutoLoginChanged()
        {
            SettingsHelper.BackgroundAutoLogin = BackgroundAutoLogin;
#if WINDOWS_UWP
            if (await BackgroundHelper.RequestAccessAsync())
                BackgroundHelper.RegisterLogin(BackgroundAutoLogin);
#endif
        }

        private bool backgroundLiveTile;
        public bool BackgroundLiveTile
        {
            get => backgroundLiveTile;
            set => SetProperty(ref backgroundLiveTile, value, onChanged: OnBackgroundLiveTileChanged);
        }

        private async void OnBackgroundLiveTileChanged()
        {
            SettingsHelper.BackgroundLiveTile = BackgroundLiveTile;
#if WINDOWS_UWP
            if (await BackgroundHelper.RequestAccessAsync())
                BackgroundHelper.RegisterLiveTile(BackgroundLiveTile);
#endif
        }
    }
}

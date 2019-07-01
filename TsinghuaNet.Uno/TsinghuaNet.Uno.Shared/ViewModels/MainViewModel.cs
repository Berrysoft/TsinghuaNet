using System;
using System.Threading.Tasks;
using TsinghuaNet.Models;
using TsinghuaNet.Uno.Helpers;
using Windows.UI.Xaml;
using System.Windows.Input;
using TsinghuaNet.Helpers;
using Windows.UI.Xaml.Controls;
using TsinghuaNet.Uno.Views;
using System.Text;

namespace TsinghuaNet.Uno.ViewModels
{
    class MainViewModel : NetViewModel
    {
        private readonly DispatcherTimer mainTimer = new DispatcherTimer();

        public new NetSettings Settings
        {
            get => (NetSettings)base.Settings;
            set => base.Settings = value;
        }

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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Settings = new NetSettings();
            // 上一次登录的用户名
            var un = SettingsHelper.StoredUsername;
            // 设置为当前用户名并获取密码
            Credential.Username = un;
            Credential.Password = CredentialHelper.GetCredential(un);
            // 自动登录
            Settings.AutoLogin = SettingsHelper.AutoLogin;
#if WINDOWS_UWP
            // 后台任务
            Settings.BackgroundAutoLogin = SettingsHelper.BackgroundAutoLogin;
            Settings.BackgroundLiveTile = SettingsHelper.BackgroundLiveTile;
#endif
            // 流量限制
            Settings.EnableFluxLimit = SettingsHelper.EnableFluxLimit;
            Settings.FluxLimit = SettingsHelper.FluxLimit;
        }

        public override void SaveSettings()
        {
            SettingsHelper.StoredUsername = Credential.Username;
            SettingsHelper.AutoLogin = Settings.AutoLogin;
            SettingsHelper.EnableFluxLimit = Settings.EnableFluxLimit;
            SettingsHelper.FluxLimit = Settings.FluxLimit;
        }

        public ICommand ChangeStateCommand { get; }

        protected override async Task<LogResponse> RefreshAsync(IConnect helper)
        {
            var res = await base.RefreshAsync(helper);
            // 先启动计时器
            mainTimer.Stop();
            // 更新磁贴
            NotificationHelper.UpdateTile(OnlineUser);
            if (!string.IsNullOrEmpty(OnlineUser.Username))
                mainTimer.Start();
            if (Settings.EnableFluxLimit && OnlineUser.Flux > Settings.FluxLimit)
                res = new LogResponse(false, $"流量已使用超过{Settings.FluxLimit}");
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

        private void MainTimerTick(object sender, object e)
        {
            OnlineTime += TimeSpan.FromSeconds(1);
        }
    }
}

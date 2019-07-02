using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TsinghuaNet.Helpers;
using TsinghuaNet.Models;
using TsinghuaNet.Uno.Helpers;
using TsinghuaNet.Uno.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TsinghuaNet.Uno.ViewModels
{
    class MainViewModel : NetViewModel
    {
        private readonly DispatcherTimer mainTimer = new DispatcherTimer();

        public new TsinghuaNet.Uno.Helpers.NetSettings Settings
        {
            get => (TsinghuaNet.Uno.Helpers.NetSettings)base.Settings;
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
            Settings = new TsinghuaNet.Uno.Helpers.NetSettings();
            // 上一次登录的用户名
            var un = Settings.StoredUsername;
            // 设置为当前用户名并获取密码
            Credential.Username = un;
            Credential.Password = CredentialHelper.GetCredential(un);
        }

        public override void SaveSettings()
        {
            Settings.SaveSettings();
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

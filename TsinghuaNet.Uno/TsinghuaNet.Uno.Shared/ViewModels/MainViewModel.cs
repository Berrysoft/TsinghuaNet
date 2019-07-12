using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;
using TsinghuaNet.Models;
using TsinghuaNet.Uno.Helpers;
using TsinghuaNet.Uno.Views;
using TsinghuaNet.Uno.Models;
using TsinghuaNet.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#if WINDOWS_UWP
using TsinghuaNet.Uno.UWP.Background;
#endif

namespace TsinghuaNet.Uno.ViewModels
{
    class MainViewModel : MainViewModelBase
    {
        private readonly DispatcherTimer mainTimer = new DispatcherTimer();

        [DoNotNotify]
        public new NetUnoSettings Settings
        {
            get => (NetUnoSettings)base.Settings;
            set => base.Settings = value;
        }

        public MainViewModel() : base()
        {
            ChangeStateCommand = new ChangeEnumCommand<NetState>(s => Credential.State = s);
            mainTimer.Interval = TimeSpan.FromSeconds(1);
            mainTimer.Tick += MainTimerTick;
        }

        public override Task LoadSettingsAsync()
        {
            Settings = new NetUnoSettings();
#if WINDOWS_UWP
            Settings.PropertyChanged += OnSettingsPropertyChanged;
#endif
            Settings.LoadSettings();
            // 上一次登录的用户名
            var un = Settings.StoredUsername;
            // 设置为当前用户名并获取密码
            Credential.Username = un;
            Credential.Password = CredentialHelper.GetCredential(un);

            Status = new InternetStatus();
            return Task.CompletedTask;
        }

#if WINDOWS_UWP
        private async void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "BackgroundAutoLogin":
                    if (await BackgroundHelper.RequestAccessAsync())
                        BackgroundHelper.RegisterLogin(Settings.BackgroundAutoLogin);
                    break;
                case "BackgroundLiveTile":
                    if (await BackgroundHelper.RequestAccessAsync())
                        BackgroundHelper.RegisterLiveTile(Settings.BackgroundLiveTile);
                    break;
            }
        }
#endif

        public override Task SaveSettingsAsync()
        {
            Settings.StoredUsername = Credential.Username;
            Settings.SaveSettings();
            return Task.CompletedTask;
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

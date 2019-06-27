using System;
using System.Threading.Tasks;
using TsinghuaNet.Models;
using TsinghuaNet.Uno.Contents;
using TsinghuaNet.Uno.Helpers;
using TsinghuaNet.Uno.UWP.Background;
using Windows.UI.Xaml;

namespace TsinghuaNet.Uno.ViewModels
{
    class MainViewModel : NetViewModel
    {
        private readonly DispatcherTimer mainTimer = new DispatcherTimer();

        public MainViewModel() : base()
        {
            Status = new InternetStatus();
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
            // 获取用户设置的主题
            Theme = SettingsHelper.Theme;
            ContentType = SettingsHelper.ContentType;
            // 自动登录
            AutoLogin = SettingsHelper.AutoLogin;
            // 后台任务
            BackgroundAutoLogin = SettingsHelper.BackgroundAutoLogin;
            BackgroundLiveTile = SettingsHelper.BackgroundLiveTile;
            // 流量限制
            if (SettingsHelper.FluxLimit != null)
            {
                FluxLimit = SettingsHelper.FluxLimit.Value;
                EnableFluxLimit = true;
            }
        }

        public override void SaveSettings()
        {
            SettingsHelper.StoredUsername = Credential.Username;
            SettingsHelper.AutoLogin = AutoLogin;
            SettingsHelper.Theme = Theme;
            SettingsHelper.ContentType = ContentType;
            SettingsHelper.FluxLimit = EnableFluxLimit ? (ByteSize?)FluxLimit : null;
        }


        private UIElement userContent;
        public UIElement UserContent
        {
            get => userContent;
            set => SetProperty(ref userContent, value);
        }

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
            if (UserContent is IUserContent content)
            {
                content.User = OnlineUser;
                content.BeginAnimation();
            }
            return res;
        }

        private void MainTimerTick(object sender, object e)
        {
            IUserContent content = (IUserContent)UserContent;
            if (!content.AddOneSecond())
                mainTimer.Stop();
        }

        protected override void OnIsBusyChanged()
        {
            base.OnIsBusyChanged();
            IUserContent content = (IUserContent)UserContent;
            if (content != null)
                content.IsProgressActive = IsBusy;
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

        private ElementTheme theme;
        public ElementTheme Theme
        {
            get => theme;
            set => SetProperty(ref theme, value);
        }

        private UserContentType contentType;
        public UserContentType ContentType
        {
            get => contentType;
            set => SetProperty(ref contentType, value, onChanged: OnContentTypeChanged);
        }
        private void OnContentTypeChanged()
        {
            IUserContent oldc = (IUserContent)UserContent;
            IUserContent newc = null;
            switch (ContentType)
            {
                case UserContentType.Line:
                    newc = new LineUserContent();
                    break;
                case UserContentType.Ring:
                    newc = new ArcUserContent();
                    break;
                case UserContentType.Water:
                    newc = new WaterUserContent();
                    break;
            }
            if (oldc != null && newc != null)
                newc.User = oldc.User;
            UserContent = (UIElement)newc;
            Refresh();
        }
    }
}

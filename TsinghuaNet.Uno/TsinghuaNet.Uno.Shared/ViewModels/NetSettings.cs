using System;
using TsinghuaNet.Uno.Helpers;

#if WINDOWS_UWP
using TsinghuaNet.Uno.UWP.Background;
#endif

namespace TsinghuaNet.Uno.ViewModels
{
    public class NetSettings : TsinghuaNet.Models.NetSettings
    {
#if WINDOWS_UWP
        private bool backgroundAutoLogin;
        public bool BackgroundAutoLogin
        {
            get => backgroundAutoLogin;
            set => SetProperty(ref backgroundAutoLogin, value, onChanged: OnBackgroundAutoLoginChanged);
        }
        private async void OnBackgroundAutoLoginChanged()
        {
            SettingsHelper.BackgroundAutoLogin = BackgroundAutoLogin;
            if (await BackgroundHelper.RequestAccessAsync())
                BackgroundHelper.RegisterLogin(BackgroundAutoLogin);
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
            if (await BackgroundHelper.RequestAccessAsync())
                BackgroundHelper.RegisterLiveTile(BackgroundLiveTile);
        }

#endif
    }
}

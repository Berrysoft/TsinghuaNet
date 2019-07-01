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
        public bool BackgroundAutoLogin { get; set; }
        private async void OnBackgroundAutoLoginChanged()
        {
            SettingsHelper.BackgroundAutoLogin = BackgroundAutoLogin;
            if (await BackgroundHelper.RequestAccessAsync())
                BackgroundHelper.RegisterLogin(BackgroundAutoLogin);
        }

        public bool BackgroundLiveTile { get; set; }

        private async void OnBackgroundLiveTileChanged()
        {
            SettingsHelper.BackgroundLiveTile = BackgroundLiveTile;
            if (await BackgroundHelper.RequestAccessAsync())
                BackgroundHelper.RegisterLiveTile(BackgroundLiveTile);
        }
#endif
    }
}

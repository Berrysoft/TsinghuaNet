using System.ComponentModel;
using TsinghuaNet.Models;
using Xamarin.Essentials;

namespace TsinghuaNet.XF.Models
{
    public class NetXFSettings : INetSettings
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public bool AutoLogin { get; set; }
        public bool EnableFluxLimit { get; set; }
        public ByteSize FluxLimit { get; set; }
        public bool BackgroundAutoLogin { get; set; }
        public bool BackgroundLiveTile { get; set; }
        public bool UseProxy { get; set; }

        private const string AutoLoginKey = "AutoLogin";
        private const string BackgroundAutoLoginKey = "BackgroundAutoLogin";
        private const string BackgroundLiveTileKey = "BackgroundLiveTile";
        private const string EnableFluxLimitKey = "EnableFluxLimit";
        private const string FluxLimitKey = "FluxLimit";
        private const string UseProxyKey = "UseProxy";

        public void LoadSettings()
        {
            AutoLogin = Preferences.Get(AutoLoginKey, true);
            BackgroundAutoLogin = Preferences.Get(BackgroundAutoLoginKey, true);
            BackgroundLiveTile = Preferences.Get(BackgroundLiveTileKey, true);
            EnableFluxLimit = Preferences.Get(EnableFluxLimitKey, false);
            FluxLimit = ByteSize.FromGigaBytes(Preferences.Get(FluxLimitKey, 20.0));
            UseProxy = Preferences.Get(UseProxyKey, false);
        }

        public void SaveSettings()
        {
            Preferences.Set(AutoLoginKey, AutoLogin);
            Preferences.Set(BackgroundAutoLoginKey, BackgroundAutoLogin);
            Preferences.Set(BackgroundLiveTileKey, BackgroundLiveTile);
            Preferences.Set(EnableFluxLimitKey, EnableFluxLimit);
            Preferences.Set(FluxLimitKey, FluxLimit.GigaBytes);
            Preferences.Set(UseProxyKey, UseProxy);
        }
    }
}

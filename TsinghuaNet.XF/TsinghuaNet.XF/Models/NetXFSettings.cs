using System.ComponentModel;
using TsinghuaNet.Models;
using Xamarin.Essentials;

namespace TsinghuaNet.XF.Models
{
    public class NetXFSettings : INetSettings
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool AutoLogin { get; set; }
        public bool EnableFluxLimit { get; set; }
        public ByteSize FluxLimit { get; set; }
        public bool BackgroundAutoLogin { get; set; }
        public bool BackgroundLiveTile { get; set; }

        private const string AutoLoginKey = "AutoLogin";
        private const string BackgroundAutoLoginKey = "BackgroundAutoLogin";
        private const string BackgroundLiveTileKey = "BackgroundLiveTile";
        private const string EnableFluxLimitKey = "EnableFluxLimit";
        private const string FluxLimitKey = "FluxLimit";

        public void LoadSettings()
        {
            AutoLogin = Preferences.Get(AutoLoginKey, true);
            BackgroundAutoLogin = Preferences.Get(BackgroundAutoLoginKey, true);
            BackgroundLiveTile = Preferences.Get(BackgroundLiveTileKey, true);
            EnableFluxLimit = Preferences.Get(EnableFluxLimitKey, false);
            FluxLimit = ByteSize.FromGigaBytes(Preferences.Get(FluxLimitKey, 20.0));
        }

        public void SaveSettings()
        {
            Preferences.Set(AutoLoginKey, AutoLogin);
            Preferences.Set(BackgroundAutoLoginKey, BackgroundAutoLogin);
            Preferences.Set(BackgroundLiveTileKey, BackgroundLiveTile);
            Preferences.Set(EnableFluxLimitKey, EnableFluxLimit);
            Preferences.Set(FluxLimitKey, FluxLimit.GigaBytes);
        }
    }
}

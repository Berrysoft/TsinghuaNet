using System.Linq;
using TsinghuaNet.Models;
using Xamarin.Essentials;

namespace TsinghuaNet.XF.Models
{
    public class NetXFSettings : NetSettingsBase
    {
        public override bool AutoLogin { get; set; }
        public override bool EnableFluxLimit { get; set; }
        public override ByteSize FluxLimit { get; set; }
        public bool BackgroundAutoLogin { get; set; }
        public bool BackgroundLiveTile { get; set; }
        public bool UseProxy { get; set; }

        private const string AutoLoginKey = "AutoLogin";
        private const string BackgroundAutoLoginKey = "BackgroundAutoLogin";
        private const string BackgroundLiveTileKey = "BackgroundLiveTile";
        private const string EnableFluxLimitKey = "EnableFluxLimit";
        private const string FluxLimitKey = "FluxLimit";
        private const string UseProxyKey = "UseProxy";
        private const string AcIdsKey = "AcIds";

        public void LoadSettings()
        {
            AutoLogin = Preferences.Get(AutoLoginKey, true);
            BackgroundAutoLogin = Preferences.Get(BackgroundAutoLoginKey, true);
            BackgroundLiveTile = Preferences.Get(BackgroundLiveTileKey, true);
            EnableFluxLimit = Preferences.Get(EnableFluxLimitKey, false);
            FluxLimit = ByteSize.FromGigaBytes(Preferences.Get(FluxLimitKey, 20.0));
            UseProxy = Preferences.Get(UseProxyKey, false);
            AcIds = Preferences.Get(AcIdsKey, string.Join(
#if NETSTANDARD2_0
                ";"
#else
                ';'
#endif
                , PredefinedAcIds)).Split(';').Select(s => int.Parse(s)).ToList();
        }

        public void SaveSettings()
        {
            Preferences.Set(AutoLoginKey, AutoLogin);
            Preferences.Set(BackgroundAutoLoginKey, BackgroundAutoLogin);
            Preferences.Set(BackgroundLiveTileKey, BackgroundLiveTile);
            Preferences.Set(EnableFluxLimitKey, EnableFluxLimit);
            Preferences.Set(FluxLimitKey, FluxLimit.GigaBytes);
            Preferences.Set(UseProxyKey, UseProxy);
            Preferences.Set(AcIdsKey, string.Join(
#if NETSTANDARD2_0
                ";"
#else
                ';'
#endif
                , AcIds));
        }
    }
}

using System;
using Windows.Foundation.Collections;
using Windows.Storage;

#if WINDOWS_UWP
using TsinghuaNet.Uno.UWP.Background;
#endif

namespace TsinghuaNet.Uno.Helpers
{
    class NetSettings : TsinghuaNet.Models.NetSettings
    {
        public string StoredUsername { get; set; }

#if WINDOWS_UWP
        public bool BackgroundAutoLogin { get; set; }
        private async void OnBackgroundAutoLoginChanged()
        {
            if (await BackgroundHelper.RequestAccessAsync())
                BackgroundHelper.RegisterLogin(BackgroundAutoLogin);
        }

        public bool BackgroundLiveTile { get; set; }

        private async void OnBackgroundLiveTileChanged()
        {
            if (await BackgroundHelper.RequestAccessAsync())
                BackgroundHelper.RegisterLiveTile(BackgroundLiveTile);
        }
#endif

        private IPropertySet values;

        private T GetValue<T>(string key, T def = default)
        {
            if (values.ContainsKey(key))
                return (T)values[key];
            else
                return def;
        }

        private void SetValue<T>(string key, T value)
        {
            if (values.ContainsKey(key))
                values[key] = value;
            else
                values.Add(key, value);
        }

        private const string StoredUsernameKey = "Username";
        private const string AutoLoginKey = "AutoLogin";
#if WINDOWS_UWP
        private const string BackgroundAutoLoginKey = "BackgroundAutoLogin";
        private const string BackgroundLiveTileKey = "BackgroundLiveTile";
#endif
        private const string EnableFluxLimitKey = "EnableFluxLimit";
        private const string FluxLimitKey = "FluxLimit";

        public NetSettings() : base()
        {
            values = ApplicationData.Current.LocalSettings.Values;
            StoredUsername = GetValue(StoredUsernameKey, string.Empty);
            AutoLogin = GetValue(AutoLoginKey, true);
#if WINDOWS_UWP
            BackgroundAutoLogin = GetValue(BackgroundAutoLoginKey, true);
            BackgroundLiveTile = GetValue(BackgroundLiveTileKey, true);
#endif
            EnableFluxLimit = GetValue<bool>(EnableFluxLimitKey);
            var limit = GetValue<double>(FluxLimitKey);
            FluxLimit = ByteSize.FromGigaBytes(limit);
        }

        public void SaveSettings()
        {
            SetValue(StoredUsernameKey, StoredUsername);
            SetValue(AutoLoginKey, AutoLogin);
#if WINDOWS_UWP
            SetValue(BackgroundAutoLoginKey, BackgroundAutoLogin);
            SetValue(BackgroundLiveTileKey, BackgroundLiveTile);
#endif
            SetValue(EnableFluxLimitKey, EnableFluxLimit);
            SetValue(FluxLimitKey, FluxLimit.GigaBytes);
        }
    }
}

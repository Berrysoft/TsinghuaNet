using Windows.Foundation.Collections;
using Windows.Storage;
using TsinghuaNet.Models;

namespace TsinghuaNet.Uno.Helpers
{
    public class NetUnoSettings : NetSettings
    {
        public string StoredUsername { get; set; }

#if WINDOWS_UWP
        public bool BackgroundAutoLogin { get; set; }

        public bool BackgroundLiveTile { get; set; }
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

        public void LoadSettings()
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

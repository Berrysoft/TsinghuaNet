using Windows.Foundation.Collections;
using Windows.Storage;

namespace TsinghuaNet.Uno.Helpers
{
    static class SettingsHelper
    {
        private static IPropertySet values;

        private static T GetValue<T>(string key, T def = default)
        {
            if (values.ContainsKey(key))
                return (T)values[key];
            else
                return def;
        }

        private static void SetValue<T>(string key, T value)
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

        static SettingsHelper()
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

        public static void SaveSettings()
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

        public static string StoredUsername { get; set; }

        public static bool AutoLogin { get; set; }


#if WINDOWS_UWP
        public static bool BackgroundAutoLogin { get; set; }

        public static bool BackgroundLiveTile { get; set; }
#endif

        public static bool EnableFluxLimit { get; set; }

        public static ByteSize FluxLimit { get; set; }
    }
}

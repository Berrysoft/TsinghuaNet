using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;

namespace TsinghuaNet.Uno.Helpers
{
    public enum UserContentType
    {
        Line,
        Ring,
        Water
    }

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
        private const string BackgroundAutoLoginKey = "BackgroundAutoLogin";
        private const string BackgroundLiveTileKey = "BackgroundLiveTile";
        private const string ThemeKey = "Theme";
        private const string ContentTypeKey = "UserContentType";
        private const string FluxLimitKey = "FluxLimit";

        static SettingsHelper()
        {
            values = ApplicationData.Current.LocalSettings.Values;
            StoredUsername = GetValue<string>(StoredUsernameKey);
            AutoLogin = GetValue(AutoLoginKey, true);
            BackgroundAutoLogin = GetValue(BackgroundAutoLoginKey, true);
            BackgroundLiveTile = GetValue(BackgroundLiveTileKey, true);
            Theme = (ElementTheme)GetValue(ThemeKey, (int)ElementTheme.Default);
            ContentType = (UserContentType)GetValue(ContentTypeKey, (int)UserContentType.Ring);
            var limit = GetValue<long?>(FluxLimitKey);
            FluxLimit = limit == null ? null : (ByteSize?)ByteSize.FromGigaBytes(limit.Value);
        }

        public static void SaveSettings()
        {
            SetValue(StoredUsernameKey, StoredUsername);
            SetValue(AutoLoginKey, AutoLogin);
            SetValue(BackgroundAutoLoginKey, BackgroundAutoLogin);
            SetValue(BackgroundLiveTileKey, BackgroundLiveTile);
            SetValue(ThemeKey, (int)Theme);
            SetValue(ContentTypeKey, (int)ContentType);
            SetValue(FluxLimitKey, FluxLimit == null ? null : (long?)FluxLimit.Value.GigaBytes);
        }

        public static string StoredUsername { get; set; }

        public static bool AutoLogin { get; set; }

        public static bool BackgroundAutoLogin { get; set; }

        public static bool BackgroundLiveTile { get; set; }

        public static ElementTheme Theme { get; set; }

        public static UserContentType ContentType { get; set; }

        public static ByteSize? FluxLimit { get; set; }
    }
}

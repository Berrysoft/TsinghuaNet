using System.Collections.Generic;
using System.ComponentModel;
using TsinghuaNet.Models;
using Xamarin.Forms;

namespace TsinghuaNet.XF.Models
{
    class NetXFSettings : INetSettings
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool AutoLogin { get; set; }
        public bool EnableFluxLimit { get; set; }
        public ByteSize FluxLimit { get; set; }
        public bool BackgroundAutoLogin { get; set; }
        public bool BackgroundLiveTile { get; set; }

        private readonly IDictionary<string, object> values;

        public NetXFSettings()
        {
            values = Application.Current.Properties;
        }

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

        private const string AutoLoginKey = "AutoLogin";
        private const string BackgroundAutoLoginKey = "BackgroundAutoLogin";
        private const string BackgroundLiveTileKey = "BackgroundLiveTile";
        private const string EnableFluxLimitKey = "EnableFluxLimit";
        private const string FluxLimitKey = "FluxLimit";

        public void LoadSettings()
        {
            AutoLogin = GetValue(AutoLoginKey, true);
            BackgroundAutoLogin = GetValue(BackgroundAutoLoginKey, true);
            BackgroundLiveTile = GetValue(BackgroundLiveTileKey, true);
            EnableFluxLimit = GetValue<bool>(EnableFluxLimitKey);
            FluxLimit = ByteSize.FromGigaBytes(GetValue<double>(FluxLimitKey));
        }

        public void SaveSettings()
        {
            SetValue(AutoLoginKey, AutoLogin);
            SetValue(BackgroundAutoLoginKey, BackgroundAutoLogin);
            SetValue(BackgroundLiveTileKey, BackgroundLiveTile);
            SetValue(EnableFluxLimitKey, EnableFluxLimit);
            SetValue(FluxLimitKey, FluxLimit.GigaBytes);
        }
    }
}

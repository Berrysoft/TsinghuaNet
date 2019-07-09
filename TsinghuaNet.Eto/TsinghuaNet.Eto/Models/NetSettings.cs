using System.Text.Json.Serialization;
using TsinghuaNet.Helpers;
using tm = TsinghuaNet.Models;

namespace TsinghuaNet.Eto.Models
{
    public class NetSettings : tm.NetSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseTimer { get; set; }

        [JsonIgnore]
        public bool DeleteSettingsOnExit { get; set; }
    }

    internal static class SettingsHelper
    {
        private const string SettingsFilename = "settings.json";
        private const string ProjectName = "TsinghuaNet.Eto";

        public static readonly SettingsFileHelper Helper = new SettingsFileHelper(ProjectName, SettingsFilename);
    }
}

using System.Text.Json.Serialization;
using TsinghuaNet.Models;

namespace TsinghuaNet.CLI
{
    class NetCLISettings : NetSettingsBase
    {
        [JsonIgnore]
        public override bool AutoLogin { get; set; }
        [JsonIgnore]
        public override bool EnableFluxLimit { get; set; }
        [JsonIgnore]
        public override ByteSize FluxLimit { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

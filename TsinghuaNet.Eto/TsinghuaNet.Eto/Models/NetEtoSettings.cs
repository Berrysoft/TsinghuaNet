using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TsinghuaNet.Models;

namespace TsinghuaNet.Eto.Models
{
    public class NetEtoSettings : NetSettingsBase
    {
        public override bool AutoLogin { get; set; }
        public override bool EnableFluxLimit { get; set; }
        [JsonConverter(typeof(JsonConverterByteSize))]
        public override ByteSize FluxLimit { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseTimer { get; set; }
        public bool UseProxy { get; set; }

        [JsonIgnore]
        public bool DeleteSettingsOnExit { get; set; }
    }

    internal static class SettingsHelper
    {
        private const string SettingsFilename = "settings.json";
        private const string ProjectName = "TsinghuaNet.Eto";

        public static readonly SettingsFileHelper Helper = new SettingsFileHelper(ProjectName, SettingsFilename);
    }

    public class JsonConverterByteSize : JsonConverter<ByteSize>
    {
        public override ByteSize Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new ByteSize(reader.GetInt64());
        }

        public override void Write(Utf8JsonWriter writer, ByteSize value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.Bytes);
        }
    }
}

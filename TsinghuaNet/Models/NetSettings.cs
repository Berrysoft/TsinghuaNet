using System;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TsinghuaNet.Models
{
    public class NetSettings : INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public bool AutoLogin { get; set; }

        public bool EnableFluxLimit { get; set; }

        [JsonConverter(typeof(JsonConverterByteSize))]
        public ByteSize FluxLimit { get; set; }
    }

    internal class JsonConverterByteSize : JsonConverter<ByteSize>
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

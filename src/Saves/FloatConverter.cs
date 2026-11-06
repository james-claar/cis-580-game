
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace cis_580_game;

public class FloatConverter : JsonConverter<float>
{
    public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetSingle();
    }

    public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}

using Poker.PhysicalObjects.Cards;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Poker.Serialisation;

public class CardJsonConverter : JsonConverter<Card>
{
    public override Card Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }

        string cardString = reader.GetString();
        return Card.Deserialize(cardString);
    }

    public override void Write(Utf8JsonWriter writer, Card value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}


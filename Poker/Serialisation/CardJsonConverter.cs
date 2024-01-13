using Poker.PhysicalObjects.Cards;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Poker.Serialisation;

public class CardJsonConverter : JsonConverter<Card>
{
    public override Card Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        reader.Read();
        if (reader.TokenType != JsonTokenType.Number)
        {
            throw new JsonException();
        }
        int rankValue = reader.GetUInt16();

        reader.Read();
        if (reader.TokenType != JsonTokenType.Number)
        {
            throw new JsonException();
        }
        int suitValue = reader.GetUInt16();

        reader.Read(); // Read the end of the array token

        var cardRank = (CardRank)rankValue;
        var cardSuit = (CardSuit)suitValue;

        return Card.GetCard(cardRank, cardSuit);
    }

    public override void Write(Utf8JsonWriter writer, Card value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue((ushort)value.CardRank);
        writer.WriteNumberValue((ushort)value.Suit);
        writer.WriteEndArray();
    }

}


using Poker.PhysicalObjects.Cards;
using System.Text.Json;
using System.Text.Json.Serialization;

public class CardJsonConverter : JsonConverter<Card>
{
    public override Card Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        CardRank rank = default;
        CardSuit suit = default;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return Card.GetCard(rank, suit);
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString();
                reader.Read();
                switch (propertyName)
                {
                    case nameof(Card.CardRank):
                        rank = JsonSerializer.Deserialize<CardRank>(ref reader, options);
                        break;
                    case nameof(Card.Suit):
                        suit = JsonSerializer.Deserialize<CardSuit>(ref reader, options);
                        break;
                }
            }
        }

        throw new JsonException("Error reading JSON.");
    }

    public override void Write(Utf8JsonWriter writer, Card value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(nameof(Card.CardRank));
        JsonSerializer.Serialize(writer, value.CardRank, options);
        writer.WritePropertyName(nameof(Card.Suit));
        JsonSerializer.Serialize(writer, value.Suit, options);
        writer.WriteEndObject();
    }
}

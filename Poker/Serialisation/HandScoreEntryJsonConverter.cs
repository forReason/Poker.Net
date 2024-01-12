using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Poker.PhysicalObjects.Cards;

namespace Poker.Secrialisation
{
    public class HandScoreEntryJsonConverter : JsonConverter<HandScoreEntry>
    {
        public override HandScoreEntry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            Card[] cards = Array.Empty<Card>();
            float winRate = 0;
            uint evaluatedRounds = 0;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return new HandScoreEntry(cards) // Assuming this constructor sets the Cards field
                    {
                        // Set other fields directly since they are public
                        WinRate = winRate,
                        EvaluatedRounds = evaluatedRounds
                    };
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case nameof(HandScoreEntry.Cards):
                            cards = JsonSerializer.Deserialize<Card[]>(ref reader, options);
                            break;
                        case nameof(HandScoreEntry.WinRate):
                            winRate = reader.GetSingle();
                            break;
                        case nameof(HandScoreEntry.EvaluatedRounds):
                            evaluatedRounds = reader.GetUInt32();
                            break;
                    }
                }
            }

            throw new JsonException("Error reading JSON.");
        }

        public override void Write(Utf8JsonWriter writer, HandScoreEntry value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            
            writer.WritePropertyName(nameof(HandScoreEntry.Cards));
            JsonSerializer.Serialize(writer, value.Cards, options);
            
            writer.WritePropertyName(nameof(HandScoreEntry.WinRate));
            writer.WriteNumberValue(value.WinRate);

            writer.WritePropertyName(nameof(HandScoreEntry.EvaluatedRounds));
            writer.WriteNumberValue(value.EvaluatedRounds);
            
            writer.WriteEndObject();
        }
    }
}

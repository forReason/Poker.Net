using QuickStatistics.Net.Average_NS;

namespace Poker.Net.PhysicalObjects.HandScores;

public class HandScoreDictionary
{
    public readonly Dictionary<byte[], (float WinRate, uint Iterations)> Dictionary;

    public HandScoreDictionary()
    {
        Dictionary = new Dictionary<byte[], (float WinRate, uint Iterations)>(new HandScoreComparer());
    }

    public void Add(byte[] serializedEntry)
    {
        (byte[] cards, float winrate, uint iterations) entry = HandScoreEntry.ExtractRawDataFromSerialized(serializedEntry);
        Dictionary[entry.cards] = (entry.winrate, entry.iterations);
    }

    public void Add(HandScoreEntry entry)
    {
        Dictionary[entry.SerializeCards()] = (entry.WinRate, entry.EvaluatedRounds);
    }

    public bool AddIteration(byte[] key, bool win)
    {
        float valueToAdd = win ? 100F : 0F; // 100 for win, 0 for loss
        if (TryGetValue(key, out (float WinRate, uint Iterations) info))
        {
            // Update the win rate and iteration count
            bool success = ProgressingAverage_Nano.AddValue(ref info.WinRate, ref info.Iterations, valueToAdd);
            Dictionary[key] = info; // Update the dictionary entry
            return success;
        }
        else
        {
            // Add new entry with initial values
            Dictionary[key] = (valueToAdd, 1);
            return true;
        }
    }


    public bool TryGetValue(byte[] key, out (float WinRate, uint Iterations) value)
    {
        return Dictionary.TryGetValue(key, out value);
    }
    public void SaveToFile(string filePath)
    {
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            foreach (var kvp in Dictionary)
            {
                // Write the key (card bytes)
                fileStream.Write(kvp.Key, 0, kvp.Key.Length);

                // Write the win rate and iterations
                var winRateBytes = BitConverter.GetBytes(kvp.Value.WinRate);
                var iterationsBytes = BitConverter.GetBytes(kvp.Value.Iterations);

                fileStream.Write(winRateBytes, 0, winRateBytes.Length);
                fileStream.Write(iterationsBytes, 0, iterationsBytes.Length);
            }
        }
    }
    public void LoadFromFile(string filePath)
    {
        Dictionary.Clear();

        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            byte[] cardKey = new byte[6];
            byte[] winRateBytes = new byte[4];
            byte[] iterationsBytes = new byte[4];

            while (fileStream.Read(cardKey, 0, cardKey.Length) == cardKey.Length)
            {
                if (fileStream.Read(winRateBytes, 0, winRateBytes.Length) != winRateBytes.Length ||
                    fileStream.Read(iterationsBytes, 0, iterationsBytes.Length) != iterationsBytes.Length)
                {
                    throw new InvalidDataException("File format is invalid or corrupted.");
                }

                float winRate = BitConverter.ToSingle(winRateBytes, 0);
                uint iterations = BitConverter.ToUInt32(iterationsBytes, 0);

                Dictionary.Add(cardKey.ToArray(), (winRate, iterations));
            }
        }
    }

    public void Clear()
    {
        Dictionary.Clear();
    }
}

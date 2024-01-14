using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Poker.PhysicalObjects.Cards;
using Poker.PhysicalObjects.HandScores;

namespace Poker.Tests.PhysicalObjects.Cards;
public class HandScoreEntryTests
{
    [Fact]
    public void SerializeAndDeserialize_ShouldReturnEquivalentHandScoreEntry()
    {
        // Arrange
        var cards = new Card[] {
            Card.GetCard(CardRank.Two, CardSuit.Clubs),
            Card.GetCard(CardRank.Three, CardSuit.Clubs),
            Card.GetCard(CardRank.Four, CardSuit.Clubs),
            Card.GetCard(CardRank.Five, CardSuit.Clubs),
            Card.GetCard(CardRank.Six, CardSuit.Clubs),
            Card.GetCard(CardRank.Seven, CardSuit.Clubs),
            Card.GetCard(CardRank.Eight, CardSuit.Clubs)
        };
        var handScoreEntry = new HandScoreEntry(cards);
        handScoreEntry.WinRate = 6.6F;
        handScoreEntry.EvaluatedRounds = 5;

        // Act
        byte[] serialized = handScoreEntry.Serialize();
        var deserializedHandScoreEntry = HandScoreEntry.Deserialize(serialized);

        // Assert
        Assert.Equal(handScoreEntry, deserializedHandScoreEntry);
    }
    [Fact]
    public void SerializeAndDeserializeLessCards_ShouldReturnEquivalentHandScoreEntry()
    {
        // Arrange
        var cards = new Card[] {
            Card.GetCard(CardRank.Two, CardSuit.Clubs),
            Card.GetCard(CardRank.Three, CardSuit.Clubs),
            Card.GetCard(CardRank.Four, CardSuit.Clubs),
            Card.GetCard(CardRank.Five, CardSuit.Clubs),
        };
        var handScoreEntry = new HandScoreEntry(cards);
        handScoreEntry.WinRate = 6.6F;
        handScoreEntry.EvaluatedRounds = 5;

        // Act
        byte[] serialized = handScoreEntry.Serialize();
        var deserializedHandScoreEntry = HandScoreEntry.Deserialize(serialized);

        // Assert
        Assert.Equal(handScoreEntry, deserializedHandScoreEntry);
    }
    [Fact]
    public void GetHashCode_ShouldReturnSameHashCodeForSameCards()
    {
        // Arrange
        var cards1 = new Card[] {
            Card.GetCard(CardRank.Two, CardSuit.Clubs),
            Card.GetCard(CardRank.Three, CardSuit.Clubs),
            Card.GetCard(CardRank.Four, CardSuit.Clubs),
            Card.GetCard(CardRank.Five, CardSuit.Clubs),
            Card.GetCard(CardRank.Six, CardSuit.Clubs),
            Card.GetCard(CardRank.Seven, CardSuit.Clubs),
            Card.GetCard(CardRank.Eight, CardSuit.Clubs)
        };
        var handScoreEntry1 = new HandScoreEntry(cards1);
        var handScoreEntry2 = new HandScoreEntry(cards1); // Same card setup

        // Act
        int hashCode1 = handScoreEntry1.GetHashCode();
        int hashCode2 = handScoreEntry2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void Equals_ShouldReturnTrueForIdenticalHandScoreEntries()
    {
        // Arrange
        var cards = new Card[] {
            Card.GetCard(CardRank.Two, CardSuit.Clubs),
            Card.GetCard(CardRank.Three, CardSuit.Clubs),
            Card.GetCard(CardRank.Four, CardSuit.Clubs),
            Card.GetCard(CardRank.Five, CardSuit.Clubs),
            Card.GetCard(CardRank.Six, CardSuit.Clubs),
            Card.GetCard(CardRank.Seven, CardSuit.Clubs),
            Card.GetCard(CardRank.Eight, CardSuit.Clubs)
        };
        var handScoreEntry1 = new HandScoreEntry(cards);
        var handScoreEntry2 = new HandScoreEntry(cards); // Same card setup

        // Act & Assert
        Assert.True(handScoreEntry1.Equals(handScoreEntry2));
    }

    [Fact]
    public void Equals_ShouldReturnFalseForDifferentHandScoreEntries()
    {
        // Arrange
        var cards1 = new Card[] { 
            Card.GetCard(CardRank.Two, CardSuit.Clubs),
            Card.GetCard(CardRank.Three, CardSuit.Clubs), 
            Card.GetCard(CardRank.Four, CardSuit.Clubs), 
            Card.GetCard(CardRank.Five, CardSuit.Clubs), 
            Card.GetCard(CardRank.Six, CardSuit.Clubs), 
            Card.GetCard(CardRank.Seven, CardSuit.Clubs), 
            Card.GetCard(CardRank.Eight, CardSuit.Clubs)
        };
        var cards2 = new Card[] {
            Card.GetCard(CardRank.Two, CardSuit.Clubs),
            Card.GetCard(CardRank.Three, CardSuit.Clubs),
            Card.GetCard(CardRank.Four, CardSuit.Clubs),
            Card.GetCard(CardRank.Five, CardSuit.Clubs),
            Card.GetCard(CardRank.Six, CardSuit.Clubs),
            Card.GetCard(CardRank.Seven, CardSuit.Clubs),
            Card.GetCard(CardRank.Nine, CardSuit.Clubs)
        };
        var handScoreEntry1 = new HandScoreEntry(cards1);
        var handScoreEntry2 = new HandScoreEntry(cards2); // Different card setup

        // Act & Assert
        Assert.False(handScoreEntry1.Equals(handScoreEntry2));
    }

    // Add more tests as needed...
}

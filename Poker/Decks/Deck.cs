using System.Security.Cryptography;
using Poker.Cards;

namespace Poker.Decks;

/// <summary>
/// Represents the Raw Deck with all 52 Cards. Primarily used for shuffling and drawing
/// </summary>
public class Deck
{
    /// <summary>
    /// Initializes the Deck with the 42 Poker Cards
    /// </summary>
    public Deck()
    {
        GenerateDeck();
    }

    /// <summary>
    /// The count of cards left in the Deck which are available to be drawn
    /// </summary>
    public int _CardCount { get; private set; } = 0;

    /// <summary>
    /// All 52 Cards in a Deck
    /// </summary>
    private Cards.Card[] _ShuffledCards = new Cards.Card[52];

    /// <summary>
    /// Draws a new Card from the Shuffled Deck
    /// </summary>
    /// <returns>The Drawn Card</returns>
    public Cards.Card DrawCard()
    {
        if (_CardCount <= 0)
            throw new InvalidOperationException("You can not draw more cards than the Deck has in Total! Please reshuffle!");
        Cards.Card selectedCard = _ShuffledCards[_CardCount-1];
        _CardCount--;
        return selectedCard;
    }
    
    /// <summary>
    /// Shuffles all the cards in the Deck
    /// </summary>
    /// <summary>
    /// Shuffles all the cards in the Deck
    /// </summary>
    public void ShuffleCards()
    {
        // shuffle randomly
        WashDeck();
        // riffle shuffle
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        byte[] randomNumber = new byte[4];
        rng.GetBytes(randomNumber);
        int shuffleCount = BitConverter.ToInt32(randomNumber, 0) % 5 + 3; // Generates a number between 3 and 7

        for (int i = 0; i < shuffleCount; i++)
        {
            RiffleShuffle();
        }

        rng.Dispose();
        // strip shuffle
        StripShuffle();
        // finalize
        Cut();
    }

    /// <summary>
    /// shuffles the deck randomly
    /// </summary>
    public void WashDeck()
    {
        byte[] randomBytes = new byte[4];
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
    
        for (int i = 0; i < _ShuffledCards.Length; i++)
        {
            rng.GetBytes(randomBytes);
            int randomIndex = BitConverter.ToInt32(randomBytes, 0) % _ShuffledCards.Length;
        
            // Swap the cards
            Cards.Card temp = _ShuffledCards[i];
            _ShuffledCards[i] = _ShuffledCards[randomIndex];
            _ShuffledCards[randomIndex] = temp;
        }

        rng.Dispose();
    }
    
    /// <summary>
    /// splits the deck in half and riffles the cards together randomly
    /// </summary>
    public void RiffleShuffle()
    {
        Cards.Card[] shuffled = new Cards.Card[52];
        int middle = _ShuffledCards.Length / 2;
        int leftIndex = 0, rightIndex = middle;

        for (int i = 0; i < _ShuffledCards.Length; i++)
        {
            byte[] randomNumber = new byte[1];
            RandomNumberGenerator.Fill(randomNumber);
            bool takeFromLeft = randomNumber[0] % 2 == 0;

            shuffled[i] = takeFromLeft ? _ShuffledCards[leftIndex++] : _ShuffledCards[rightIndex++];
            if (leftIndex == middle) leftIndex = 0;
            if (rightIndex == _ShuffledCards.Length) rightIndex = middle;
        }

        _ShuffledCards = shuffled;
    }
    /// <summary>
    /// takes stacks of random amounts of cards and places them under
    /// </summary>
    public void StripShuffle()
    {
        Cards.Card[] shuffled = new Cards.Card[52];
        List<Cards.Card> tempDeck = new List<Cards.Card>(_ShuffledCards);
    
        for (int i = 0; i < shuffled.Length;)
        {
            byte[] randomBytes = new byte[4];
            RandomNumberGenerator.Fill(randomBytes);
            int chunkSize = BitConverter.ToInt32(randomBytes, 0) % 5 + 1; // 1 to 5 cards
            RandomNumberGenerator.Fill(randomBytes);
            int chunkStart = BitConverter.ToInt32(randomBytes, 0) % (tempDeck.Count - chunkSize);

            for (int j = 0; j < chunkSize; j++)
            {
                shuffled[i++] = tempDeck[chunkStart + j];
            }
            tempDeck.RemoveRange(chunkStart, chunkSize);
        }

        _ShuffledCards = shuffled;
    }

    /// <summary>
    /// Cuts the deck at a random position, placing the top cards at the bottom
    /// </summary>
    public void Cut()
    {
        Random rnd = new Random();
        int cutPoint = rnd.Next(1, _ShuffledCards.Length);
        Cards.Card[] cutDeck = new Cards.Card[52];

        Array.Copy(_ShuffledCards, cutPoint, cutDeck, 0, _ShuffledCards.Length - cutPoint);
        Array.Copy(_ShuffledCards, 0, cutDeck, _ShuffledCards.Length - cutPoint, cutPoint);

        _ShuffledCards = cutDeck;
    }


    /// <summary>
    /// Generates the Deck of 52 Cards, one of each rank and suit combination
    /// </summary>
    private void GenerateDeck()
    {
        int i = 0;
        foreach (Rank rank in Enum.GetValuesAsUnderlyingType<Rank>())
        {
            foreach (Suit suit in Enum.GetValuesAsUnderlyingType<Suit>())
            {
                _ShuffledCards[i] = new Cards.Card(rank, suit);
                i++;
            }
        }

        ShuffleCards();
    }
}
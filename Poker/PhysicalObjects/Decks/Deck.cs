using System.Security.Cryptography;
using Poker.PhysicalObjects.Cards;

namespace Poker.PhysicalObjects.Decks;

/// <summary>
/// Represents the Raw Deck with all 52 Cards. Primarily used for shuffling and drawing
/// </summary>
public class Deck
{
    /// <summary>
    /// Initializes the Deck with the 52 Poker Cards
    /// </summary>
    public Deck()
    {
        GenerateDeck();
    }

    /// <summary>
    /// The count of cards left in the Deck which are available to be drawn
    /// </summary>
    public int CardCount { get; private set; } = 52;

    /// <summary>
    /// All 52 Cards in a Deck
    /// </summary>
    private Card[] _shuffledCards = new Card[52];

    /// <summary>
    /// Draws a new Card from the Shuffled Deck
    /// </summary>
    /// <returns>The Drawn Card</returns>
    /// <exception cref="InvalidOperationException">thrown when there are no more cords in the deck and you still try to draw one</exception>
    public Card DrawCard()
    {
        if (CardCount <= 0)
            throw new InvalidOperationException("You can not draw more cards than the Deck has in Total! Please reshuffle!");
        Card selectedCard = _shuffledCards[CardCount-1];
        CardCount--;
        return selectedCard;
    }
    
    /// <summary>
    /// Shuffles all the cards in the Deck by first washing the cards, then riffleshuffling and finally stripshuffling followed by a final cut
    /// </summary>
    /// <remarks>Also resets the states of the deck so that cards can be drawn from again</remarks>
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
    private void WashDeck()
    {
        byte[] randomBytes = new byte[4];
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
    
        for (int i = 0; i < _shuffledCards.Length; i++)
        {
            rng.GetBytes(randomBytes);
            int randomIndex = BitConverter.ToInt32(randomBytes, 0) % _shuffledCards.Length;
        
            // Swap the cards
            (_shuffledCards[i], _shuffledCards[randomIndex]) = (_shuffledCards[randomIndex], _shuffledCards[i]);
        }

        rng.Dispose();
    }

    /// <summary>
    /// splits the deck in half and riffles the cards together randomly
    /// </summary>
    private void RiffleShuffle()
    {
        Card[] shuffled = new Card[52];
        int middle = _shuffledCards.Length / 2;
        int leftIndex = 0, rightIndex = middle;

        for (int i = 0; i < _shuffledCards.Length; i++)
        {
            byte[] randomNumber = new byte[1];
            RandomNumberGenerator.Fill(randomNumber);
            bool takeFromLeft = randomNumber[0] % 2 == 0;

            shuffled[i] = takeFromLeft ? _shuffledCards[leftIndex++] : _shuffledCards[rightIndex++];
            if (leftIndex == middle) leftIndex = 0;
            if (rightIndex == _shuffledCards.Length) rightIndex = middle;
        }

        _shuffledCards = shuffled;
    }
    /// <summary>
    /// takes stacks of random amounts of cards and places them under
    /// </summary>
    private void StripShuffle()
    {
        Card[] shuffled = new Card[52];
        List<Card> tempDeck = [.._shuffledCards];
    
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

        _shuffledCards = shuffled;
    }

    /// <summary>
    /// Cuts the deck at a random position, placing the top cards at the bottom
    /// </summary>
    private void Cut()
    {
        Random rnd = new Random();
        int cutPoint = rnd.Next(1, _shuffledCards.Length);
        Card[] cutDeck = new Card[52];

        Array.Copy(_shuffledCards, cutPoint, cutDeck, 0, _shuffledCards.Length - cutPoint);
        Array.Copy(_shuffledCards, 0, cutDeck, _shuffledCards.Length - cutPoint, cutPoint);

        _shuffledCards = cutDeck;
    }


    /// <summary>
    /// Generates the Deck of 52 Cards, one of each CardRank and suit combination
    /// </summary>
    private void GenerateDeck()
    {
        int i = 0;
        foreach (CardRank CardRank in Enum.GetValuesAsUnderlyingType<CardRank>())
        {
            foreach (CardSuit suit in Enum.GetValuesAsUnderlyingType<CardSuit>())
            {
                _shuffledCards[i] = new Card(CardRank, suit);
                i++;
            }
        }

        ShuffleCards();
    }
}
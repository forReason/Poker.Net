using System.Security.Cryptography;
using Poker.Net.PhysicalObjects.Cards;

namespace Poker.Net.PhysicalObjects.Decks;
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
    /// Draws a Card.GetCard from the Shuffled Deck
    /// </summary>
    /// <returns>The Drawn Card</returns>
    /// <exception cref="InvalidOperationException">thrown when there are no more cords in the deck and you still try to draw one</exception>
    public Card DrawCard()
    {
        return _shuffledCards[--CardCount];
    }

    /// <summary>
    /// Provides a secure snapshot of the current state of the deck.
    /// </summary>
    /// <returns>A new array containing a snapshot of the deck's current state.</returns>
    internal Card[] GetDeckSnapshot()
    {
        Card[] snapshot = new Card[_shuffledCards.Length];
        Array.Copy(_shuffledCards, snapshot, _shuffledCards.Length);
        return snapshot;
    }

    /// <summary>
    /// Shuffles all the cards in the Deck by first washing the cards, then riffle shuffling and finally strip shuffling followed by a final cut
    /// </summary>
    /// <remarks>Also resets the states of the deck so that cards can be drawn from again<br/><br/>
    /// verifies the Deck integrity after Data Generation</remarks>
    /// <exception cref="InvalidOperationException">The Deck sees to be corrupted</exception>
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
    internal void WashDeck()
    {
        byte[] randomBytes = new byte[4];
        RandomNumberGenerator rng = RandomNumberGenerator.Create();

        for (int i = _shuffledCards.Length - 1; i > 0; i--)
        {
            rng.GetBytes(randomBytes);
            uint randomIndex = BitConverter.ToUInt32(randomBytes, 0) % (uint)i;

            // Swap the cards
            (_shuffledCards[i], _shuffledCards[randomIndex]) = (_shuffledCards[randomIndex], _shuffledCards[i]);
        }

        rng.Dispose();
    }



    /// <summary>
    /// splits the deck in half and riffles the cards together randomly
    /// </summary>
    internal void RiffleShuffle()
    {
        Card[] shuffled = new Card[52];
        int middle = _shuffledCards.Length / 2;
        int leftIndex = 0, rightIndex = middle;

        // Generate a batch of random numbers
        byte[] randomNumbers = new byte[_shuffledCards.Length];
        RandomNumberGenerator.Fill(randomNumbers);
        int randomIndex = 0;

        for (int i = 0; i < _shuffledCards.Length; i++)
        {
            bool takeFromLeft = (randomNumbers[randomIndex++] % 2) == 0;
            if (randomIndex >= randomNumbers.Length) randomIndex = 0; // Reset index if needed

            if (takeFromLeft && leftIndex < middle)
            {
                shuffled[i] = _shuffledCards[leftIndex++];
            }
            else if (!takeFromLeft && rightIndex < _shuffledCards.Length)
            {
                shuffled[i] = _shuffledCards[rightIndex++];
            }
            else if (leftIndex < middle) // If rightIndex is exhausted
            {
                shuffled[i] = _shuffledCards[leftIndex++];
            }
            else // If leftIndex is exhausted
            {
                shuffled[i] = _shuffledCards[rightIndex++];
            }
        }

        _shuffledCards = shuffled;
    }


    /// <summary>
    /// takes stacks of random amounts of cards and places them under
    /// </summary>
    internal void StripShuffle(uint shuffleRounds = 7)
    {
        Card[] leftStack = _shuffledCards;
        Card[] rightStack = new Card[leftStack.Length];

        byte[] randomBytes = new byte[4 * shuffleRounds]; // Generate all random bytes at once
        RandomNumberGenerator.Fill(randomBytes);
        int randomIndex = 0;

        for (int i = 0; i < shuffleRounds; i++)
        {
            uint shuffleIndex = 0;
            while (shuffleIndex < leftStack.Length)
            {
                uint chunkSize = (BitConverter.ToUInt32(randomBytes, randomIndex) % 8) + 3; // 3 to 10 cards
                randomIndex += 4;
                if (randomIndex >= randomBytes.Length) randomIndex = 0; // Reset index if needed

                if (shuffleIndex + chunkSize > leftStack.Length) //reduce chunk size if larger than rest of cards
                    chunkSize = (uint)leftStack.Length - shuffleIndex;

                uint leftStackStartIndex = (uint)leftStack.Length - (shuffleIndex + chunkSize);
                Array.Copy(leftStack, leftStackStartIndex, rightStack, shuffleIndex, chunkSize);
                shuffleIndex += chunkSize;
            }
            (leftStack, rightStack) = (rightStack, leftStack);
        }

        _shuffledCards = leftStack;
    }


    /// <summary>
    /// Cuts the deck at a random position, placing the top cards at the bottom<br/>
    /// finalizes the shuffling process and verifies the Deck integrity to allow drawing cards again
    /// </summary>
    /// <remarks>verifies the Deck integrity after Data Generation</remarks>
    /// <exception cref="InvalidOperationException">The Deck sees to be corrupted</exception>
    internal void Cut()
    {
        Random rnd = new Random();
        int cutPoint = rnd.Next(1, _shuffledCards.Length);
        Card[] cutDeck = new Card[52];

        Array.Copy(_shuffledCards, cutPoint, cutDeck, 0, _shuffledCards.Length - cutPoint);
        Array.Copy(_shuffledCards, 0, cutDeck, _shuffledCards.Length - cutPoint, cutPoint);
        _shuffledCards = cutDeck;
        if (!ConfirmDeckIntegrity())
        {
            throw new InvalidOperationException("the deck integrity could not be confirmed!");
        }
        CardCount = cutDeck.Length;
    }


    /// <summary>
    /// Generates the Deck of 52 Cards, one of each CardRank and suit combination
    /// </summary>
    private void GenerateDeck()
    {
        int i = 0;
        foreach (CardRank cardRank in Enum.GetValuesAsUnderlyingType<CardRank>())
        {
            foreach (CardSuit suit in Enum.GetValuesAsUnderlyingType<CardSuit>())
            {
                _shuffledCards[i] = Card.GetCard(cardRank, suit);
                i++;
            }
        }
    }

    /// <summary>
    /// this method validates the integrity of the Deck by verifying that it contains exactly 52 Cards and no Duplicates
    /// </summary>
    /// <returns>true if deck is integer and false if it is corrupted</returns>
    public bool ConfirmDeckIntegrity()
    {
        HashSet<Card> cards = [.._shuffledCards];
        return cards.Count == 52;
    }
    /// <summary>
    /// this function may be used to see how well the deck is shuffled.
    /// </summary>
    /// <returns></returns>
    public double CalculateAverageDisplacement()
    {
        // Generate a sorted deck for comparison
        Deck sortedDeck = new Deck();
        Card[] sortedCards = sortedDeck.GetDeckSnapshot();

        double totalDisplacement = 0;
        for (int i = 0; i < _shuffledCards.Length; i++)
        {
            int sortedPosition = Array.IndexOf(sortedCards, _shuffledCards[i]);
            totalDisplacement += Math.Abs(sortedPosition - i);
        }

        return totalDisplacement / _shuffledCards.Length;
    }
}
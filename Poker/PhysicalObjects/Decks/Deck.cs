using System.Security.Cryptography;
using Poker.PhysicalObjects.Cards;

namespace Poker.PhysicalObjects.Decks;
// TODO: Throughoutly check the shuffle Methods
// TODO: Write Test Cases for the shuffle Methods
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
    
        for (int i = 0; i < _shuffledCards.Length; i++)
        {
            rng.GetBytes(randomBytes);
            uint randomIndex = BitConverter.ToUInt32(randomBytes, 0) % (uint)_shuffledCards.Length;
        
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

        for (int i = 0; i < _shuffledCards.Length; i++)
        {
            byte[] randomNumber = new byte[1];
            RandomNumberGenerator.Fill(randomNumber);
            bool takeFromLeft = randomNumber[0] % 2 == 0;

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
        
        byte[] randomBytes = new byte[4];
        uint shuffleIndex;
        for (int i = 0; i < shuffleRounds; i++)
        {
            shuffleIndex = 0;
            do
            {
                RandomNumberGenerator.Fill(randomBytes);
                uint chunkSize = BitConverter.ToUInt32(randomBytes, 0) % 8 + 3; // 3 to 10 cards
                if (shuffleIndex + chunkSize >= leftStack.Length)
                {
                    chunkSize = (uint)leftStack.Length - shuffleIndex;
                }
                // take chunk
                Array.Copy(leftStack, shuffleIndex,rightStack,shuffleIndex,chunkSize);
                shuffleIndex += chunkSize;
            } while (shuffleIndex < leftStack.Length);
            // swap arrays
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
                _shuffledCards[i] = new Card(cardRank, suit);
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
}
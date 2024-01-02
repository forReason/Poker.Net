namespace Poker.Players
{
    /// <summary>
    /// Defines the different actions a player can take during an action
    /// </summary>
    public enum PlayerAction
    {
        /// <summary>
        /// approves the current bet, basically no action. Passing without penalty
        /// </summary>
        Check,

        /// <summary>
        /// leaves the current turn
        /// </summary>
        Fold,

        /// <summary>
        /// if your current bet is lower than the Table bet, raises your bet to the Table Bet
        /// </summary>
        Call,

        /// <summary>
        /// Raises the Current Bet
        /// </summary>
        Raise,

        /// <summary>
        /// Bets all of the player's remaining chips.
        /// </summary>
        AllIn
    }
}

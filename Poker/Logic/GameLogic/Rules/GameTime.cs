namespace Poker.Net.Logic.GameLogic.Rules
{
    /// <summary>
    /// the GameTime specifies how the Time is beeing calculated. Either by using the actual system clock or calculate it from the rounds played
    /// </summary>
    public enum GameTime
    {
        /// <summary>
        /// this is a simulated, non live environment. The Game Time can be derived by calculating Round * AverageTimePerRound
        /// </summary>
        Simulated,
        /// <summary>
        /// this is a real game. Use the system Time to accurately measure time
        /// </summary>
        RealTime
    }
}

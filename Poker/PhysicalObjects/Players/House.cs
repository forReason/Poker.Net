namespace Poker.Players
{
    /// <summary>
    /// represents the casino, used to track the fees for fun ;)
    /// </summary>
    public static class House
    {
        static House()
        {
            Casino.UniqueIdentifier = "House";
        }
        /// <summary>
        /// represents the casino, used to track the fees for fun ;)
        /// </summary>
        public static Player Casino = new Player();
    }
}

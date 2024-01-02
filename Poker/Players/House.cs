namespace Poker.Players
{
    public static class House
    {
        static House()
        {
            Casino.UniqueIdentifier = "House";
        }
        public static Player Casino = new Player();
    }
}

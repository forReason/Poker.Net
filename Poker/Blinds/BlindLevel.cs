public struct BlindLevel
{
    public int Level { get; set; }
    public ulong SmallBlind { get; set; }
    public ulong BigBlind { get; set; }
    public ulong Ante { get; set; }

    public BlindLevel(int level, ulong smallBlind, ulong bigBlind, ulong ante)
    {
        Level = level;
        SmallBlind = smallBlind;
        BigBlind = bigBlind;
        Ante = ante;
    }
}
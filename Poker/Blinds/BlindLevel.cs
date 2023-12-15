public struct BlindLevel
{
    public int Level { get; set; }
    public double SmallBlind { get; set; }
    public double BigBlind { get; set; }
    public double Ante { get; set; }

    public BlindLevel(int level, double smallBlind, double bigBlind, double ante)
    {
        Level = level;
        SmallBlind = smallBlind;
        BigBlind = bigBlind;
        Ante = ante;
    }
}
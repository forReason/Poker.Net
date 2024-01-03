namespace Poker.PhysicalObjects.Chips;

/// <summary>
/// Provides utility methods for handling and distributing poker chips.
/// </summary>
public partial class Bank
{
    /// <summary>
    /// converts a chip to micro value. EG 1 chip -> 0.01$ 
    /// </summary>
    /// <param name="chip"></param>
    /// <returns></returns>
    public static double GetChipMicroValue(PokerChip chip)
    {
        return ConvertMacroToMicro((ulong)chip);
    }
    /// <summary>
    /// converts a micro to a macro value, eg 0.01$ -> 1 chip value
    /// </summary>
    /// <param name="microValue"></param>
    /// <returns></returns>
    public static ulong ConvertMicroToMacro(double microValue)
    {
        double value = microValue * 100;
        value = Math.Round(value, 0);
        return (ulong)value;
    }
    /// <summary>
    /// converts a decimal micro to a macro value, eg 0.01$ -> 1 chip value
    /// </summary>
    /// <param name="microValue"></param>
    /// <returns></returns>
    public static ulong ConvertMicroToMacro(decimal microValue)
    {
        decimal value = microValue * 100;
        value = Math.Round(value, 0);
        return (ulong)value;
    }
    /// <summary>
    /// converts a macro to a micro value, eg 1 chip value -> 0.01$
    /// </summary>
    /// <param name="microValue"></param>
    /// <returns></returns>
    public static double ConvertMacroToMicro(ulong microValue)
    {
        double value = (double)microValue / 100;
        return Math.Round(value, 2);
    }
    /// <summary>
    /// converts a macro to a decimal micro value, eg 1 chip value -> 0.01$
    /// </summary>
    /// <param name="microValue"></param>
    /// <returns></returns>
    public static decimal ConvertMacroToPreciseMicro(ulong microValue)
    {
        decimal value = (decimal)microValue / 100;
        return Math.Round(value, 2);
    }

}

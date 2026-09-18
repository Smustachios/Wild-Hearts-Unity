using System.Collections.Generic;

public class Reel
{
    public List<Symbols> reelSymbols = new List<Symbols>();

    public Reel(Dictionary<Symbols, int> reelData)
    {
        foreach (var symbolData in reelData)
        {
            for (int i = 0; i < symbolData.Value; i++)
            {
                reelSymbols.Add(symbolData.Key);
            }
        }
    }
}

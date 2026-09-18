using System.Collections.Generic;

public class ReelData
{
    public List<Reel> GetReels()
    {
        Reel realOne = new Reel(reelOne);
        Reel realTwo = new Reel(reelTwo);
        Reel realThree = new Reel(reelThree);
        Reel realFour = new Reel(reelFour);
        Reel realFive = new Reel(reelFive);

        List<Reel> reels = new List<Reel>();
        reels.Add(realOne);
        reels.Add(realTwo);
        reels.Add(realThree);
        reels.Add(realFour);
        reels.Add(realFive);

        return reels;
    }

    public List<Reel> GetBonusReels()
    {
        Reel realOne = new Reel(reelOne);
        Reel realTwo = new Reel(reelTwo);
        Reel realThree = new Reel(reelThree);
        Reel realFour = new Reel(reelFour);
        Reel realFive = new Reel(reelFive);

        List<Reel> reels = new List<Reel>();
        reels.Add(realOne);
        reels.Add(realTwo);
        reels.Add(realThree);
        reels.Add(realFour);
        reels.Add(realFive);

        return reels;
    }

    private Dictionary<Symbols, int> reelOne = new Dictionary<Symbols, int>()
        {
            { Symbols.Club, 10 },
            { Symbols.Diamond, 10 },
            { Symbols.Heart, 9 },
            { Symbols.Spade, 9 },
            { Symbols.S1, 6 },
            { Symbols.S2, 6 },
            { Symbols.S3, 5 },
            { Symbols.S4, 3 },
        };

    private Dictionary<Symbols, int> reelTwo = new Dictionary<Symbols, int>()
        {
            { Symbols.Club, 10 },
            { Symbols.Diamond, 10 },
            { Symbols.Heart, 9 },
            { Symbols.Spade, 9 },
            { Symbols.S1, 6 },
            { Symbols.S2, 6 },
            { Symbols.S3, 5 },
            { Symbols.S4, 3 },
        };

    private Dictionary<Symbols, int> reelThree = new Dictionary<Symbols, int>()
        {
            { Symbols.Club, 10 },
            { Symbols.Diamond, 10 },
            { Symbols.Heart, 9 },
            { Symbols.Spade, 9 },
            { Symbols.S1, 6 },
            { Symbols.S2, 6 },
            { Symbols.S3, 5 },
            { Symbols.S4, 3 },
        };

    private Dictionary<Symbols, int> reelFour = new Dictionary<Symbols, int>()
        {
            { Symbols.Club, 10 },
            { Symbols.Diamond, 10 },
            { Symbols.Heart, 9 },
            { Symbols.Spade, 9 },
            { Symbols.S1, 6 },
            { Symbols.S2, 6 },
            { Symbols.S3, 5 },
            { Symbols.S4, 3 },
        };

    private Dictionary<Symbols, int> reelFive = new Dictionary<Symbols, int>()
        {
            { Symbols.Club, 10 },
            { Symbols.Diamond, 10 },
            { Symbols.Heart, 9 },
            { Symbols.Spade, 9 },
            { Symbols.S1, 6 },
            { Symbols.S2, 6 },
            { Symbols.S3, 5 },
            { Symbols.S4, 3 },
        };

}

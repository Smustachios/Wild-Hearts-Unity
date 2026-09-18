using System.Collections.Generic;

public class Paytable
{
    public List<SymbolPay> Pays { get; private set; }

    public Paytable()
    {
        Pays = new List<SymbolPay>
            {
                new SymbolPay(Symbols.Club, 0.1f, 0.2f, 0.3f),
                new SymbolPay(Symbols.Diamond, 0.1f, 0.2f, 0.3f),
                new SymbolPay(Symbols.Heart, 0.2f, 0.3f, 0.4f),
                new SymbolPay(Symbols.Spade, 0.2f, 0.3f, 0.4f),
                new SymbolPay(Symbols.S1, 0.5f, 0.7f, 1f),
                new SymbolPay(Symbols.S2, 0.5f, 0.7f, 1f),
                new SymbolPay(Symbols.S3, 0.7f, 1f, 2f),
                new SymbolPay(Symbols.S4, 0.7f, 1f, 3f),
                new SymbolPay(Symbols.Bonus, 2f, 3f, 5f)
            };
    }
}

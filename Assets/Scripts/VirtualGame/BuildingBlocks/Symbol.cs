using System.Numerics;

public class Symbol
{
    public Symbols SymbolType { get; set; }
    public int Multiplier { get; set; } = 1;
    public int Hearts { get; set; } = 1;
    public bool WildChecked { get; set; } = false;
    public Vector2 Coordinates { get; set; }

    public Symbol()
    {

    }

    public Symbol(Symbols type)
    {
        SymbolType = type;
    }

    // For wilds
    public Symbol(Symbols type, int multplier, int hearts)
    {
        SymbolType = type;
        Multiplier = multplier;
        Hearts = hearts;
    }

    public void LoseLife()
    {
        Hearts--;
    }

    public void Check(bool toChange)
    {
        WildChecked = toChange;
    }
}

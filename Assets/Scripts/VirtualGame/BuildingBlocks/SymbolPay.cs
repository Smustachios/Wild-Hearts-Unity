public struct SymbolPay
{
    public Symbols Type { get; private set; }
    public float X3 { get; private set; }
    public float X4 { get; private set; }
    public float X5 { get; private set; }

    public SymbolPay(Symbols type, float x3, float x4, float x5)
    {
        Type = type;
        X3 = x3;
        X4 = x4;
        X5 = x5;
    }
}

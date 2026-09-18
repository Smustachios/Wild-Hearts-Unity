using System.Collections.Generic;
using System.Numerics;

public class WinningCombo
{
    public int Type { get; set; }
    public int Ways { get; set; }
    public int Lenght { get; set; }
    public List<Vector2> Coordinates { get; set; } = new List<Vector2>();
    public int WildMultiplier { get; set; } = 1;
    public decimal Win { get; set; }


    public WinningCombo(int type, int ways, int lenght)
    {
        Type = type;
        Ways = ways;
        Lenght = lenght;
    }
}

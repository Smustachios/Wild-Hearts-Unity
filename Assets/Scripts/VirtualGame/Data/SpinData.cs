using System.Collections.Generic;
using System.Numerics;

public class SpinData
{
    public List<FullSpin> RegularSpins {  get; set; }
    public bool BonusHit { get; set; }
    public List<Vector2> ScatterPositions { get; set; }
    public decimal ScatterWin { get; set; }
    public List<Freespin> Freespins { get; set; }
}
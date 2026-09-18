using System.Collections.Generic;

public class Freespin
{
    public List<FullSpin> Spins = new List<FullSpin>();
    public WinningCombo Scatters;


    public Freespin()
    {

    }

    public Freespin(List<FullSpin> spins, WinningCombo scatters)
    {
        Spins = spins;
        Scatters = scatters;
    }
}
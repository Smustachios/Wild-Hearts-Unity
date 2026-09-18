using System;
using System.Collections.Generic;
using System.Numerics;
using Debug = UnityEngine.Debug;

public class Simulator
{
    private decimal bet = 1.0M;

    private Spinner spinner = new Spinner();
    private WinChecker winChecker = new WinChecker();
    private ReelData reelData = new ReelData();
    private Cleaner cleaner = new Cleaner();
    private Paytable payTable = new Paytable();

    private List<Reel> reels;
    private List<Reel> bonusReels;

    public Simulator()
    {
        reels = reelData.GetReels();
        bonusReels = reelData.GetBonusReels();
    }

    public SpinData Spin()
    {
        SpinData spinData = new SpinData();
        List<FullSpin> spins = new List<FullSpin>();

        Board board = new Board(new Symbol[5,3]);

        spinner.InitialSpin(board, reels);
        List<WinningCombo> wins = winChecker.CheckWin(board);

        FullSpin fullSpin = new FullSpin(board, wins);

        while (wins.Count > 0)
        {
            fullSpin.totalWin = winChecker.GetSpinWin(wins, payTable.Pays, bet);

            cleaner.Clean(board, wins);

            fullSpin.movedSymbols = cleaner.MoveSymbols(board);

            Symbol wild = spinner.MakeWild(board, false, out bool wildHit);
            if (wildHit)
            {
                fullSpin.wild = wild;
            }
            else
            {
                fullSpin.wild = new Symbol(Symbols.Empty);
            }

            fullSpin.newSymbols = spinner.ReSpin(board, reels);
            
            wins = winChecker.CheckWin(board);
            spins.Add(fullSpin);

            fullSpin = new FullSpin(board, wins);
        }

        if (spins.Count < 1)
        {
            spins.Add(fullSpin);
        }

        spinData.RegularSpins = spins;

        WinningCombo bonusWin = winChecker.CheckBonus(board);

        if (bonusWin.Lenght >= 3)
        {
            spinData.BonusHit = true;
            spinData.ScatterWin = winChecker.GetSpinWin(new List<WinningCombo>() { bonusWin }, payTable.Pays, bet);
            spinData.ScatterPositions = bonusWin.Coordinates;

            spinData.Freespins = PlayFreespin(5);
        }

        return spinData;
    }

    private List<FullSpin> SingleFreespin(int bonusHearts, int bonusMultiplier, out WinningCombo scatters)
    {
        List<FullSpin> spins = new List<FullSpin>();

        Board board = new Board(new Symbol[5, 3]);

        spinner.InitialSpin(board, reels);
        List<WinningCombo> wins = winChecker.CheckWin(board);

        FullSpin fullSpin = new FullSpin(board, wins);

        while (wins.Count > 0)
        {
            fullSpin.totalWin = winChecker.GetSpinWin(wins, payTable.Pays, bet);

            cleaner.Clean(board, wins);

            fullSpin.movedSymbols = cleaner.MoveSymbols(board);

            Symbol wild = spinner.MakeWild(board, false, out bool wildHit);
            if (wildHit)
            {
                wild.Hearts += bonusHearts;
                wild.Multiplier += bonusMultiplier;
                fullSpin.wild = wild;
            }
            else
            {
                fullSpin.wild = new Symbol(Symbols.Empty);
            }

            fullSpin.newSymbols = spinner.ReSpin(board, reels);

            wins = winChecker.CheckWin(board);
            spins.Add(fullSpin);

            fullSpin = new FullSpin(board, wins);
        }

        scatters = winChecker.CheckBonus(board);

        if (spins.Count < 1)
        {
            spins.Add(fullSpin);
        }


        return spins;
    }

    private List<Freespin> PlayFreespin(int amount)
    {
        List<Freespin> freespins = new List<Freespin>();

        int spinsLeft = amount;
        int bonusHearts = 0;
        int bonusMultiplier = 0;
        int collectedScatters = 0;
        int level = 1;

        while(spinsLeft > 0)
        {
            List<FullSpin> spins = SingleFreespin(bonusHearts, bonusMultiplier, out WinningCombo scatters);

            Freespin freespin = new Freespin(spins, scatters);


            if (scatters.Lenght >= 1 && level < 4)
            {
                Debug.Log($"{scatters.Type}, {scatters.Lenght}, BONUS SYMBOLS");

                collectedScatters += scatters.Lenght;

                if (collectedScatters == 4)
                {
                    spinsLeft += 2;
                    bonusHearts++;
                    bonusMultiplier++;
                    level++;
                    collectedScatters = 0;
                }
            }

            spinsLeft--;

            freespins.Add(freespin);
        }

        return freespins;
    }

    //DEBUG
    private void PrintBoard(Symbol[,] board)
    {
        for (int i = 0; i < 3; i++)
        {
            string line = "";

            for (int j = 0; j < 5; j++)
            {
                line += $"[{(int)board[j, i].SymbolType}]";
            }
            Debug.Log(line);
        }
    }
}

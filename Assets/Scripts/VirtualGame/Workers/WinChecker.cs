using System;
using System.Collections.Generic;
using Vector2 = System.Numerics.Vector2;
using Debug = UnityEngine.Debug;

public class WinChecker
{
    public List<WinningCombo> CheckWin(Board board)
    {
        // Add uniqe first reel symbols (and how many of them) to be checked from other reels
        Dictionary<int, WinningCombo> wins = new Dictionary<int, WinningCombo>();

        List<WinningCombo> finalWins = new List<WinningCombo>();

        for (int i = 0; i < 3; i++)
        {
            int winSymbol = (int)board.BoardGrid[0, i].SymbolType;

            // Bonus symbol cant be winline
            if (winSymbol == 8)
            {
                continue;
            }

            if (wins.ContainsKey(winSymbol))
            {
                wins[winSymbol].Ways++;
            }
            else
            {
                wins.Add(winSymbol, new WinningCombo(winSymbol, 1, 1));
            }

            wins[winSymbol].Coordinates.Add(new Vector2(0, i));
        }

        // Check and count ways of each winning symbol in other reels
        foreach (var firstReelSymbol in wins)
        {
            // Each reel
            for (int i = 1; i < 5; i++)
            {
                int reelHits = 0;
                bool reelHit = false;

                // Each symbol on the reel
                for (int j = 0; j < 3; j++)
                {
                    Symbol symbol = board.BoardGrid[i, j];

                    if ((int)symbol.SymbolType == firstReelSymbol.Key || symbol.SymbolType == Symbols.Wild)
                    {
                        reelHits++;
                        firstReelSymbol.Value.Coordinates.Add(new Vector2(i, j));

                        if (!reelHit)
                        {
                            reelHit = true;
                        }

                        if (symbol.SymbolType == Symbols.Wild)
                        {
                            firstReelSymbol.Value.WildMultiplier *= symbol.Multiplier;
                        }
                    }
                }

                if (reelHit)
                {
                    wins[firstReelSymbol.Key].Ways *= reelHits;
                    wins[firstReelSymbol.Key].Lenght++;
                }
                else
                {
                    break;
                }
            }
        }

        // Remove "wins" that are less than 3 rows long
        foreach (var win in wins)
        {
            if (win.Value.Lenght >= 3)
            {
                finalWins.Add(win.Value);
            }
        }

        return finalWins;
    }

    public WinningCombo CheckBonus(Board board)
    {
        WinningCombo win = new WinningCombo(8, 1, 0);

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < board.BoardGrid.GetLength(1); j++)
            {
                if (board.BoardGrid[i, j].SymbolType == Symbols.Bonus)
                {
                    win.Lenght++;
                    win.Coordinates.Add(new Vector2(i, j));
                }
            }
        }

        return win;
    }


    public decimal GetSpinWin(List<WinningCombo> wins, List<SymbolPay> pays, decimal bet)
    {
        decimal totalWin = 0M;

        foreach (WinningCombo win in wins)
        {
            decimal lineWin = GetWin(win.Ways, win.WildMultiplier, bet, pays, (Symbols)win.Type, win.Lenght);
            win.Win = lineWin;
            totalWin += lineWin;
        }

        return totalWin;
    }


    private float GetMulti(List<SymbolPay> pays, Symbols symbol, int lenght)
    {
        float payMulti = 0f;

        foreach (SymbolPay pay in pays)
        {
            if (symbol == pay.Type)
            {
                if (lenght == 3)
                {
                    payMulti = pay.X3;
                }
                else if (lenght == 4)
                {
                    payMulti = pay.X4;
                }
                else if (lenght == 5)
                {
                    payMulti = pay.X5;
                }
            }
        }

        return payMulti;
    }

    private decimal GetWin(int ways, int wildMulti, decimal bet, List<SymbolPay> pays, Symbols symbol, int lenght)
    {
        decimal win = 0M;

        win = ways * (decimal)GetMulti(pays, symbol, lenght) * bet * wildMulti;

        return win;
    }
}

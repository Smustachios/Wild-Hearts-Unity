using System;
using System.Collections.Generic;
using System.Numerics;
using Random = System.Random;

public class Spinner
{
    private Random rng = new Random();

    private float extraWildP = 0.25f;
    private float extraHeartP = 0.25f;
    private float extraMultiplierP = 0.15f;
    private float bonusSymbolP = 0.4f;
    private float symbolBonusP = 0.14f;

    // First spin of the game
    public void InitialSpin(Board board, List<Reel> reels)
    {
        for (int i = 0; i < 5; i++)
        {
            Reel reel = reels[i];
            // Each reel has small chance to get bonus symbol
            if (rng.NextDouble() <= bonusSymbolP)
            {
                int bonusPos = rng.Next(0, 3);

                for (int j = board.BoardGrid.GetLength(1) - 1; j >= 0; j--)
                {
                    if (j == bonusPos)
                    {
                        board.BoardGrid[i, j] = new Symbol(Symbols.Bonus);
                    }
                    else
                    {
                        int randomSymbol = rng.Next(0, reel.reelSymbols.Count);

                        board.BoardGrid[i, j] = new Symbol(reel.reelSymbols[randomSymbol]);

                        // reel.reelSymbols.RemoveAt(randomSymbol);
                    }
                }
            }

            else
            {
                for (int j = board.BoardGrid.GetLength(1) - 1; j >= 0; j--)
                {
                    int randomSymbol = rng.Next(0, reel.reelSymbols.Count);

                    board.BoardGrid[i, j] = new Symbol(reel.reelSymbols[randomSymbol]);

                    // reel.reelSymbols.RemoveAt(randomSymbol);
                }
            }
        }
    }


    public Dictionary<Vector2, Symbols> ReSpin(Board board, List<Reel> reels)
    {
        Random rng = new Random();
        Dictionary<Vector2, Symbols> newSymbols = new Dictionary<Vector2, Symbols>();

        for (int i = 0; i < board.BoardGrid.GetLength(0); i++)
        {
            Reel reel = reels[i];
            int emptys = 0;
            List<Vector2> emptyCoords = new List<Vector2>();
            bool hasBonus = false;

            // Start putting symbols on the board from bottom to top
            for (int j = board.BoardGrid.GetLength(1) - 1; j >= 0; j--)
            {
                Symbols symbol = board.BoardGrid[i, j].SymbolType;

                if (symbol == Symbols.Empty)
                {
                    emptys++;

                    int randomSymbol = rng.Next(0, reel.reelSymbols.Count);

                    board.BoardGrid[i, j] = new Symbol(reel.reelSymbols[randomSymbol]);
                    emptyCoords.Add(new Vector2(i, j));

                    // reel.reelSymbols.RemoveAt(randomSymbol);

                    newSymbols.Add(new Vector2(i, j), board.BoardGrid[i, j].SymbolType);
                }
                else if (symbol == Symbols.Bonus)
                {
                    hasBonus = true;
                }
            }

            if (!hasBonus && emptys >= 1)
            {
                if (rng.NextDouble() <= emptys * symbolBonusP)
                {
                    Vector2 pos = emptyCoords[rng.Next(0, emptyCoords.Count)];
                    board.BoardGrid[(int)pos.X, (int)pos.Y] = new Symbol(Symbols.Bonus);

                    newSymbols[pos] = Symbols.Bonus;
                }
            }
        }

        return newSymbols;
    }


    public Symbol MakeWild(Board board, bool bonus, out bool wildHit)
    {
        List<Vector2> emptySymbols = new List<Vector2>();
        bool hasWild = false;
        Symbol wild = new Symbol();

        // Check all the avaible spots to put wilds first and then make wild

        // Wild cant be on the first reel => i = 1
        for (int i = 1; i < board.BoardGrid.GetLength(0); i++)
        {
            bool rowHasWild = false;
            List<Vector2> rowSymbols = new List<Vector2>();

            for (int j = 0; j < board.BoardGrid.GetLength(1); j++)
            {
                // Cant have more than 1 wild on each reel
                if (board.BoardGrid[i, j].SymbolType == Symbols.Wild)
                {
                    rowHasWild = true;
                    break;
                }

                if (board.BoardGrid[i, j].SymbolType == Symbols.Empty)
                {
                    rowSymbols.Add(new Vector2(i, j));
                }
            }

            // Only make spots avaible on the row if there isnt wild there
            if (rowHasWild)
            {
                hasWild = true;
            }
            else
            {
                foreach (Vector2 symbol in rowSymbols)
                {
                    emptySymbols.Add(symbol);
                }
            }
        }

        // 100% give wild if no wild on the board
        if (!hasWild)
        {
            Vector2 newWild = emptySymbols[rng.Next(0, emptySymbols.Count)];

            int hearts = MakeHearts(bonus);
            int multiplier = MakeMultiplier(bonus);

            wild = new Symbol(Symbols.Wild, multiplier, hearts);
            wild.Coordinates = new Vector2(newWild.X, newWild.Y);
            wildHit = true;

            board.BoardGrid[(int)newWild.X, (int)newWild.Y] = new Symbol(Symbols.Wild, multiplier, hearts);
        }
        // Only small chance to get new wild if already has a wild on the board
        else
        {
            if (rng.NextDouble() < extraWildP)
            {
                Vector2 newWild = emptySymbols[rng.Next(0, emptySymbols.Count)];

                int hearts = MakeHearts(bonus);
                int multiplier = MakeMultiplier(bonus);

                wild = new Symbol(Symbols.Wild, multiplier, hearts);
                wild.Coordinates = new Vector2(newWild.X, newWild.Y);
                wildHit = true;

                board.BoardGrid[(int)newWild.X, (int)newWild.Y] = new Symbol(Symbols.Wild, multiplier, hearts);
            }
            else
            {
                wildHit = false;
            }
        }

        return wild;
    }


    private int MakeHearts(bool bonus)
    {
        int hearts = 1;
        int amount = bonus ? 4 : 2;

        for (int i = 0; i < amount; i++)
        {
            if (rng.NextDouble() < extraHeartP)
            {
                hearts++;
            }
        }

        return hearts;
    }

    private int MakeMultiplier(bool bonus)
    {
        int multiplier = 1;
        int amount = bonus ? 4 : 2;

        for (int i = 0; i < amount; i++)
        {
            if (rng.NextDouble() < extraMultiplierP)
            {
                multiplier++;
            }
        }

        return multiplier;
    }
}

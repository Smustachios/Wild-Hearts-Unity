using System.Collections.Generic;
using System.Numerics;

public class Cleaner
{
    // Change every winning symbol into empty symbol
    public void Clean(Board board, List<WinningCombo> wins)
    {
        foreach (var win in wins)
        {
            foreach (var coord in win.Coordinates)
            {
                Symbol symbol = board.BoardGrid[(int)coord.X, (int)coord.Y];

                if (symbol.SymbolType == Symbols.Wild)
                {
                    continue;
                }
                else
                {
                    board.BoardGrid[(int)coord.X, (int)coord.Y] = new Symbol(Symbols.Empty);
                }
            }
        }

        // Check if wild needs to be destroyed
        for (int i = 0; i < board.BoardGrid.GetLength(0); i++)
        {
            for (int j = 0; j < board.BoardGrid.GetLength(1); j++)
            {
                Symbol symbol = board.BoardGrid[i, j];

                if (symbol.SymbolType == Symbols.Wild)
                {
                    if (symbol.Hearts > 1)
                    {
                        symbol.LoseLife();
                    }
                    else
                    {
                        board.BoardGrid[i, j] = new Symbol(Symbols.Empty);
                    }
                }
            }
        }
    }

    public void CleanBonus(Board board, WinningCombo win)
    {
        foreach (Vector2 coord in win.Coordinates)
        {
            board.BoardGrid[(int)coord.X, (int)coord.Y] = new Symbol(Symbols.Empty);
        }
    }

    public Dictionary<Vector2, Vector2> MoveSymbols(Board board)
    {
        // Return for spindata
        Dictionary<Vector2, Vector2> movedSymbols = new Dictionary<Vector2, Vector2>();

        // Check every row
        for (int i = 0; i < 5; i++)
        {
            // Where to move left over symbols if there is winning symbols on the row
            Vector2 target = new Vector2(i, 2);
            bool targetTaken = false;

            // Check and move every symbol on the row (if need to)
            for (int j = 2; j >= 0; j--)
            {
                if ((int)board.BoardGrid[i, j].SymbolType == -1)
                {
                    if (!targetTaken)
                    {
                        target = new Vector2(i, j);
                        targetTaken = true;

                        movedSymbols.Add(target, target);
                    }

                    continue;
                }
                else
                {
                    if (targetTaken)
                    {
                        board.BoardGrid[(int)target.X, (int)target.Y] = board.BoardGrid[i, j];
                        board.BoardGrid[i, j] = new Symbol(Symbols.Empty);

                        movedSymbols[target] = new Vector2(i, j);
                        movedSymbols.Add(new Vector2(i, j), new Vector2(i, j));

                        target.X = i;
                        target.Y = j;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        return movedSymbols;
    }
}

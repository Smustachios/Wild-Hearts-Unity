using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class FullSpin
{
    public Symbol[,] initBoard  = new Symbol[5, 3];
    public List<WinningCombo> wins;

    public Symbol wild = new Symbol(Symbols.Empty);

    // First is target and second is symbol coords
    public Dictionary<Vector2, Vector2> movedSymbols = new Dictionary<Vector2, Vector2>();

    public Dictionary<Vector2, Symbols> newSymbols = new Dictionary<Vector2, Symbols>();

    public decimal totalWin = 0;


    public FullSpin(Board board, List<WinningCombo> initWins)
    {
        initBoard = (Symbol[,])board.BoardGrid.Clone();
        this.wins = initWins;
    }
}
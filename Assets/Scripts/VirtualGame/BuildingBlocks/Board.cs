public struct Board
{
    // Y axis is revesed
    public Symbol[,] BoardGrid;

    public Board(Symbol[,] boardGrid)
    {
        BoardGrid = boardGrid;
    }
}

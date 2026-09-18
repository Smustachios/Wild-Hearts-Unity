using System;
using UnityEngine;
using System.Collections.Generic;

public class BoardController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Rows;

    [SerializeField]
    private GameObject ClubPrefab, DiamondPrefab, HeartPrefab, SpadePrefab, S1Prefab, S2Prefab, S3Prefab, S4Prefab, BonusPrefab, WildPrefab;

    public List<GameObject> Wilds = new List<GameObject>();

    private GameObject[,] symbols = new GameObject[5, 3];



    public void SetBoard(Symbol[,] board)
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            int yPos = 1;

            for (int j = 0; j < board.GetLength(1); j++)
            {
                Symbols type = board[i, j].SymbolType;
                GameObject prefab = Instantiate(GetSymbolType(type), new Vector2(Rows[i].transform.position.x, yPos), ClubPrefab.transform.rotation, Rows[i].transform);
                prefab.GetComponent<GameSymbol>().Type = type;
                symbols[i, j] = prefab;
                yPos--;
            }
        }
    }

    public void ShowWins(List<WinningCombo> wins)
    {
        foreach (WinningCombo w in wins)
        {
            foreach (System.Numerics.Vector2 coord in w.Coordinates)
            {
                SpriteRenderer symbol = symbols[(int)coord.X, (int)coord.Y].GetComponent<SpriteRenderer>();

                symbol.color = Color.red;
            }
        }
    }

    public void ShowScatters(List<System.Numerics.Vector2> coords)
    {
        foreach (System.Numerics.Vector2 coord in coords)
        {
            SpriteRenderer symbol = symbols[(int)coord.X, (int)coord.Y].GetComponent<SpriteRenderer>();

            symbol.color = Color.red;
        }
    }

    public void ClearWins(List<WinningCombo> wins)
    {
        List<GameObject> wildsToRemove = new List<GameObject>();

        foreach (WinningCombo w in wins)
        {
            foreach (System.Numerics.Vector2 coord in w.Coordinates)
            {
                GameObject symbolToClear = symbols[(int)coord.X, (int)coord.Y];

                if (symbolToClear.GetComponent<GameSymbol>().Type != Symbols.Wild)
                {
                    Destroy(symbolToClear);

                    symbols[(int)coord.X, (int)coord.Y] = null;
                }
            }
        }

        foreach (GameObject wild in Wilds)
        {
            GameSymbol wildScript = wild.GetComponent<GameSymbol>();

            SpriteRenderer[] hearts = wildScript.GetComponentsInChildren<SpriteRenderer>();

            for (int i = 3; i >= 1; i--)
            {
                if (hearts[i].enabled)
                {
                    hearts[i].enabled = false;
                    break;
                }
            }

            if (wildScript.Hearts == 1)
            {
                wildsToRemove.Add(wild);
            }
            else
            {
                wildScript.Hearts--;
            }
        }

        foreach (GameObject wild in wildsToRemove)
        {
            Wilds.Remove(wild);
            Destroy(wild);
            GameSymbol wildScript = wild.GetComponent<GameSymbol>();
            symbols[(int)wildScript.Coordinates.x, (int)wildScript.Coordinates.y] = null;
        }
    }

    public void ClearScatters(WinningCombo scatters)
    {
        foreach (System.Numerics.Vector2 coord in scatters.Coordinates)
        {
            GameObject symbolToClear = symbols[(int)coord.X, (int)coord.Y];
            symbols[(int)coord.X, (int)coord.Y] = null;

            Destroy(symbolToClear);
        }
    }

    public void MoveSymbols(Dictionary<System.Numerics.Vector2, System.Numerics.Vector2> symbols)
    {
        foreach (var symbol in symbols)
        {
            if (symbol.Value == symbol.Key)
            {
                continue;
            }

            GameObject symbolToMove = this.symbols[(int)symbol.Value.X, (int)symbol.Value.Y];
            this.symbols[(int)symbol.Value.X, (int)symbol.Value.Y] = null;
            this.symbols[(int)symbol.Key.X, (int)symbol.Key.Y] = symbolToMove;

            if (symbolToMove != null)
            {
                symbolToMove.transform.position = new Vector2(symbolToMove.transform.position.x, GetColumnCoord(symbol.Key.Y));
            }
        }
    }

    public void MakeWild(Symbol wild)
    {
        GameObject wildPrefab = Instantiate(WildPrefab, new Vector2(GetRowCoord(wild.Coordinates.X), GetColumnCoord(wild.Coordinates.Y)), WildPrefab.transform.rotation, Rows[(int)wild.Coordinates.X].transform);
        Transform[] hearts = wildPrefab.GetComponentsInChildren<Transform>();
        symbols[(int)wild.Coordinates.X, (int)wild.Coordinates.Y] = wildPrefab;

        GameSymbol wildSymbol = wildPrefab.GetComponent<GameSymbol>();
        wildSymbol.WildSetup(Symbols.Wild, wild.Hearts, wild.Multiplier, new Vector2(wild.Coordinates.X, wild.Coordinates.Y));
        Wilds.Add(wildPrefab);

        for (int i = 0; i < wild.Hearts; i++)
        {
            hearts[i+1].GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void ReSpin(Dictionary<System.Numerics.Vector2, Symbols> newSymbols)
    {
        foreach (var symbol in newSymbols)
        {
            GameObject symbolPrefab = Instantiate(GetSymbolType(symbol.Value), new Vector2(GetRowCoord(symbol.Key.X), GetColumnCoord(symbol.Key.Y)), Quaternion.Euler(Vector2.zero), Rows[(int)symbol.Key.X].transform);

            symbols[(int)symbol.Key.X, (int)symbol.Key.Y] = symbolPrefab;
        }
    }

    public void ClearBoard()
    {
        for (int i = 0; i < symbols.GetLength(0); i++)
        {
            for (int j = 0; j < symbols.GetLength(1); j++)
            {
                Destroy(symbols[i, j]);
                symbols[i, j] = null; 
            }
        }
    }

    private GameObject GetSymbolType(Symbols symbol)
    {
        switch (symbol)
        {
            case Symbols.Club:
                return ClubPrefab;
            case Symbols.Diamond:
                return DiamondPrefab;
            case Symbols.Heart:
                return HeartPrefab;
            case Symbols.Spade:
                return SpadePrefab;
            case Symbols.S1:
                return S1Prefab;
            case Symbols.S2:
                return S2Prefab;
            case Symbols.S3:
                return S3Prefab;
            case Symbols.S4:
                return S4Prefab;
            case Symbols.Bonus:
                return BonusPrefab;
            case Symbols.Wild:
                return WildPrefab;

            default:
                return ClubPrefab;
        }
    }

    private int GetColumnCoord(float boardCoord)
    {
        if (boardCoord == 0)
        {
            return 1;
        }
        else if (boardCoord == 1)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }

    private int GetRowCoord(float boardCoord)
    {
        if (boardCoord == 0)
        {
            return -2;
        }
        else if (boardCoord == 1)
        {
            return -1;
        }
        else if (boardCoord == 2)
        {
            return 0;
        }
        else if (boardCoord == 3)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
}

using TMPro;
using UnityEngine;

public class GameSymbol : MonoBehaviour
{
    public Symbols Type {  get; set; }
    public int Hearts { get; set; }
    public int Multiplier { get; set; }
    public Vector2 Coordinates { get; set; }

    [SerializeField]
    public TextMeshPro MultiplierText;


    public void WildSetup(Symbols type, int hearts, int multiplier, Vector2 coords)
    {
        Type = type;
        Hearts = hearts;
        Multiplier = multiplier;
        Coordinates = coords;
        MultiplierText.text = $"{multiplier}X";
    }
}

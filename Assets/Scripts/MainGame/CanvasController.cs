using TMPro;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI SpinWin;

    [SerializeField]
    private TextMeshProUGUI TotalWin;

    [SerializeField]
    private TextMeshProUGUI Bankroll;

    [SerializeField]
    private TextMeshProUGUI FreespinsMessage;

    [SerializeField]
    private TextMeshProUGUI FreespinsLeft;

    public void SetSpinWin(string text)
    {
        SpinWin.text = text;
    }

    public void SetTotalWin(string text)
    {
        TotalWin.text = text;
    }

    public void SetBankroll(string text)
    {
        Bankroll.text = text;
    }

    public void SetFreespinsMessage(string text)
    {
        FreespinsMessage.text = text;
    }

    public void SetFreespinsLeft(string text) 
    {
        FreespinsLeft.text = text;
    }
}

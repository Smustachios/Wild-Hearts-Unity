using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CanvasController canvasController;
    private Simulator simulator;
    private BoardController boardController;
    private BankrollManager bankrollManager;
    private SpinData spinData;
    private bool isSpinning = false;
    private bool playingBonus = false;
    private float timerOne = 1f;
    private int freespinTracker = 0;
    private int freespinsLeft = 5;
    decimal totalWin = 0;



    private void Awake()
    {
        simulator = new Simulator();
        boardController = GetComponent<BoardController>();
        bankrollManager = GetComponent<BankrollManager>();

        canvasController.SetBankroll($"{decimal.Round(bankrollManager.Bankroll, 2)}");
    }

    public void SpinButton()
    {
        StartCoroutine(Spin());
    }

    public IEnumerator Spin()
    {
        if (!isSpinning && !playingBonus)
        {
            canvasController.SetFreespinsMessage("");

            isSpinning = true;

            boardController.ClearBoard();

            bankrollManager.Bankroll -= (decimal)bankrollManager.Bet;
            canvasController.SetBankroll($"{decimal.Round(bankrollManager.Bankroll, 2)}");

            spinData = simulator.Spin();

            boardController.SetBoard(spinData.RegularSpins[0].initBoard);

            yield return GameLoop(spinData.RegularSpins);

            if (spinData.BonusHit)
            {
                playingBonus = true;
                yield return ShowBonus(spinData);
            }
            else
            {
                totalWin = 0;
                canvasController.SetTotalWin("");
            }
        }
        else if (!isSpinning && playingBonus)
        {
            canvasController.SetFreespinsMessage("");
            canvasController.SetFreespinsLeft($"{freespinsLeft - freespinTracker} freespins left!");

            isSpinning = true;

            boardController.ClearBoard();

            boardController.SetBoard(spinData.Freespins[freespinTracker].Spins[0].initBoard);

            yield return GameLoop(spinData.Freespins[freespinTracker].Spins);

            // Scatters! 

            freespinTracker++;

            if (freespinTracker == spinData.Freespins.Count)
            {
                playingBonus = false;
                freespinTracker = 0;
                canvasController.SetFreespinsLeft("");
                canvasController.SetFreespinsMessage($"{spinData.Freespins.Count} freespins won {totalWin}!!");
                totalWin = 0;
                canvasController.SetTotalWin("");
            }

        }
    }

    IEnumerator GameLoop(List<FullSpin> spins)
    {
        foreach (FullSpin sp in spins)
        {
            if (sp.wins.Count <= 0)
            {
                break;
            }

            totalWin += sp.totalWin;
            yield return new WaitForSeconds(timerOne);

            boardController.ShowWins(sp.wins);
            canvasController.SetSpinWin($"{decimal.Round(sp.totalWin, 2)}");
            canvasController.SetTotalWin($"{decimal.Round(totalWin, 2)}");
            yield return new WaitForSeconds(timerOne);
            canvasController.SetSpinWin("");

            boardController.ClearWins(sp.wins);
            yield return new WaitForSeconds(timerOne);

            boardController.MoveSymbols(sp.movedSymbols);
            yield return new WaitForSeconds(timerOne);

            if (sp.wild.SymbolType != Symbols.Empty)
            {
                boardController.MakeWild(sp.wild);
                yield return new WaitForSeconds(timerOne);
            }

            if (sp.newSymbols.Count > 0)
            {
                boardController.ReSpin(sp.newSymbols);
            }
        }

        bankrollManager.Bankroll += totalWin;
        canvasController.SetBankroll($"{decimal.Round(bankrollManager.Bankroll, 2)}");
        boardController.Wilds.Clear();

        isSpinning = false;
    }

    IEnumerator ShowBonus(SpinData spins)
    {
        totalWin += spins.ScatterWin;

        yield return new WaitForSeconds(timerOne);
        boardController.ShowScatters(spins.ScatterPositions);
        canvasController.SetSpinWin($"{decimal.Round(spins.ScatterWin, 2)}");
        canvasController.SetTotalWin($"{decimal.Round(totalWin, 2)}");

        yield return new WaitForSeconds(timerOne);
        canvasController.SetSpinWin("");
        bankrollManager.Bankroll += totalWin;
        canvasController.SetBankroll($"{decimal.Round(bankrollManager.Bankroll, 2)}");
        canvasController.SetFreespinsMessage("5 freespins won!!");
    }
}

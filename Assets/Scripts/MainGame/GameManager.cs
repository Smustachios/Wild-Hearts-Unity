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
    private int scatters = 0;
    private int bonusLevel = 1;
    private decimal totalWin = 0;



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
            scatters += spinData.Freespins[freespinTracker].Scatters.Lenght;
            if (scatters >= 4 && bonusLevel < 4)
            {
                bonusLevel++;
                freespinsLeft += 2;
                scatters -= 4;
                canvasController.SetFreespinsMessage("2 extra spins won!");
            }

            freespinTracker++;

            if (freespinTracker == spinData.Freespins.Count)
            {
                playingBonus = false;
                canvasController.SetFreespinsLeft("");
                canvasController.SetFreespinsMessage($"{spinData.Freespins.Count} freespins won {totalWin}!!");
                totalWin = 0;
                scatters = 0;
                freespinTracker = 0;
                freespinsLeft = 0;
                bonusLevel = 1;
                canvasController.SetTotalWin("");
            }

        }
    }

    IEnumerator GameLoop(List<FullSpin> spins)
    {
        foreach (FullSpin spin in spins)
        {
            if (spin.wins.Count <= 0)
            {
                break;
            }

            totalWin += spin.totalWin;
            yield return new WaitForSeconds(timerOne);

            boardController.ShowWins(spin.wins);
            canvasController.SetSpinWin($"{decimal.Round(spin.totalWin, 2)}");
            canvasController.SetTotalWin($"{decimal.Round(totalWin, 2)}");
            yield return new WaitForSeconds(timerOne);
            canvasController.SetSpinWin("");

            boardController.ClearWins(spin.wins);
            yield return new WaitForSeconds(timerOne);

            boardController.MoveSymbols(spin.movedSymbols);
            yield return new WaitForSeconds(timerOne);

            if (spin.wild.SymbolType != Symbols.Empty)
            {
                boardController.MakeWild(spin.wild);
                yield return new WaitForSeconds(timerOne);
            }

            if (spin.newSymbols.Count > 0)
            {
                boardController.ReSpin(spin.newSymbols);
            }
        }

        bankrollManager.Bankroll += totalWin;
        canvasController.SetBankroll($"{decimal.Round(bankrollManager.Bankroll, 2)}");
        boardController.Wilds.Clear();

        isSpinning = false;
    }

    IEnumerator ShowBonus(SpinData spins)
    {
        // Debug
        Debug.Log($"TOTAL FREESPINS: {spins.Freespins.Count}");
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
        freespinsLeft = 5;
    }

    private void CheckScatters()
    {

    }
}

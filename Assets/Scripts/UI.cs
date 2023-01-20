using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject restartDialogue, leaderboard, guide;
    [SerializeField] CanvasGroup background;
    private bool restartDialogueIsShowed, leaderboardIsShowed, guideIsShowed;
    private const float duration = 0.5f;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Restart()
    {
        gameController.Restart();
    }

    // RestartDialogue
    public void RestartButtonClick()
    {
        if (!restartDialogueIsShowed && !leaderboardIsShowed && !guideIsShowed) ShowRestartDialogue();
        else BackgroundClick();
    }

    public void NoButtonClick()
    {
        HideRestartDialogue();
    }

    public void YesButtonClick()
    {
        Restart();
    }

    private void ShowRestartDialogue()
    {
        restartDialogue.LeanMoveLocalY(0, duration).setEaseOutExpo().setOnStart(EnableRestartDialogue);
        background.LeanAlpha(1, duration).setEaseOutExpo();
        restartDialogueIsShowed = true;
        DisableControls();
    }

    private void HideRestartDialogue()
    {
        restartDialogue.LeanMoveLocalY(-1200, duration).setOnComplete(DisableRestartDialogue).setEaseInExpo();
        background.LeanAlpha(0, duration).setEaseInExpo();
        restartDialogueIsShowed = false;
        EnableControls();
    }

    private void DisableRestartDialogue()
    {
        restartDialogue.SetActive(false);
    }

    private void EnableRestartDialogue()
    {
        restartDialogue.SetActive(true);
    }


    // Leaderboard
    public void LeaderboardButtonClick()
    {
        if (!restartDialogueIsShowed && !leaderboardIsShowed && !guideIsShowed) ShowLeaderboard();
        else BackgroundClick();
    }

    private void ShowLeaderboard()
    {
        leaderboard.LeanMoveLocalY(0, duration).setEaseOutExpo().setOnStart(EnableLeaderboard);
        background.LeanAlpha(1, duration).setEaseOutExpo();
        leaderboardIsShowed = true;
        DisableControls();
    }

    private void HideLeaderboard()
    {
        leaderboard.LeanMoveLocalY(-1500, duration).setOnComplete(DisableLeaderboard).setEaseInExpo();
        background.LeanAlpha(0, duration).setEaseInExpo();
        leaderboardIsShowed = false;
        EnableControls();
    }

    private void DisableLeaderboard()
    {
        leaderboard.SetActive(false);
    }

    private void EnableLeaderboard()
    {
        leaderboard.SetActive(true);
    }


    // Guide
    public void GuideClick()
    {
        if (!restartDialogueIsShowed && !leaderboardIsShowed && !guideIsShowed) ShowGuide();
        else BackgroundClick();
    }
    private void ShowGuide()
    {
        guide.LeanMoveLocalY(0, duration).setEaseOutExpo().setOnStart(EnableGuide);
        background.LeanAlpha(1, duration).setEaseOutExpo();
        guideIsShowed = true;
        DisableControls();
    }

    private void HideGuide()
    {
        guide.LeanMoveLocalY(-1600, duration).setOnComplete(DisableGuide).setEaseInExpo();
        background.LeanAlpha(0, duration).setEaseInExpo();
        guideIsShowed = false;
        EnableControls();
    }

    private void DisableGuide()
    {
        guide.SetActive(false);
    }

    private void EnableGuide()
    {
        guide.SetActive(true);
    }


    // EnableControls
    public void BackgroundClick()
    {
        if (leaderboardIsShowed) HideLeaderboard();
        if (restartDialogueIsShowed) HideRestartDialogue();
        if (guideIsShowed) HideGuide();
        gameController.ControlsIsAvaliable = true;
    }
    public void EnableControls()
    {
        if (leaderboardIsShowed && restartDialogueIsShowed && !guideIsShowed) gameController.ControlsIsAvaliable = true;
    }
    public void DisableControls()
    {
        gameController.ControlsIsAvaliable = false;
    }
}

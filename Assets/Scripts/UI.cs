using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class UI : MonoBehaviour
{
    private GameController _gameController;

    [SerializeField] private GameObject _restartDialogue, _leaderboard, _winDialogue, _guide, _levelsDialogue;
    [SerializeField] CanvasGroup _background;
    [SerializeField] LevelButton[] _levelButtons;
    public string LeaderboardName { get; private set; }

    private bool _windowIsShowed;

    private const float _duration = 0.4f;

    void Start()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        LeaderboardName = _leaderboard.GetComponent<LeaderboardYG>().nameLB;
        if (PlayerPrefs.GetInt("GuideIsShowed") == 0)
        {
            ShowGuide();
            PlayerPrefs.SetInt("GuideIsShowed", 1);
        }
    }

    public void Restart()
    {
        _gameController.Restart();
    }

    // RestartDialogue
    public void RestartButtonClick()
    {
        if (_gameController.IsWin) return;
        if (!_windowIsShowed) ShowRestartDialogue();
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
        _restartDialogue.LeanCancel();
        EnableRestartDialogue();
        _restartDialogue.LeanMoveLocalY(0, _duration).setEaseOutExpo();
        _background.LeanAlpha(1, _duration).setEaseOutExpo();
        _windowIsShowed = true;
        DisableControls();
    }

    private void HideRestartDialogue()
    {
        _restartDialogue.LeanCancel();
        _restartDialogue.LeanMoveLocalY(-1200, _duration).setOnComplete(DisableRestartDialogue).setEaseInExpo();
        _background.LeanAlpha(0, _duration).setEaseInExpo();
        _windowIsShowed = false;
        EnableControls();
    }

    private void DisableRestartDialogue()
    {
        _restartDialogue.SetActive(false);
    }

    private void EnableRestartDialogue()
    {
        _restartDialogue.SetActive(true);
    }


    // Leaderboard
    public void LeaderboardButtonClick()
    {
        if (_gameController.IsWin) return;
        if (!_windowIsShowed) ShowLeaderboard();
        else BackgroundClick();
    }

    private void ShowLeaderboard()
    {
        _leaderboard.LeanCancel();
        EnableLeaderboard();
        _leaderboard.LeanMoveLocalY(0, _duration).setEaseOutExpo();
        _background.LeanAlpha(1, _duration).setEaseOutExpo();
        _windowIsShowed = true;
        DisableControls();
    }

    private void HideLeaderboard()
    {
        _leaderboard.LeanCancel();
        _leaderboard.LeanMoveLocalY(-1500, _duration).setOnComplete(DisableLeaderboard).setEaseInExpo();
        _background.LeanAlpha(0, _duration).setEaseInExpo();
        _windowIsShowed = false;
        EnableControls();
    }

    private void DisableLeaderboard()
    {
        _leaderboard.SetActive(false);
    }

    private void EnableLeaderboard()
    {
        _leaderboard.SetActive(true);
    }


    // Guide
    public void GuideClick()
    {
        if (_gameController.IsWin) return;
        if (!_windowIsShowed) ShowGuide();
        else BackgroundClick();
    }
    private void ShowGuide()
    {
        _guide.LeanCancel();
        EnableGuide();
        _guide.LeanMoveLocalY(0, _duration).setEaseOutExpo();
        _background.LeanAlpha(1, _duration).setEaseOutExpo();
        _windowIsShowed = true;
        DisableControls();
    }

    private void HideGuide()
    {
        _guide.LeanCancel();
        _guide.LeanMoveLocalY(-1600, _duration).setOnComplete(DisableGuide).setEaseInExpo();
        _background.LeanAlpha(0, _duration).setEaseInExpo();
        _windowIsShowed = false;
        EnableControls();
    }

    private void DisableGuide()
    {
        _guide.SetActive(false);
    }

    private void EnableGuide()
    {
        _guide.SetActive(true);
    }


    // EnableControls
    public void BackgroundClick()
    {
        if (_gameController.IsWin) return;
        if (_leaderboard.activeInHierarchy) HideLeaderboard();
        if (_restartDialogue.activeInHierarchy) HideRestartDialogue();
        if (_guide.activeInHierarchy) HideGuide();
        if (_levelsDialogue.activeInHierarchy) HideLevelsDialogue();

        _gameController.SwitchControls(true);
    }
    public void EnableControls()
    {
        if (!_windowIsShowed) _gameController.SwitchControls(true);
    }
    public void DisableControls()
    {
        _gameController.SwitchControls(false);
    }
    

    // WinDialogue
    public void ShowWinDialogue()
    {
        _winDialogue.LeanCancel();
        EnableWinDialogue();
        _winDialogue.LeanMoveLocalY(0, _duration).setEaseOutExpo();
        _background.LeanAlpha(1, _duration).setEaseOutExpo();
        LevelButtonsUpdate();
    }
    private void EnableWinDialogue()
    {
        _winDialogue.SetActive(true);
    }



    // LevelsDialogue
    public void LevelsDialogueButtonClick()
    {
        if (_gameController.IsWin) return;
        if (!_windowIsShowed) ShowLevelsDialogue();
        else BackgroundClick();
    }

    private void ShowLevelsDialogue()
    {
        _levelsDialogue.LeanCancel();
        EnableLevelsDialogue();
        _levelsDialogue.LeanMoveLocalY(0, _duration).setEaseOutExpo();
        _background.LeanAlpha(1, _duration).setEaseOutExpo();
        _windowIsShowed = true;
        DisableControls();
        LevelButtonsUpdate();
    }

    private void HideLevelsDialogue()
    {
        _levelsDialogue.LeanMoveLocalY(-1500, _duration).setOnComplete(DisableLevelsDialogue).setEaseInExpo();
        _background.LeanAlpha(0, _duration).setEaseInExpo();
        _windowIsShowed = false;
        EnableControls();
    }

    private void DisableLevelsDialogue()
    {
        _levelsDialogue.SetActive(false);
    }

    private void EnableLevelsDialogue()
    {
        _levelsDialogue.SetActive(true);
    }

    public void LevelButton_Click(int index)
    {
        int firstLevelInBuildSettings = 1;
        SceneManager.LoadScene(index + firstLevelInBuildSettings);
    }

    private void LevelButtonsUpdate()
    {
        foreach(LevelButton button in _levelButtons)
        {
            button.UpdateButton();
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using YG;

public class GameController : MonoBehaviour
{
    [SerializeField] public int LevelIndex;

    [SerializeField] private Disc[] _allDiscs;
    [SerializeField] private float _diskHeight;
    private float _choicedDiscHeight;
    [SerializeField] private Tower[] _towers;
    private Disc _choicedDisc;
    private int _moveCount;
    private Text _moveCountText, _smallestNumberOfMovesText;
    public bool ControlsIsAvaliable { get; private set; } = true;
    private int _smallestNumberOfMoves;
    private UI _userInterface;
    public bool IsWin { get; private set; }

    [SerializeField] private AdController _adController;

    private void Awake()
    {
        _choicedDiscHeight = (_allDiscs.Length + 1) * _diskHeight;
        _moveCountText = GameObject.Find("MoveCount").GetComponent<Text>();
        _smallestNumberOfMovesText = GameObject.Find("SmallestMoveCount").GetComponent<Text>();
        _userInterface = GameObject.Find("UI").GetComponent<UI>();
    }

    void Start()
    {
        PlayerPrefs.SetInt("LastPlayedLevel", SceneManager.GetActiveScene().buildIndex);
        _smallestNumberOfMoves = PlayerPrefs.GetInt($"{SceneManager.GetActiveScene().buildIndex}SmallestNumberOfMoves");
        _smallestNumberOfMovesText.text = _smallestNumberOfMoves != 0 ? _smallestNumberOfMoves.ToString() : "";
        LoadProgress();
    }

    public float GetHeight()
    {
        return _choicedDiscHeight;
    }

    public void SwitchControls(bool value)
    {
        ControlsIsAvaliable = value;
    }

    public void OnClick(Tower tower)
    {
        if (!ControlsIsAvaliable || IsWin) return;

        if (tower.discs.Count != 0 && _choicedDisc == null)
        {

            Vector3 endPosition = new Vector3(tower.GetPosition().x, tower.GetPosition().y + _choicedDiscHeight, tower.GetPosition().z);
            _choicedDisc = tower.discs[tower.discs.Count - 1];
            _choicedDisc.GetComponent<MoveAnimation>().PickUp(tower.GetPosition() + new Vector3(0, _diskHeight * (tower.discs.Count - 1), 0), endPosition);

            tower.discs.Remove(_choicedDisc);
            _choicedDisc.transform.parent = null;
        }
        else
        {
            if (_choicedDisc != null && (tower.discs.Count == 0 || _choicedDisc.Scale < tower.discs[tower.discs.Count - 1].Scale))
            {
                Vector3 beginPosition = new Vector3(_choicedDisc.transform.position.x, _choicedDiscHeight, _choicedDisc.transform.position.z);
                Vector3 endPosition = new Vector3(tower.GetPosition().x, tower.GetPosition().y + tower.discs.Count * _diskHeight, tower.GetPosition().z);

                if (new Vector2(beginPosition.x, beginPosition.z) != new Vector2(endPosition.x, endPosition.z))
                    _moveCountText.text = (++_moveCount).ToString();

                _choicedDisc.GetComponent<MoveAnimation>().DoMove(beginPosition, endPosition);

                tower.discs.Add(_choicedDisc);
                _choicedDisc.transform.parent = tower.transform;
                _choicedDisc.towerIndex = Array.IndexOf(_towers, tower);
                _choicedDisc = null;
                SaveProgress();

                WinCheck();
            }
        }
    }

    private void WinCheck()
    {
        if (_towers[2].discs.Count == _allDiscs.Length)
        {
            Invoke("Win", 0.5f);
            IsWin = true;
            if (PlayerPrefs.GetInt("AvaliableLevels") < LevelIndex + 1)
                PlayerPrefs.SetInt("AvaliableLevels", LevelIndex + 1);
        }
    }

    private void Win()
    {
        if (_smallestNumberOfMoves == 0 || _moveCount < _smallestNumberOfMoves)
        {
            PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().buildIndex}SmallestNumberOfMoves", _smallestNumberOfMoves = _moveCount);
            YandexGame.NewLeaderboardScores(_userInterface.LeaderboardName, _smallestNumberOfMoves);
        }
        ResetSave();
        ControlsIsAvaliable = false;
        _userInterface.ShowWinDialogue();
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().buildIndex}StepsCount", _moveCount);

        for (int discIndex = _allDiscs.Length - 1; discIndex >= 0; discIndex--)
            PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().buildIndex}{discIndex}", _allDiscs[discIndex].towerIndex);
    }

    private void LoadProgress()
    {
        _moveCountText.text = (_moveCount = PlayerPrefs.GetInt($"{SceneManager.GetActiveScene().buildIndex}StepsCount", _moveCount)).ToString();

        for (int discIndex = _allDiscs.Length - 1; discIndex >= 0; discIndex--)
        {
            int towerIndex = PlayerPrefs.GetInt($"{SceneManager.GetActiveScene().buildIndex}{discIndex}");
            if (towerIndex == -1) Restart();

            _allDiscs[discIndex].transform.position = new Vector3(_towers[towerIndex].GetPosition().x,
                _towers[towerIndex].GetPosition().y + _towers[towerIndex].discs.Count * _diskHeight, _towers[towerIndex].GetPosition().z);
            _towers[towerIndex].discs.Add(_allDiscs[discIndex]);
            _allDiscs[discIndex].transform.parent = _towers[towerIndex].transform;
        }
    }

    public void Restart()
    {
        ResetSave();

        _adController.ShowAd();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetSave()
    {
        _moveCountText.text = (_moveCount = 0).ToString();

        for (int i = _allDiscs.Length - 1; i >= 0; i--)
        {
            _allDiscs[i].towerIndex = 0;
        }
        SaveProgress();
    }
}
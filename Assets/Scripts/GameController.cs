using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using YG;

public class GameController : MonoBehaviour
{
    [SerializeField] Disc[] allDiscs;
    [SerializeField] float diskHeight;
    private float choicedDiscHeight;
    [SerializeField] Tower[] towers;
    private Disc choicedDisc;
    private int moveCount;
    [SerializeField] Text moveCountText, smallestNumberOfMovesText;
    public bool ControlsIsAvaliable = true;
    [SerializeField] bool savesIsEnabled;
    private int theLeastNumberOfMoves;

    [SerializeField] AdController adController;
    private void Awake()
    {
        choicedDiscHeight = 8 * diskHeight;
    }
    void Start()
    {
        theLeastNumberOfMoves = PlayerPrefs.GetInt("SmallestNumberOfMoves");
        smallestNumberOfMovesText.text = theLeastNumberOfMoves != 0 ? theLeastNumberOfMoves.ToString() : "";
        LoadProgress();
    }
    public float GetHeight()
    {
        return choicedDiscHeight;
    }
    public void OnClick(Tower tower)
    {
        if (!ControlsIsAvaliable) return;
        if (tower.discs.Count != 0 && choicedDisc == null)
        {
            choicedDisc = tower.discs[tower.discs.Count - 1];
            tower.discs.Remove(choicedDisc);
            choicedDisc.transform.parent = null;
            Vector3 endPosition = new Vector3(tower.GetPosition().x, tower.GetPosition().y + choicedDiscHeight, tower.GetPosition().z);
            choicedDisc.GetComponent<MoveAnimation>().PickUp(choicedDisc.transform.position, endPosition);
        }
        else
        {
            if (choicedDisc != null && (tower.discs.Count == 0 || choicedDisc.scale < tower.discs[tower.discs.Count - 1].scale))
            {
                Vector3 beginPosition = new Vector3(choicedDisc.transform.position.x, choicedDiscHeight, choicedDisc.transform.position.z);
                Vector3 endPosition = new Vector3(tower.GetPosition().x, tower.GetPosition().y + tower.discs.Count * diskHeight, tower.GetPosition().z);
                
                if (new Vector2(beginPosition.x, beginPosition.z) != new Vector2(endPosition.x, endPosition.z))
                    moveCountText.text = (++moveCount).ToString();

                choicedDisc.GetComponent<MoveAnimation>().DoMove(beginPosition, endPosition);

                tower.discs.Add(choicedDisc);
                choicedDisc.transform.parent = tower.transform;
                choicedDisc.towerIndex = Array.IndexOf(towers, tower);
                choicedDisc = null;
                SaveProgress();
                
                WinCheck();
            }
        }
    }
    private void WinCheck()
    {
        if (towers[2].discs.Count == allDiscs.Length - 1) Win();
    }
    private void Win()
    {
        if (moveCount < theLeastNumberOfMoves)
        {
            PlayerPrefs.SetInt("TheLeastNumberOfMoves", theLeastNumberOfMoves = moveCount);
            YandexGame.NewLeaderboardScores("TheLeastNumberOfMoves", theLeastNumberOfMoves);
        }
        ControlsIsAvaliable = false;
        adController.ShowAd();
    }
    private void SaveProgress()
    {
        PlayerPrefs.SetInt("StepsCount", moveCount);

        for (int discIndex = allDiscs.Length - 1; discIndex >= 0; discIndex--)
            PlayerPrefs.SetInt($"{discIndex}", allDiscs[discIndex].towerIndex);
    }
    private void LoadProgress()
    {
        moveCountText.text = (moveCount = PlayerPrefs.GetInt("StepsCount", moveCount)).ToString();

        for (int discIndex = allDiscs.Length - 1; discIndex >= 0; discIndex--)
        {
            int towerIndex = PlayerPrefs.GetInt($"{discIndex}");
            if (towerIndex == -1) Restart();

            allDiscs[discIndex].transform.position = new Vector3(towers[towerIndex].GetPosition().x,
                towers[towerIndex].GetPosition().y + towers[towerIndex].discs.Count * diskHeight, towers[towerIndex].GetPosition().z);
            towers[towerIndex].discs.Add(allDiscs[discIndex]);
            allDiscs[discIndex].transform.parent = towers[towerIndex].transform;
        }
    }
    public void Restart()
    {
        moveCountText.text = (moveCount = 0).ToString();

        for (int i = allDiscs.Length - 1; i >= 0; i--)
        {
            allDiscs[i].towerIndex = 0;
        }
        SaveProgress();
        SceneManager.LoadScene(0);
    }
}
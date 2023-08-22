using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLauncher : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("LastPlayedLevel", 1));
    }
}

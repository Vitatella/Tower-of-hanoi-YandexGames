using UnityEngine;
using UnityEngine.UI;
public class LevelButton : MonoBehaviour
{
    [SerializeField] private int _index;
    private Button _button;

    public void UpdateButton()
    {
        if (_button == null) _button = gameObject.GetComponent<Button>();
        _button.interactable = _index <= PlayerPrefs.GetInt("AvaliableLevels") ? true : false;
    }
}

using UnityEngine;
using YG;

public class AdController : MonoBehaviour
{
    [SerializeField] YandexGame yandexGame;

    public void ShowAd()
    {
        yandexGame._FullscreenShow();
    }
}

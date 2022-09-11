using __PUBLISH_v1.Scripts;
using DG.Tweening;
using fromWordSearch;
using UnityEngine;
using UnityEngine.UI;

public class EulaPrivacy : MonoBehaviour
{
    const string KEY = "EULA_PRIVACY_READED";
    public Button acceptButton;
    public Button urlButton;
    public LevelConfig level;
    public string url;
    [SerializeField] CanvasGroup group;
    [SerializeField] CanvasGroup popupGroup;
    [SerializeField] float fadeTime = 0.4f;
    [SerializeField] float pompSize = 1.5f;
    [SerializeField] float pompTime = 0.2f;

    void Awake()
    {
        group.alpha = 0;
        popupGroup.alpha = 0;
    }

    void Start()
    {
        var isRead = PlayerPrefs.HasKey(KEY);
        if (isRead)
            GameManager.Instance.LoadLevel(level, 0, false);
        else
            Invoke(nameof(Show), 0.1f);
    }

    void Show()
    {
        acceptButton.onClick.AddListener(LoadMainMenu);
        urlButton.onClick.AddListener(OpenURL);

        group.DOFade(1, fadeTime);
        popupGroup.DOFade(1, fadeTime);
        popupGroup.transform
            .DOScale(pompSize, pompTime / 2)
            .OnComplete(() => popupGroup.transform.DOScale(1, pompTime / 2));
    }

    void LoadMainMenu()
    {
        PlayerPrefs.SetInt(KEY, 1);
        PlayerPrefs.Save();
        GameManager.Instance.LoadLevel(level, 0, false);
    }

    void OpenURL() => Application.OpenURL(url);
}
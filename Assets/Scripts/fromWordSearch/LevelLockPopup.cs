using DG.Tweening;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;
using UnityEngine.UI;

public class LevelLockPopup : MonoBehaviour
{
    [SerializeField] CanvasGroup mainGroup;
    [SerializeField] CanvasGroup popupGroup;
    [SerializeField] Button button;
    [SerializeField] float fadeTime;
    [SerializeField] float pompSize;
    [SerializeField] float pompTime;
    [SerializeField] SoundData lockSound;
    [SerializeField] SoundData clickSound;

    void Awake()
    {
        Disable();
    }

    bool _isShow;

    public void Show()
    {
        if (_isShow) return;

        Enable();
        AudioManager.Instance.PlaySound(lockSound);
        
        mainGroup.DOFade(1, fadeTime);
        popupGroup.DOFade(1, fadeTime);
        popupGroup.transform
            .DOScale(pompSize, pompTime / 2)
            .OnComplete(() => popupGroup.transform.DOScale(1, pompTime / 2));

        button.onClick.AddListener(Hide);
        _isShow = true;
    }

    void Hide()
    {
        AudioManager.Instance.PlaySound(clickSound);
        button.onClick.RemoveListener(Hide);
        mainGroup.DOFade(0, fadeTime).OnComplete(Disable);
        _isShow = false;
    }

    void Disable()
    {
        mainGroup.alpha = 0;
        mainGroup.interactable = false;
        mainGroup.blocksRaycasts = false;

        popupGroup.alpha = 0;
    }

    void Enable()
    {
        mainGroup.interactable = true;
        mainGroup.blocksRaycasts = true;
    }
}
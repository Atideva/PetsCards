using DG.Tweening;
using fromWordSearch;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WInPlayUI : MonoBehaviour
{
    [SerializeField] CanvasGroup mainGroup;
    [SerializeField] CanvasGroup buttonGroup;
    [SerializeField] CanvasGroup levelGroup;
    [SerializeField] Image levelRibbonImage;
    [SerializeField] TextMeshProUGUI levelRibbonText;
    [SerializeField] CanvasGroup levelRibbonGroup;
    [SerializeField] LevelSelectItemUI levelItem;
    [SerializeField] float maxSize = 1.3f;

    float _fadeTime;
    float _delay;
    SoundData _sound;

    void Awake() => Disable();
    public Color lvlCardColor;
    public void Init(LevelConfig lvl, float fadeTime, float delay, SoundData sound)
    {
        _sound = sound;
        _fadeTime = fadeTime;
        _delay = delay;

        buttonGroup.alpha = 0;
        mainGroup.alpha = 0;

        levelItem.RefreshGUI(lvl);
        levelItem.Unlock(lvlCardColor);
        levelItem.UnComplete();
    }

    public void Show(float mainDelay)
    {
        Enable();
        Animate(levelGroup, mainDelay);
        //   DOVirtual.Float(0f, 100f, _fadeTime * 2, (v) => levelRibbonText.text = Mathf.Floor(v).ToString(CultureInfo.InvariantCulture));
        AudioManager.Instance.PlaySound(_sound, mainDelay);
        levelRibbonImage.fillAmount = 0;
        levelRibbonImage
            .DOFillAmount(1, _fadeTime * 2)
            .SetDelay(mainDelay);
        levelRibbonImage.transform
            .DOScale(1.5f, _fadeTime / 2)
            .SetDelay(mainDelay)
            .OnComplete(()
                => levelRibbonImage.transform
                    .DOScale(1, _fadeTime / 2));

        //   Animate(levelRibbonGroup, mainDelay + _delay);
        AudioManager.Instance.PlaySound(_sound, mainDelay + _delay);
        buttonGroup
            .DOFade(1, _fadeTime)
            .SetDelay(mainDelay + _delay);
        buttonGroup.transform
            .DOScale(1.5f, _fadeTime / 2)
            .SetDelay(mainDelay + _delay)
            .OnComplete(()
                => buttonGroup.transform
                    .DOScale(1, _fadeTime / 2));
    }

    public void Hide()
        => mainGroup.DOFade(0, _fadeTime).OnComplete(Disable);

    void Enable()
    {
        mainGroup.alpha = 1;
        mainGroup.interactable = true;
        mainGroup.blocksRaycasts = true;
    }

    void Disable()
    {
        mainGroup.interactable = false;
        mainGroup.blocksRaycasts = false;
    }

    public void Animate(CanvasGroup canvasGroup, float delay)
    {
        if (!gameObject.activeSelf) return;
        canvasGroup.alpha = 0;
        canvasGroup
            .DOFade(1, _fadeTime / 2)
            .SetDelay(delay);
        canvasGroup.transform
            .DOScale(maxSize, _fadeTime / 2)
            .SetDelay(delay)
            .OnComplete(()
                => canvasGroup.transform
                    .DOScale(1, _fadeTime / 2));
    }
}
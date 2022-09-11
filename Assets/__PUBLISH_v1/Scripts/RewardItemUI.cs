using DG.Tweening;
using TMPro;
using UnityEngine;

public class RewardItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] float maxSize;
    [SerializeField] CanvasGroup canvasGroup;
    float _fadeTime;

    public void Init(int count, float fadeTime)
    {
        _fadeTime = fadeTime;
        canvasGroup.alpha = 0;
        txt.text = count.ToString();
    }

    public void Disable()
        => gameObject.SetActive(false);

    public void Animate(float delay)
    {
        if (!gameObject.activeSelf) return;
        canvasGroup
            .DOFade(1, _fadeTime / 2)
            .SetDelay(delay);
        transform
            .DOScale(maxSize, _fadeTime / 2)
            .SetDelay(delay)
            .OnComplete(() => transform.DOScale(1, _fadeTime / 2));
    }
}
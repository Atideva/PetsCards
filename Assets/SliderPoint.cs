using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderPoint : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] Image back;
    [SerializeField] Image checkMark;
    [Header("Animation")]
    [SerializeField] float pumpSize = 2f;
    [SerializeField] float activeSize = 1.3f;
    [SerializeField] float completeSize = 1.3f;
    [SerializeField] float pumpTime = 0.4f;
    [Header("Colors")]
    [SerializeField] Color txtActiveColor;
    [SerializeField] Color txtPassiveColor;
    [SerializeField] Color txtCompleteColor;

    Sprite _backComplete;
    Sprite _backUnComplete;
    Sprite _activeSprite;


    public void Init(int value, Sprite completeSprite, Sprite uncompleteSprite, Sprite activeSprite)
    {
        _activeSprite = activeSprite;
        txt.text = value.ToString();
        _backComplete = completeSprite;
        _backUnComplete = uncompleteSprite;
        Passive();
    }

    public void Complete()
    {
        // txt.color = txtCompleteColor;
        txt.enabled = false;
        back.sprite = _backComplete;
        checkMark.gameObject.SetActive(true);
        transform
            .DOScale(pumpSize, pumpTime / 2)
            .OnComplete(()
                => transform.DOScale(completeSize, pumpTime / 2));
    }


    public void Active()
    {
        txt.color = txtActiveColor;
        back.sprite = _activeSprite;
        transform
            .DOScale(activeSize, pumpTime)
            .SetDelay(pumpTime + 0.2f);
    }

    void Passive()
    {
        txt.color = txtPassiveColor;
        back.sprite = _backUnComplete;
        checkMark.gameObject.SetActive(false);
    }
}
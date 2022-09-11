using __PUBLISH_v1.Scripts;
using DG.Tweening;
using game.cards.data;
using UnityEngine;
using UnityEngine.UI;

public class SliderCardPoint : PoolObject
{
    [Header("Images")]
    [SerializeField] Image back;
    [SerializeField] Image icon;
    [Header("Materials")]
    [SerializeField] Sprite backSprite;
    [SerializeField] Sprite backDangerSprite;
    [SerializeField] Material grayBackMaterial;
    [SerializeField] Material grayPetMaterial;
    [SerializeField] Material dangerPetMaterial;
    [SerializeField] Material normalMaterial;
    [Header("Animation")]
    [SerializeField] float pumpSize = 2f;
    [SerializeField] float completeSize = 1.3f;
    [SerializeField] float pumpTime = 0.4f;
    CardData _cd;
    public float showTime = 0.2f;
    public CanvasGroup group;
    public CardData Data => _cd;

    public float PumpTime => pumpTime;

    public void Init(CardData cd, bool isDanger)
    {
        _cd = cd;
        icon.sprite = GameManager.Instance.IsCraft
            ? cd.ResultCraft
            : cd.CardPrefab.MainArt.sprite;
        back.sprite = isDanger ? backDangerSprite : backSprite;
        Passive(isDanger);
    }

    public float showSize = 1.3f;

    public void Show(float delay)
    {
        group.alpha = 0;
        group.DOFade(1, showTime).SetDelay(delay);
        transform.localScale = Vector3.zero;
        transform
            .DOScale(showSize, showTime / 2)
            .OnComplete(() => transform.DOScale(1, showTime / 2));
    }

    public bool IsComplete { get; private set; }

    public void Complete()
    {
        IsComplete = true;
        icon.material = normalMaterial;
        back.material = normalMaterial;
        back.sprite = backSprite;
        transform
            .DOScale(pumpSize, pumpTime / 2)
            .OnComplete(() => transform.DOScale(completeSize, pumpTime / 2));
    }

    public void Passive(bool isDanger)
    {
        IsComplete = false;
        icon.material = grayPetMaterial;
        back.material = isDanger ? null : grayBackMaterial;
    }

    void OnDisable() => ReturnToPool();
}
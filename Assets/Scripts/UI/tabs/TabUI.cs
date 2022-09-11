using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UI.tabs;
using UnityEngine;
using UnityEngine.UI;

public class TabUI : MonoBehaviour
{
    [SerializeField] private Tab onTab;

    [SerializeField] private Image circleImage;
    [SerializeField] private Sprite circleEnableSprite;
    [SerializeField] private Sprite circleDisableSprite;
    [SerializeField] private RectTransform container;
    [SerializeField] private float containerMoveBy;
    [SerializeField] private float containerMoveTime;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private List<Image> backgrounds = new();
    [SerializeField] private Color backEnableColor;
    [SerializeField] private Color backDisableColor;
    [SerializeField] private List<Image> frames = new();
    [SerializeField] private Color frameEnableColor;
    [SerializeField] private Color frameDisableColor;
    bool _active = true;
         bool _init;
         void Init() => _init = true;

    public bool _passive
    {
        get => !_active;
        set => _active = !value;
    }

    void Awake()
    {
        onTab.OnTabEnabled += Enable;
        onTab.OnTabDisabled += Disable;
        Invoke(nameof(Init), 1f);
    }

    void Enable()
    {
        if (_active) return;
        _active = true;
        circleImage.sprite = circleEnableSprite;
        foreach (var b in backgrounds) b.color = backEnableColor;
        foreach (var f in frames) f.color = frameEnableColor;
        if (_init)
            container.DOLocalMoveY(-containerMoveBy, containerMoveTime).SetRelative(true);
        else
            container.anchoredPosition = new Vector2(0, 0);
        circleImage.transform
            .DOScale(1.4f, containerMoveTime / 2)
            .OnComplete(() => circleImage.transform.DOScale(1.1f, containerMoveTime / 2));
        nameLabel.enabled = true;
    }

    void Disable()
    {
        if (_passive) return;
        _passive = true;
        circleImage.sprite = circleDisableSprite;
        foreach (var b in backgrounds) b.color = backDisableColor;
        foreach (var f in frames) f.color = frameDisableColor;
        if (_init)
            container.DOLocalMoveY(containerMoveBy, containerMoveTime).SetRelative(true);
        else
            container.anchoredPosition = new Vector2(0, containerMoveBy);
 
        circleImage.transform
            .DOScale(1, containerMoveTime);
        nameLabel.enabled = false;
    }

}
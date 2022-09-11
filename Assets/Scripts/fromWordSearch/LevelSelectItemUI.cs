using System;
using __PUBLISH_v1.Scripts;
using DG.Tweening;
using fromWordSearch;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LevelSelectItemUI : MonoBehaviour
{
    [Header("Mode")]
    [SerializeField] CanvasGroup modeNone;
    [SerializeField] CanvasGroup modeLives;
    [SerializeField] CanvasGroup modeTimer;
    [Header("Status")]
    public DOTweenAnimation jumpAnim;
    [SerializeField] CanvasGroup statusLock;
    [SerializeField] CanvasGroup statusComplete;
    [Header("Card")]
    [SerializeField] CanvasGroup card;
    [SerializeField] Image cardIcon;
    [SerializeField] Image cardIconShadow;
    [SerializeField] Image diagonalShadow;
    [SerializeField] Image cardImage;
    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] Button cardClickButton;
    [Header("Settings")]
    [SerializeField] [Range(0, 1)]
    float lockAlpha = 0.3f;
    [Header("Manual set level")]
    [SerializeField] bool useManualLevel;

    [SerializeField] LevelConfig manualLevel;


    LevelConfig _lvl;
    LevelData _data;
    Color _noneColor;
    Color _cardLockColor;
    Color _cardCompleteColor;
    Color _livesColor;
    Color _timerColor;
    public event Action<LevelConfig, LevelData> OnClick = delegate { };


    void Awake()
    {
        if (useManualLevel && manualLevel)
        {
            SetMode(LevelMode.None);
            RefreshGUI(manualLevel);
            ResetPositions();
            cardClickButton.onClick.AddListener(LoadLevel);
        }
        else
        {
            cardClickButton.onClick.AddListener(Click);
        }
    }

    void LoadLevel()
    {
        GameManager.Instance.LoadLevel(manualLevel);
    }

    public void Click() => OnClick(_lvl, _data);


    public void Init(LevelConfig lvl, LevelData data, Color cardColor, Color cardLockColor, Color cardCompleteColor,
        Color timerColor, Color livesColor)
    {
        _timerColor = timerColor;
        _livesColor = livesColor;
        _cardCompleteColor = cardCompleteColor;
        _cardLockColor = cardLockColor;
        _noneColor = cardColor;
        _lvl = lvl;
        _data = data;

        cardImage.color = cardColor;

        ResetPositions();
        RefreshGUI(lvl);
        SetMode(lvl.Mode);
        SetState(data);
    }

    public void RefreshGUI(LevelConfig lvl)
    {
        SetIcon(lvl.Icon);
        SetName(lvl.LvlName);
    }

    void SetIcon(Sprite sprite)
    {
        cardIcon.sprite = sprite;
        cardIconShadow.sprite = sprite;
        cardIcon.enabled = sprite;
        cardIconShadow.enabled = sprite;
    }

  public  void SetName(string text)
  {
      cardName.enabled = true;
        cardName.text = text;
    }

    public void Lock()
    {
        cardImage.color = _cardLockColor;
        statusLock.alpha = 1;
        card.alpha = lockAlpha;
        modeNone.alpha = 0;
        cardIcon.color = Color.black;
        diagonalShadow.enabled = false;
    }

    public void Unlock(Color color)
    {
        cardImage.color = color;
        jumpAnim.DOPlay();
        statusLock.alpha = 0;
        card.alpha = 1;
        cardIcon.color = Color.white;
        diagonalShadow.enabled = true;
    }

    public void Complete()
    {
        jumpAnim.DOKill();
        statusComplete.alpha = 1;
        modeNone.alpha = 0;
        Unlock(_noneColor);
        cardImage.color = _cardCompleteColor;
        
    }
public void DisableName()=>cardName.enabled = false;
    public void UnComplete()
    {
        statusComplete.alpha = 0;
    }

    public void SetMode(LevelMode mode)
    {
        cardImage.color = mode == LevelMode.None
            ? _noneColor
            : mode == LevelMode.Timer
                ? _timerColor
                : _livesColor;
        modeNone.alpha = mode == LevelMode.None ? 1 : 0;
        modeLives.alpha = mode == LevelMode.Lives ? 1 : 0;
        modeTimer.alpha = mode == LevelMode.Timer ? 1 : 0;
    }

    void HideModes()
    {
        modeNone.alpha = 0;
        modeLives.alpha = 0;
        modeTimer.alpha = 0;
    }

    public void SetState(LevelData data = null)
    {
        if (data == null || data.isLock)
        {
            HideModes();
            Lock();
        }
        else
            Unlock(_noneColor);

        if (data is {isComplete: true})
        {
            HideModes();
            Complete();
        }
        else
            UnComplete();
    }

    void ResetPositions()
    {
        modeNone.transform.localPosition = Vector3.zero;
        modeLives.transform.localPosition = Vector3.zero;
        modeTimer.transform.localPosition = Vector3.zero;
        statusLock.transform.localPosition = Vector3.zero;
        statusComplete.transform.localPosition = Vector3.zero;
    }
}
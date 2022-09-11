using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] TextMeshProUGUI txtScore;
    [SerializeField] bool useScoreAnimation = true;
    [SerializeField] bool useHideAnimation = true;
    [SerializeField] bool changeRibbonSprite = true;
    [SerializeField] bool changeRibbonColor;
    [SerializeField] RectTransform hideTransform;
    [SerializeField] float hideMoveBy;
    [SerializeField] float hideMoveTime = 0.5f;
    [SerializeField] Image ribbon;
    [SerializeField] float pumpSize = 3f;
    [SerializeField] float pumpTime = 1f;
    [SerializeField] Color ribbonActiveColor;
    [SerializeField] Color ribbonPassiveColor;
    Color _passiveColor;
    Sprite _activeSprite;
    Sprite _passiveSprite;
    public void Init(Sprite passiveSprite, Color passiveColor)
    {
        _activeSprite = ribbon.sprite;
        _passiveSprite = passiveSprite;
        _passiveColor = passiveColor;
        _score = 0;
        active = true;
        txtScore.text = _score.ToString();

    }
    int _score;

    public int Score => _score;

    public void AddScore()
    {
        _score++;
        txtScore.text = _score.ToString();
        if (useScoreAnimation)
            txtScore.transform
              .DOScale(pumpSize, pumpTime / 2)
              .OnComplete(() => txtScore.transform.DOScale(1, pumpTime / 2));
    }
    bool active;
    public void Active()
    {
        if (changeRibbonSprite) ribbon.sprite = _activeSprite;
        if (changeRibbonColor) ribbon.DOColor(ribbonActiveColor, ribbonColorTime);
        txt.color = Color.white;
        txtScore.color = Color.white;
        if (useHideAnimation && !active)
        {
            active = true;
            var moveBy = hideTransform.rect.height * hideMoveBy;
            var dir = hideDown ? 1 : -1;
            transform.DOLocalMoveY(moveBy * dir, hideMoveTime).SetRelative(true);
        }

    }
    public float ribbonColorTime=1f;
    [SerializeField] bool hideDown = true;
    public void Passive()
    {
        if(changeRibbonSprite)        ribbon.sprite = _passiveSprite;
        if (changeRibbonColor) ribbon.DOColor(ribbonPassiveColor, ribbonColorTime);
        txt.color = _passiveColor;
     //   txtScore.color = _passiveColor;
        if (useHideAnimation && active)
        {
            active = false;
            var moveBy = hideTransform.rect.height * hideMoveBy;
            var dir = hideDown ? 1 : -1;
            transform.DOLocalMoveY(-moveBy * dir, hideMoveTime).SetRelative(true);
        }

    }

}

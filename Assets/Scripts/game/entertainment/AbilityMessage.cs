using System;
using DG.Tweening;
using game.cards;
using game.managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace game.entertainment
{
    public class AbilityMessage : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] Canvas canvas;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] float animTime = 0.5f;
        [SerializeField] float activeShowTime = 2f;
        [SerializeField] float pumpSize = 1.3f;
        [SerializeField] float pumpTime = 0.5f;
        [Header("Text")]
        [SerializeField] Sprite ribbonGood;
        [SerializeField] Sprite ribbonEvil;
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] Transform nameContainer;
        [SerializeField] Image nameRibbon;
        [SerializeField] TextMeshProUGUI descriptionText;
        [SerializeField] Color warningColor;
        [SerializeField] Color goodColor;
        [SerializeField] Color evilColor;
        [Header("Glow line")]
        [SerializeField] Image backImage;
        [SerializeField] Color lineGoodColor;
        [SerializeField] Color lineEvilColor;
        AbilityConfig _config;
        [SerializeField] DOTweenAnimation shakeAnim;

          bool _isAnimated;
        bool _isDisabled;

        public void Disable() => _isDisabled = true;

        //  public DOTweenAnimation shakeAnim;
        void Start()
        {
            Events.Instance.OnShowAbilityMessage += Show;
            Events.Instance.OnHideAbilityMessage += Hide;
            Events.Instance.OnAbilityUse += AbilityUse;
            DisableCanvas();
            nameRibbon.enabled = false;
            canvasGroup.alpha = 1;
        }

        void AbilityUse(AbilityConfig config, Card card1, Card card2)
        {
            if (_isDisabled) return;

            switch (config.Type)
            {
                case AbilityType.Good:
                    UseAnimation(goodColor, ribbonGood);
                    break;

                case AbilityType.Evil:
                    UseAnimation(evilColor, ribbonEvil);
                    break;
            }
        }


        void UseAnimation(Color textClr, Sprite ribbon)
        {
            _isAnimated = true;
            //    glowLine.enabled = true;
            //    glowLine.DOColor(lineClr, animTime);
            //    shakeAnim.DOPause();
            nameText
                .DOColor(textClr, animTime)
                .OnComplete(() => Invoke(nameof(DisableAnimation), activeShowTime));
            //   descriptionText
            //        .DOColor(Color.white, animTime / 3f);   
            descriptionText
                .DOColor(Color.clear, 0.1f);

            nameRibbon.enabled = true;
            nameRibbon.color = Color.white;
            nameRibbon.sprite = ribbon;
            nameRibbon.DOFade(1, 0.1f);
            // nameContainer.transform
            //    .DOScale(pumpSize * 1.5f, pumpTime*3f);
            backImage.DOFade(0, 0.1f);
            nameContainer.transform
                .DOScale(1.5f, pumpTime / 1.8f)
                .OnComplete(() => nameContainer.transform.DOScale(1.1f, pumpTime / 1.8f));
        }


        void Hide()
        {
            if (_isDisabled) return;
            if (_isAnimated) return;

            //    shakeAnim.DOPause();
            //     if (glowLine.enabled)
            //         glowLine.DOColor(Color.clear, animTime);

            nameText.DOColor(Color.clear, animTime).OnComplete(DisableCanvas);
            descriptionText.DOColor(Color.clear, animTime);
        }

        void DisableAnimation()
        {
            //  shakeAnim.DOPause();
            //     if (glowLine.enabled)
            //         glowLine.DOColor(Color.clear, animTime);

            nameRibbon.DOColor(Color.clear, 0.66f);
 
            nameText.DOColor(Color.clear, 0.66f).OnComplete(DisableCanvas);
            //       descriptionText.DOColor(Color.clear, animTime);
        }

        void Show(AbilityConfig config)
        {
            if (_isDisabled) return;

            shakeAnim.DOPlay();
            _config = config;
            EnableCanvas();
            var nameColor = config.Type == AbilityType.Good ? goodColor : evilColor;
            var descColor = warningColor;

            nameRibbon.enabled = false;
            backImage.DOFade(1, 0.2f);
            nameText.text = config.AbilityName;
            nameText.color = Color.clear;
            nameText.DOColor(nameColor, animTime);
            nameText.transform
                .DOScale(pumpSize, pumpTime / 2)
                .OnComplete(() => nameText.transform.DOScale(1, pumpTime / 2));

            descriptionText.text = config.Description;
            descriptionText.color = Color.clear;
            descriptionText.DOColor(descColor, animTime);

            //  shakeAnim.DOPlay();
            //      glowLine.enabled = false;
        }


        void DisableCanvas()
        {
            //  shakeAnim.DOPause();
            canvas.enabled = false;
            _isAnimated = false;
        }

        void EnableCanvas()
        {
            canvas.enabled = true;
        }
    }
}
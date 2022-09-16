using System.Collections.Generic;
using __PUBLISH_v1.Scripts;
using DG.Tweening;
using game.cards;
using game.managers;
using RengeGames.HealthBars;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace game.entertainment
{
    public class BonusCoins : MonoBehaviour
    {
        public bool useRadial;
        public UltimateCircularHealthBar circularBar;
        public CustomSlider slider;
        public CanvasGroup canvasGroup;
        public List<Image> coinsImages = new();
        [SerializeField] int minimumRow = 2;
        [SerializeField] TextMeshProUGUI txt;
        public int maxSegments;
        public float fadeTime = 0.2f;
        public float pumpTime = 0.4f;
        public float pumpSize = 1.5f;
        public float pumpSizeSmall = 1.2f;
        public float hideDelay = 0.5f;
        bool _isVisible;

        void Start()
        {
            canvasGroup.alpha = 0;
            slider.SetValue(0);
            if (useRadial)
            {
                circularBar.SetSegmentCount(maxSegments);
                circularBar.SetRemovedSegments(maxSegments);
            }

            Events.Instance.OnComboSuccess += OnComboSuccess;
            Events.Instance.OnComboBreak += OnComboBreak;
            DisableAll();
        }

        void DisableAll()
        {
            foreach (var coinsImage in coinsImages)
            {
                coinsImage.enabled = false;
            }
        }

        void OnComboBreak(int obj)
        {
            txt.text = "+0";
            if (useRadial) circularBar.SetRemovedSegments(maxSegments);
            slider.SetValue(0);

            _isVisible = false;
            canvasGroup.DOFade(0, fadeTime)
                .SetDelay(hideDelay);
            canvasGroup.transform.DOScale(0, pumpTime)
                .SetDelay(hideDelay);

            DisableAll();
        }


        void OnComboSuccess(int comboRowCount, Card card1, Card card2)
        {
            if (comboRowCount < minimumRow) return;
            if (!_isVisible)
            {
                _isVisible = true;
                canvasGroup.DOFade(1, fadeTime);
                canvasGroup.transform.DOScale(pumpSize, pumpTime / 2)
                    .OnComplete(()
                        => canvasGroup.transform.DOScale(1, pumpTime / 2));
            }
            else
            {
                canvasGroup.transform.DOScale(pumpSizeSmall, pumpTime / 2)
                    .OnComplete(()
                        => canvasGroup.transform.DOScale(1, pumpTime / 2));
            }

            if (useRadial)
            {
                var segments = maxSegments - comboRowCount;
                segments = Mathf.Clamp(segments, 0, maxSegments);
                circularBar.SetRemovedSegments(segments);
            }

            var fill = (float) comboRowCount / maxSegments;
            slider.SetValue(fill);

            var bonusCoin = (int) (comboRowCount * GameManager.Instance.Config.Settings.ComboBonusPerRow);
            txt.text = "+" + bonusCoin;

            for (int i = 0; i < coinsImages.Count; i++)
                coinsImages[i].enabled = i < bonusCoin;
        }
    }
}
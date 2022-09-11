using __PUBLISH_v1.Scripts;
using DG.Tweening;
using game.cards;
using game.managers;
using RengeGames.HealthBars;
using TMPro;
using UI;
using UnityEngine;

namespace game.entertainment
{
    public class BonusCoins : MonoBehaviour
    {
        public UltimateCircularHealthBar circularBar;
        public CustomSlider slider;
        public CanvasGroup canvasGroup;

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
            circularBar.SetSegmentCount(maxSegments);
            circularBar.SetRemovedSegments(maxSegments);
            Events.Instance.OnComboSuccess += OnComboSuccess;
            Events.Instance.OnComboBreak += OnComboBreak;
        }

        void OnComboBreak(int obj)
        {
            txt.text = "+0";
            circularBar.SetRemovedSegments(maxSegments);
            slider.SetValue(0);
    
            _isVisible = false;
            canvasGroup.DOFade(0, fadeTime)
                .SetDelay(hideDelay);
            canvasGroup.transform.DOScale(0, pumpTime)
                .SetDelay(hideDelay);
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
            
            var segments = maxSegments - comboRowCount;
            segments = Mathf.Clamp(segments, 0, maxSegments);
            circularBar.SetRemovedSegments(segments);
           
            var fill = (float) comboRowCount / maxSegments;
            slider.SetValue(fill);
            
            var bonusCoin = (int) (comboRowCount * GameManager.Instance.Config.Settings.ComboBonusPerRow);
            txt.text = "+" + bonusCoin;
        }
    }
}
using System.Collections.Generic;
using DamageNumbersPro;
using DG.Tweening;
using game.cards;
using game.managers;
using TMPro;
using UnityEngine;

namespace game.entertainment
{
    public class ComboMessages : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float messageShowTime;
        [SerializeField] [Range(1, 2)] float sizeMultiplier;
        [SerializeField] float animTime;

        [Header("Setup")]
        [SerializeField] DamageNumber floatNumber;
        [SerializeField] Vector3 floatOffset;
        [SerializeField] Canvas messageCanvas;
        [SerializeField] TextMeshProUGUI txt;
        [SerializeField] int minimumRow = 2;
        [SerializeField] bool keepMessages;
        [SerializeField] float messageLifeTime;
        [SerializeField] float keepLifeTime;
        [Header("DEBUG")]
        public List<DamageNumber> keepList = new();

        float _basicFontSize;
        float _showTimer;
        bool _isActive;
        float _basicSpeed, _basicMinX, _basicMaxX, _basicMinY, _basicMaxY;

        void Start()
        {
            Events.Instance.OnComboSuccess += Refresh;
            Events.Instance.OnComboBreak += OnBreak;
            Events.Instance.OnRoundWin += OnRoundWin;
            _basicFontSize = txt.fontSize;
            Disable();
        }

        void OnRoundWin(int totalPairs) => KillMessages();

        void OnBreak(int successRow) => KillMessages();

        void KillMessages()
        {
            foreach (var damageNumber in keepList) damageNumber.lifetime = 1f;
            keepList = new List<DamageNumber>();
        }

        void FixedUpdate()
        {
            if (!_isActive) return;

            if (_showTimer > 0)
                _showTimer -= Time.fixedDeltaTime;
            else
            {
                _showTimer = 0;
                Disable();
            }
        }

        void Refresh(int comboRowCount, Card card1, Card card2)
        {
            if (comboRowCount < minimumRow) return;
            var cardScale = card2.transform.localScale.x;

            var number2 = floatNumber.CreateNew(comboRowCount, card2.transform.position + floatOffset * cardScale);
            number2.lifetime = keepMessages ? keepLifeTime : messageLifeTime;
            if (keepMessages) keepList.Add(number2);

            ScaleBasic(number2, cardScale);
            RefreshText(comboRowCount);
            RefreshShowTimer();
            Enable();
            if (sizeMultiplier > 1)
            {
                txt.DOFontSize(_basicFontSize * sizeMultiplier, animTime / 2)
                    .OnComplete(() => txt.DOFontSize(_basicFontSize, animTime / 2));
            }
        }

        void ScaleBasic(DamageNumber dn, float cardScale)
        {
            var scale = MaxPointsScale(cardScale);
            dn.transform.localScale = new Vector3(scale, scale, scale);
            dn.lerpSettings.speed = _basicSpeed * cardScale;
            dn.lerpSettings.minX = _basicMinX * cardScale;
            dn.lerpSettings.maxX = _basicMaxX * cardScale;
            dn.lerpSettings.minY = _basicMinY * cardScale;
            dn.lerpSettings.maxY = _basicMaxY * cardScale;
        }

        [SerializeField] float maxPointsSize = 1;

        float MaxPointsScale(float cardScale)
            => maxPointsSize == 0
                ? cardScale
                : cardScale > maxPointsSize
                    ? maxPointsSize
                    : cardScale;

        void Enable()
        {
            _isActive = true;
            messageCanvas.enabled = true;
        }

        void Disable()
        {
            _isActive = false;
            messageCanvas.enabled = false;
        }

        void RefreshShowTimer() => _showTimer = messageShowTime;
        void RefreshText(int comboRowCount) => txt.text = comboRowCount.ToString();
    }
}
using System.Collections;
using __PUBLISH_v1.Scripts;
using DG.Tweening;
using game.managers;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace game.player
{
    public class ScoreUI : MonoBehaviour
    {
        enum TextChangeType
        {
            Instant,
            IncrementBy1
        }

        [Header("Settings")]
        [SerializeField] TextChangeType textChangeType;
        [SerializeField] TextChangeType comboTextChangeType;
        [SerializeField] Color flashColor;
        [SerializeField] Color flashComboColor;
        [SerializeField] float flashTime;
        [SerializeField] float flashSize;
        [Header("Setup")]
        [SerializeField] Score score;
        [SerializeField] TextMeshProUGUI txt;
        [SerializeField] Image coinImage;
        [Header("Animation setup")]
        [SerializeField] float incrementAnimationSpeed = 5;
        SoundData ScoreUpSound => GameManager.Instance.Config.Sound.ScoreUp;
        SoundData ScoreUpByComboSound => GameManager.Instance.Config.Sound.ScoreUpByCombo;

        int _totalPoints;
        Color _normalColor;
        bool _isDisable;
        float baseFontSize;

        public void Disable()
        {
            _isDisable = true;
            coinImage.enabled = false;
            txt.enabled = false;
        }

        void Start()
        {
            baseFontSize = txt.fontSize;
            _normalColor = txt.color;
            _totalPoints = score.TotalScore;
            upCounterText.enabled = false;
            RefreshText(0);

            Events.Instance.OnAddScore += OnAddScore;
            //     Events.Instance.OnRoundWin += OnRoundWin;
            // Events.Instance.OnPlayerTotalPointsChanged += TotalPoints;
        }

        // void OnRoundWin(int totalpair)
        // {
        //     upCounterTimer = upCounterCheckTime;
        // }

        bool onScoreUp;

        void OnAddScore(int earnedPoints, bool isCombo)
        {
            if (_isDisable) return;
            PlaySound(isCombo ? ScoreUpByComboSound : ScoreUpSound);
            Refresh(earnedPoints, isCombo);

            if (!useUpCounter) return;
            onScoreUp = true;
            upCounter += earnedPoints;
            if (!upCounterRun) StartCoroutine(UpCounter());
        }

        [Header("UpCounter")]
        public bool useUpCounter;
        public float upCounterCheckTime;
        public float upCounterDuration;
        public float upCounterPumpSize;
        public float upCounterPumpTime;
        public float upCounterFadeTime;
        public int upCounterMinValue;
        public SoundData upCounterSound;
        public TextMeshProUGUI upCounterText;
        [Header("DEBUG")]
        public bool upCounterRun;
        public int upCounter;
        public float upCounterTimer;

        IEnumerator UpCounter()
        {
            upCounterRun = true;
            onScoreUp = false;
            upCounterTimer = 0f;
            while (upCounterTimer < upCounterCheckTime)
            {
                upCounterTimer += Time.deltaTime;
                if (onScoreUp)
                {
                    onScoreUp = false;
                    upCounterTimer = 0f;
                }

                yield return null;
            }


            if (upCounter > upCounterMinValue)
            {
                PlaySound(upCounterSound);
                upCounterText.DOKill();
                upCounterText.enabled = true;
                upCounterText.color = upCounterColor;
                upCounterText.text = "+" + upCounter;
                upCounterText
                    .DOFade(1, upCounterPumpTime / 2);
                upCounterText.transform
                    .DOScale(upCounterPumpSize, upCounterPumpTime / 2)
                    .OnComplete(()
                        => upCounterText.transform.DOScale(1, upCounterPumpTime / 2));
                upCounterText
                    .DOFade(0, upCounterFadeTime)
                    .SetDelay(upCounterDuration)
                    .OnComplete(() => upCounterText.enabled = false);
            }

            upCounter = 0;
            upCounterRun = false;
        }

        public Color upCounterColor;

        void Refresh(int changeValue, bool isCombo)
        {
            if (isCombo)
            {
                switch (comboTextChangeType)
                {
                    case TextChangeType.Instant:
                        _totalPoints = score.TotalScore;
                        RefreshText(_totalPoints);
                        FlashAnimation(true);

                        break;
                    case TextChangeType.IncrementBy1:
                        AnimatePointsChange(changeValue, true);
                        FlashAnimation(false);

                        break;
                }
            }
            else
            {
                switch (textChangeType)
                {
                    case TextChangeType.Instant:
                        _totalPoints = score.TotalScore;
                        RefreshText(_totalPoints);
                        FlashAnimation(false);

                        break;
                    case TextChangeType.IncrementBy1:
                        AnimatePointsChange(changeValue, false);

                        break;
                }
            }
        }

        void AnimatePointsChange(int changeValue, bool isCombo)
        {
            if (changeValue > 0)
            {
                StartCoroutine(PointsIncrementAnimation(changeValue, isCombo));
            }
            else
            {
                RefreshText(changeValue);
            }
        }


        void FlashAnimation(bool isComboPoints)
        {
            var clr = isComboPoints ? flashComboColor : flashColor;
            var size = isComboPoints ? flashSize * 1.3f : flashSize;

            // txt.DOColor(clr, flashTime / 2)
            //     .OnComplete(()
            //         => txt
            //             .DOColor(_normalColor, flashTime / 2));

            // var fontSize = baseFontSize;
            // txt.DOFontSize(fontSize * size, flashTime / 2)
            //     .OnComplete(()
            //         => txt
            //             .DOFontSize(fontSize, flashTime / 2));

            coinImage.transform
                .DOScale(size, flashTime / 2)
                .OnComplete(()
                    => coinImage.transform
                        .DOScale(1, flashTime / 2));
        }

        IEnumerator PointsIncrementAnimation(int changeValue, bool isCombo)
        {
            var incValue = 0;
            var animTime = 1 / incrementAnimationSpeed;

            while (incValue < changeValue)
            {
                incValue++;
                _totalPoints++;
                RefreshText(_totalPoints);

                var clr = isCombo ? flashComboColor : flashColor;
                txt.color = clr;

                yield return new WaitForSeconds(animTime);
            }

            txt.color = _normalColor;
        }


        void RefreshText(int total) => txt.text = total.ToString();

        void PlaySound(SoundData sound) => AudioManager.Instance.PlaySound(sound);
    }
}
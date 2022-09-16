using System;
using DG.Tweening;
using game.managers;
using TMPro;
using UI;
using UnityEngine;

namespace game.sessions.timer
{
    public class TimerUI : MonoBehaviour
    {
        [Header("Values")]
        public bool useValuesText;
        public TextMeshProUGUI txt;
        [Header("Slider")]
        public bool useSlider;
        public CustomSlider slider;
        [Header("Animation")]
        public float pumpSize;
        public float pumpTime;
       
        int _lastUpdateSec;
        float _secondsMax;
        
        void Awake() => DisableUI();

        void Start()
        {
            Events.Instance.OnTimerEnable += EnableUI;
            Events.Instance.OnTimerDisable += DisableUI;
            Events.Instance.OnTimerUpdate += OnUpdate;
            Events.Instance.OnTimerContinue += OnTimerContinue;
            Events.Instance.OnSetTimer += OnSetTimer;
            if (!useValuesText) txt.gameObject.SetActive(false);
            if (!useSlider) slider.gameObject.SetActive(false);
        }

        void OnSetTimer(int sec) => RefreshText(sec);

        void OnTimerContinue()
        {
            txt.transform.DOScale(pumpSize, pumpTime / 2)
                .OnComplete(()
                    => txt.transform.DOScale(1, pumpTime / 2));
        }



        void FixedUpdate()
        {
            if (useValuesText)
            {
                var remain = Mathf.RoundToInt(Timer.Instance.Seconds);
                if (_lastUpdateSec != remain)
                {
                    _lastUpdateSec = remain;
                    RefreshText(remain);
                }
            }

            if (!useSlider) return;
            if (Timer.Instance.Seconds != 0) RefreshSlider(Timer.Instance.Seconds);
        }

        void OnUpdate(int remain)
        {
            //  Debug.LogError("TIMER");
            if (remain > _secondsMax) _secondsMax = remain;
            if (useValuesText) RefreshText(remain);
            //if (useSlider) RefreshSlider(secondsRemainng);
        }

        void EnableUI()
        {
            slider.gameObject.SetActive(true);
            slider.transform
                .DOScale(pumpSize, pumpTime / 2)
                .OnComplete(() => slider.transform.DOScale(1, pumpTime / 2));
        }


        void DisableUI() => slider.gameObject.SetActive(false);


        void RefreshText(int seconds) => txt.text = seconds.ToString();
        //  void RefreshText(int seconds) => txt.text = Format0000(seconds); 

        void RefreshSlider(float secondsRemaining)
        {
            if (secondsRemaining > _secondsMax) _secondsMax = secondsRemaining;
            var sliderValue = secondsRemaining / _secondsMax;
            slider.SetValue(sliderValue);
        }

        string Format0000(int seconds)
        {
            var minutes = seconds < 60 ? 0 : seconds / 60;
            var minStr = minutes.ToString();
            var minutesString = minutes < 10 ? "0" + minStr : minStr;

            var sec = seconds < 60 ? seconds : seconds % 60;
            var secStr = sec.ToString();
            var secondsString = sec < 10 ? "0" + secStr : secStr;

            var resultString = minutesString + ":" + secondsString;
            return resultString;
        }
    }
}
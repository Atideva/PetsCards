using System;
using System.Collections;
using __PUBLISH_v1.Scripts;
using DG.Tweening;
using game.cards;
using game.cards.managers;
using game.managers;
using game.player;
using game.sessions.timer;
using UnityEngine;

namespace game.levels
{
    public class WinLose : MonoBehaviour
    {
        [Header("Indicators")]
        [SerializeField] int winPoints;
        [SerializeField] int winPointsRequired;
        RuntimeData _runtimeData;

        bool _isFlip;
        float _flipTime;

        void Start()
        {
            _runtimeData = GameManager.Instance.Config.RuntimeData;
            Events.Instance.OnWinPointRequest += OnWinPointRequest;
            Events.Instance.OnWinPointEarn += OnWinPointEarn;
            Events.Instance.OnTimeOver += OnTimeOver;
            Events.Instance.OnNoMoreLives += OnNoMoreLives;
            Events.Instance.OnTimerState += OnTimerState;
        }


        void OnWinPointRequest()
        {
            winPointsRequired++;
            RuntimeData();
        }

        void OnWinPointEarn()
        {
            winPoints++;
            RuntimeData();
            WinCheck();
        }

        void WinCheck()
        {
            if (winPoints >= winPointsRequired)
                Win();
        }

        void OnNoMoreLives()
            => Lose();

        void OnTimeOver()
        {
            Turns.Instance.GameOver();
            //    Invoke(nameof(CheckTimeRemain), 2f); //let good abilities be done
            if (_isFlip)
                Invoke(nameof(CheckTimer), Flipper.Instance.RotationTime); //let cars finish rotate
            else
                Lose();
        }

        void OnTimerState(TimerState state)
        {
            switch (state)
            {
                case TimerState.Critical:
                    Events.Instance.OnFlip += OnFlip;
                    break;
                case TimerState.Normal:
                    Events.Instance.OnFlip -= OnFlip;
                    break;
                case TimerState.Low:
                    break;
            }
        }

        void OnFlip(Card arg1, bool arg2, Ease arg3, float arg4)
        {
            _flipTime = Flipper.Instance.RotationTime;

            if (!_isFlip)
            {
                _isFlip = true;
                StartCoroutine(CheckRoutine());
            }
        }


        IEnumerator CheckRoutine()
        {
            while (_flipTime > 0)
            {
                _flipTime -= Time.deltaTime;
                yield return null;
            }

            _isFlip = false;
        }


        void CheckTimer()
        {
            if (Timer.Instance.Seconds <= 0)
                Lose();
            else
                Turns.Instance.GameAreNotOver();
        }

        void Win()
        {
            GameManager.Instance.LevelComplete();
            Events.Instance.Win();
        }

        void Lose() => Events.Instance.Lose();


        void RuntimeData()
        {
            if (!_runtimeData) return;
            _runtimeData.WinPointsData.points = winPoints;
            _runtimeData.WinPointsData.require = winPointsRequired;
        }
    }
}
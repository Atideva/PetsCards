using __PUBLISH_v1.Scripts;
using game.cards;
using game.managers;
using IngameDebugConsole;
using UnityEngine;

namespace game.sessions.timer
{
    public class TimerHandler : MonoBehaviour
    {
        float _perPair;
        bool _isGameStarted;
        bool _isSubscribed;

        void Start()
        {
            _perPair = GameManager.Instance.Config.Settings.SecondsPerPair /
                       GameManager.Instance.CurrentLevel.Difficulty;

            Events.Instance.OnTimerEnable += Enable;
            Events.Instance.OnSessionPairStart += OnRoundStart;
            Events.Instance.OnRoundWin += OnRoundWin;
            Events.Instance.OnAbilityUse += OnAbilityUse;
            Events.Instance.OnAbilityFinish += OnAbilityFinish;
        }

        void OnAbilityFinish(Card c1, Card c2)
        {
            if (!_isEnable) return;
            Continue();
        }

        void OnAbilityUse(AbilityConfig ac, Card c1, Card c2)
        {
            if (!_isEnable) return;
            Pause();
        }


        bool _isEnable;

        void Enable()
        {
            _isEnable = true;
            if (!_isSubscribed)
            {
                _isSubscribed = true;
                Events.Instance.OnFlipEnd += OnFlipEnd;
            }
        }

        void OnFlipEnd(Card card)
        {
            Events.Instance.OnFlipEnd -= OnFlipEnd;
            _isGameStarted = true;
            Continue();
        }


        void OnRoundStart(int totalPairs)
        {
            var seconds = (int) (totalPairs * _perPair);
           // AddTime(seconds);
            SetTimer(seconds);
            Events.Instance.OnFlipEnd += OnFlipEnd;
        }
        
        void OnRoundWin(int totalPairs) => Pause();

        void AddTime(int seconds) => Events.Instance.TimerAdd(seconds);
        void SetTimer(int seconds) => Events.Instance.SetTimer(seconds);
        void Continue() => Events.Instance.ContinueTimer();
        void Pause() => Events.Instance.PauseTimer();
    }
}
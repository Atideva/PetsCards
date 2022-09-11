using __PUBLISH_v1.Scripts;
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

        void Start()
        {
            _runtimeData = GameManager.Instance.Config.RuntimeData;
            Events.Instance.OnWinPointRequest += OnWinPointRequest;
            Events.Instance.OnWinPointEarn += OnWinPointEarn;
            Events.Instance.OnTimeOver += OnTimeOver;
            Events.Instance.OnNoMoreLives += OnNoMoreLives;
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
            {
                Win();
            }
        }


        void OnNoMoreLives() => Lose();

        void OnTimeOver()
        {
            Turns.Instance.GameOver();
            Invoke(nameof(CheckTimeRemain), 2f);
        }

        void CheckTimeRemain()
        {
            if (Timer.Instance.Seconds <= 0)
            {
                Lose();
            }
            else
            {
                Turns.Instance.GameAreNotOver();
            }
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
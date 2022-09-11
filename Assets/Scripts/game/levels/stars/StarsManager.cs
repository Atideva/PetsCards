using game.managers;
using UnityEngine;

namespace game.levels.stars
{
    public class StarsManager : MonoBehaviour
    {
        #region Singleton
        //-------------------------------------------------------------
        public static StarsManager Instance;
        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);
        }
        //-------------------------------------------------------------
        #endregion

        int _starsEarned, _secondsRemaining;
        StarsConfig _thisLevel;
        bool _inited;


        public void Init()
        {
            if (_inited) return;
            _inited = true;

            _starsEarned = StarsSave.Instance.Get_StarsEarnedForThisLevel();
            if (_starsEarned == -1)
            {
                Debug.LogWarning("No such level in data");
                return; //no such level in data
            }
            if (_starsEarned < 3)
            {
                _thisLevel = StarsConfig.Instance;
                WinConditionType(StarsConfig.Instance.StarsRequirement);
            }
        }

        void WinConditionType(StarsRequirement winRequirement)
        {
            switch (winRequirement)
            {
                case StarsRequirement.Points:
                    StarsForGamePoints();
                    break;
                case StarsRequirement.TimeRemaining:
                    StartForTimeRemain();
                    break;
                default:
                    break;
            }
        }
        void StarsForGamePoints()
        {
            Events.Instance.OnScoreChange += Score;
        }
        void StartForTimeRemain()
        {
            Events.Instance.OnTimerUpdate += TimeRemaining;
            Events.Instance.OnWin += Win;
        }



        void Win()
        {
            if (_secondsRemaining >= _thisLevel.timeRemainingForThreeStars)
            {
                if (_starsEarned == 2)
                {
                    SetStars(3);
                    return;
                }
            }
            if (_secondsRemaining >= _thisLevel.timeRemainingForTwoStars)
            {
                if (_starsEarned == 1)
                {
                    SetStars(2);
                    return;
                }
            }
            if (_secondsRemaining >= _thisLevel.timeRemainingForOneStar)
            {
                if (_starsEarned == 0)
                {
                    SetStars(1);
                    return;
                }
            }
        }
        void TimeRemaining(int secondsRemaining) => this._secondsRemaining = secondsRemaining;
        void Score(int totalPoints)
        {
            if (_starsEarned == 3)
            {
                Events.Instance.OnScoreChange -= Score;
                return;
            }

            if (totalPoints >= _thisLevel.pointsRequiredForThreeStars)
            {
                if (_starsEarned == 2)
                {
                    SetStars(3);
                    return;
                }
            }
            if (totalPoints >= _thisLevel.pointsRequiredForTwoStars)
            {
                if (_starsEarned == 1)
                {
                    SetStars(2);
                    return;
                }
            }
            if (totalPoints >= _thisLevel.pointsRequiredForOneStar)
            {
                if (_starsEarned == 0)
                {
                    SetStars(1);
                    return;
                }
            }
        }



        void SetStars(int count)
        {
            _starsEarned = count;
            StarsSave.Instance.Set_StarsEarnedForThisLevel(_starsEarned);
        }

    }
}

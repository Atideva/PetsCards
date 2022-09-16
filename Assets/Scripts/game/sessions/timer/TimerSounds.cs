using Analytics;
using game.managers;
using UnityEngine;

namespace game.sessions.timer
{
    public class TimerSounds : MonoBehaviour
    {
        public GameObject timeLowSound;
        public GameObject timeCriticalSound;

        void Awake() => Mute();


        void Start()
        {
            Events.Instance.OnTimerState += OnState;
            Events.Instance.OnTimerPause += Mute;
            Events.Instance.OnLose += Mute;
            Events.Instance.OnTimeOver += Mute;
        }

        void OnState(TimerState state)
        {
            switch (state)
            {
                case TimerState.Low:
                    Low();
                    break;
                case TimerState.Critical:
                    Critical();
                    break;
                case TimerState.Normal:
                    Mute();
                    break;
            }
        }


        void Mute()
        {
            timeLowSound.SetActive(false);
            timeCriticalSound.SetActive(false);
        }

        void Low()
        {
            timeLowSound.SetActive(true);
            timeCriticalSound.SetActive(false);
        }

        void Critical()
        {
            timeLowSound.SetActive(false);
            timeCriticalSound.SetActive(true);
        }
    }
}
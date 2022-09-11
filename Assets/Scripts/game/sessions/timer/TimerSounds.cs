using game.managers;
using UnityEngine;

namespace game.sessions.timer
{
    public class TimerSounds : MonoBehaviour
    {

        public GameObject timeLowSound;
        public GameObject timeCriticalSound;

        void Awake() => StopAllSounds();


        void Start()
        {
            Events.Instance.OnTimerState += State;
            Events.Instance.OnTimerPause += Pause;
        }


        void Pause() => StopAllSounds();
        void State(TimerState state)
        {
            if (state == TimerState.Low)
            {
                SoundLowTime();
            }
            if (state == TimerState.Critical)
            {
                SoundCriticalTime();
            }
            if (state == TimerState.Normal)
            {
                StopAllSounds();
            }
        }



        void StopAllSounds()
        {
            timeLowSound.SetActive(false);
            timeCriticalSound.SetActive(false);
        }
        void SoundLowTime()
        {
            timeLowSound.SetActive(true);
            timeCriticalSound.SetActive(false);
        }
        void SoundCriticalTime()
        {
            timeLowSound.SetActive(false);
            timeCriticalSound.SetActive(true);
        }

    }
}

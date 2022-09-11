using game.managers;
using UnityEngine;

namespace game.sessions.timer
{
    public class Timer : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static Timer Instance;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);
        }

        //-------------------------------------------------------------

        #endregion

        [Header("Settings")]
        public float timeLowThreshold;
        public float timeCriticalThreshold;

        [Header("DEBUG")]
        bool _enable;
        public bool _isPaused = true;
        [field: SerializeField] public float Seconds { get; private set; }

        TimerState _state = TimerState.Normal;

        void Start()
        {
            Events.Instance.OnTimerEnable += Enable;
            Events.Instance.OnTimerDisable += Disable;
            Events.Instance.OnTimerContinue += Continue;
            Events.Instance.OnTimerPause += Pause;
        }

        void Update()
        {
            if (_isPaused) return;

            if (Seconds > 0)
                Seconds -= Time.deltaTime;
            else
                TimeOver();

            CheckState();
        }

        bool _isSubscribed;

        void Subscribe(bool subscribe)
        {
            if (subscribe)
            {
                if (_isSubscribed) return;

                _isSubscribed = true;
                Events.Instance.OnSetTimer += Set;
                Events.Instance.OnTimerAdd += Add;
                Events.Instance.OnTimerSubtract += Subtract;
            }
            else
            {
                _isSubscribed = false;
                Events.Instance.OnSetTimer -= Set;
                Events.Instance.OnTimerAdd -= Add;
                Events.Instance.OnTimerSubtract -= Subtract;
            }
        }

        void Set(int sec)
        {
            Seconds = sec;
            OnChange();
        }

        void Enable()
        {
            _enable = true;
            Subscribe(true);
            OnChange();
        }

        void Disable()
        {
            _enable = false;
            Subscribe(false);
            Pause();
        }

        void CheckState()
        {
            var stateNew = GetState(Seconds);
            if (_state == stateNew) return;

            _state = stateNew;
            Events.Instance.TimerStateChange(stateNew);
        }

        TimerState GetState(float sec) =>
            sec < timeCriticalThreshold
                ? TimerState.Critical
                : sec < timeLowThreshold
                    ? TimerState.Low
                    : TimerState.Normal;

        void Continue()
        {
            if (!_enable) return;
            _isPaused = false;
            OnChange();
        }

        void Pause() => _isPaused = true;

        void TimeOver()
        {
            Pause();
            Events.Instance.TimeOver();
        }


        void Add(int value) => Change(value);

        void Subtract(int value) => Change(-value);

        void Change(int value)
        {
            Seconds += value;
            OnChange();
        }

        void OnChange() => Events.Instance.TimerUpdate((int) Seconds);
    }
}
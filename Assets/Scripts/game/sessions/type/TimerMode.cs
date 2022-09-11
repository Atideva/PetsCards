using game.managers;
using UnityEngine;

namespace game.sessions.type
{
    [ExecuteInEditMode]
    public class TimerMode : MonoBehaviour, ISession
    {
        #region EDITOR RENAME

#if UNITY_EDITOR
        string _defaultName;
        void Start() => _defaultName = "Timer";

        void LateUpdate()
        {
            gameObject.name = _defaultName + ": " + NameSuffix;
        }

        string NameSuffix => timerAction.ToString();
#endif

        #endregion


        public TimerActions timerAction;

        public enum TimerActions
        {
            Enable,
            Disable,
            Start,
            Pause
        }



        public void StartSession()
        {
            DoTimer();
            Events.Instance.SessionComplete();
        }

        public void Request()
        {
        }



        void DoTimer()
        {
            switch (timerAction)
            {
                case TimerActions.Enable:
                    Events.Instance.EnableTimer();
                    break;
                case TimerActions.Disable:
                    Events.Instance.DisableTimer();
                    break;
                case TimerActions.Start:
                    Events.Instance.ContinueTimer();
                    break;
                case TimerActions.Pause:
                    Events.Instance.PauseTimer();
                    break;
            }
        }
        
    }
}
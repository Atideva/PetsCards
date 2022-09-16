using System.Collections;
using game.managers;
using UnityEngine;

namespace game.sessions.timer
{
    public class TimerAnimations : MonoBehaviour
    {
        public Transform shakeObject;
        public float shakeBy = 0.2f;
        public float speed;
        bool _way;
        void Start()
        {
            Events.Instance.OnTimerState += OnState;
            Events.Instance.OnTimerPause += Stop;
            Events.Instance.OnTimeOver += Stop;
        }
        float _size = 1f;
 
        void OnState(TimerState state)
        {
            switch (state)
            {
                case TimerState.Low:
                    break;
                case TimerState.Critical:
                    StartCoroutine(ShakeTransform());
                    break;
                case TimerState.Normal:
                    Stop();
                    break;
            }
        }
        void Stop()
        {
            StopAllCoroutines();
            _size = 1;
            shakeObject.localScale = new Vector3(_size, _size, _size);
        }
        IEnumerator ShakeTransform()
        {
            while (true)
            {
                float dir = _way ? 1 : -1;
                _size += Time.deltaTime * speed * dir;

                if (_way)
                {
                    if (_size > (1 + shakeBy)) _way = !_way;
                }
                else
                {
                    if (_size < (1 - shakeBy)) _way = !_way;
                }

                shakeObject.localScale = new Vector3(_size, _size, _size);
                yield return null;
            }
        }
    }
}

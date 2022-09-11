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
            Events.Instance.OnTimerState += State;
            Events.Instance.OnTimerPause += Pause;
        }
        float _size = 1f;
        void Pause() => StopShake();
        void State(TimerState state)
        {
            if (state == TimerState.Low)
            {

            }
            if (state == TimerState.Critical)
            {
                StartCoroutine(ShakeTransform());
            }
            if (state == TimerState.Normal)
            {
                StopShake();
            }
        }
        void StopShake()
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

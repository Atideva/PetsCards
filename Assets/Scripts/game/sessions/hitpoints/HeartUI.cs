using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace game.sessions.hitpoints
{
    public class HeartUI : MonoBehaviour
    {
        public Image heart;
        RectTransform _rect;
        public RectTransform Rect { get => _rect; }
        public bool IsAlive { get => _isAlive;  }
        bool _isAlive=true;
        void Start() => _rect = GetComponent<RectTransform>();


        public void HeartDead()
        {
            _isAlive = false;
            heart.gameObject.SetActive(false);
        }
        public void HeartRestored()
        {
            _isAlive = true;
            heart.gameObject.SetActive(true);
            StartCoroutine(SimpleFillHeart());
        }
        IEnumerator SimpleFillHeart()
        {
            var value = 0f;
            var speed = 1f;
            while (value < 1)
            {
                value += Time.deltaTime * speed;
                heart.fillAmount = value;
                yield return null;
            }
            heart.fillAmount = 1;
        }
        public void HeartFill(float value) => heart.fillAmount = value;
    }
}

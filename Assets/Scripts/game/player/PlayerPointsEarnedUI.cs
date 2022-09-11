using game.managers;
using TMPro;
using UnityEngine;

namespace game.player
{
    public class PlayerPointsEarnedUI : MonoBehaviour
    {

        [Header("Settings")]
        public float lifeTime = 0.5f;
        float _timer;


        [Header("Setup")]
        public GameObject messageCanvas;
        public GameObject messageObject;
        public TextMeshProUGUI txt;
        bool _isActive;


        void Start()
        {
            Events.Instance.OnAddScore += EarnAddScore;

            Disable();
        }

        void FixedUpdate()
        {
            if (!_isActive) return;
            if (_timer > 0)
            {
                _timer -= Time.fixedDeltaTime;
            }
            else
            {
                Disable();
            }
        }


        void Enable()
        {
            _isActive = true;
            _timer = lifeTime;
            messageCanvas.SetActive(true);
        }

        void Disable()
        {
            _isActive = false;
            _timer = 0;
            messageCanvas.SetActive(false);
        }


        void EarnAddScore(int earnedPoints,bool isCombo)
        {
            RefreshText(earnedPoints);
            Enable();
        }


        void RefreshText(int earnedPoints) => txt.text = TextFormat(earnedPoints);
        string TextFormat(int earnedPoints) => "+" + earnedPoints;
    }
}
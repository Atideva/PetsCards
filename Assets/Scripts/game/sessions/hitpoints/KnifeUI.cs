using game.managers;
using UnityEngine;
using UnityEngine.UI;

namespace game.sessions.hitpoints
{
    public class KnifeUI : MonoBehaviour
    {
        public GameObject knifeObject;
        public Image knifeImage;
        public Image knifeBackImage;
        public float moveSpeed;
        public bool move;
        RectTransform _knifeRect;
        public float startX, startY;
        float _x, _y, _endX, _endY;
        public Vector2 knifeSize;
        public HeartControllerUI heartsUI;
        public LivesUI hpUIcontroller;
        public Lives hpScript;

        void Start()
        {
            Debug.Log("Knife mode is disabled by coder");
            //if (hpUIcontroller.knifeMode)
            //{
            //    HpShield(hpScript.ShieldMax);
            //    KnifeMode();
            //    knifeObject.SetActive(true);
            //}
            //else
            //{
            knifeObject.SetActive(false);
            //}
        }

        void HpShield(int shieldMax)
        {
            _hpShieldMax = shieldMax;
            _hpShield = shieldMax;
        }

        int _hpShield, _hpShieldMax;
        void HpShieldChanged(int shieldCurrent)
        {
            _hpShield = shieldCurrent;

            float curent = (float)_hpShield;
            float max = (float)_hpShieldMax;
            float fillValue = 1 - curent / max;

            fillValueDestanation = fillValue;
  
        }
        public float fillValueDestanation;
        public float fillKnifeSpeed = 0.5f;
        void KnifeMode()
        {
            Debug.Log("Knife ui enabled");
            knifeObject.SetActive(true);
            _knifeRect = knifeObject.GetComponent<RectTransform>();
            startX = _knifeRect.localPosition.x;
            startY = _knifeRect.localPosition.y;
            _x = startX;
            _y = startY;
            Events.Instance.OnHitpointsLost += HpLost;
            Events.Instance.OnHitpointsShieldChanged += HpShieldChanged;
        }

        void HpLost(int lostValue)
        {
            Debug.LogError("Nofin yet here, because of unused system");
            //var heartToDestroy = heartsUI.hearts[heartsUI.UiHP - 1];
            //endX = heartToDestroy.Rect.localPosition.x;
            //endY = heartToDestroy.Rect.localPosition.y;
            //move = true;
        }

        void Update()
        {
            if (move)
            {
                _x -= moveSpeed * Time.deltaTime;
                _knifeRect.localPosition = new Vector2(_x, _y);
                if (_x <= (_endX + knifeSize.x))
                {
                    //Debug.LogError(x + " (x), (endX) " + endX);
                    move = false;
                    Debug.LogError("Nofin yet here, because of unused system");
                    //heartsUI.KillHeart();
                    fillValueDestanation = 0;
                    Invoke(nameof(RefreshKnife), 0.6f);
                }
            }
            if(knifeImage.fillAmount< fillValueDestanation)
            {
                knifeImage.fillAmount += fillKnifeSpeed * Time.deltaTime;
            }
        }
        void RefreshKnife()
        {
            _x = startX;
            _y = startY;
            _knifeRect.localPosition = new Vector2(_x, _y);
            knifeImage.fillAmount = fillValueDestanation;
        }
    }
}

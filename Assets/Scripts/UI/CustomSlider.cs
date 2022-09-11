using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [ExecuteInEditMode]
    public class CustomSlider : MonoBehaviour
    {
        [Header("VALUE")]
        [SerializeField] [Range(0, 1)] float sliderValue;
        [Header("Settings")]
        [SerializeField]  bool useGradient;
        [SerializeField]  Gradient gradient;
        [Header("Setup")]
        [SerializeField]  Image fill;
        [SerializeField]  Image fillShadow;
        [Header("Colors")]
        [SerializeField]  Color shadowMinusColor;
        [SerializeField]  Color shadowPlusColor;
        [Header("Shadow")]
        [SerializeField]  bool useShadow;
        [SerializeField]  float shadowThreshold = 0.05f;
        [SerializeField]  float shadowCatchSpeed = 0.05f;

        [Header("TEST")]
        public bool testPlus;
        public bool testMinus;
        ShadowMoveType _moveType;
        bool _isMove;
        public float Value { get; private set; }

        public enum ShadowMoveType
        {
            SliderMove,
            ShadowMove
        }

        public void AddValue(float value)
        {
            var newValue = Value += value;
            SetValue(newValue);
        }
        public void SetValue(float value)
        {
            Value = value;
            if (useShadow)
            {
                if (!_isMove)
                {
                    var gap = value - sliderValue;
                    if (Mathf.Abs(gap) < shadowThreshold)
                    {
                        FillInstant(value);
                    }
                    else
                    {
                        FillShadow(value, gap);
                    }
                }
                else
                {
                    var dif = _moveType == ShadowMoveType.SliderMove
                        ? value - fill.fillAmount
                        : value - fillShadow.fillAmount;
                    FillShadow(value, dif);
                }
            }
            else
            {
                FillInstant(value);
            }

            sliderValue = value;
            sliderValue = Mathf.Clamp01(sliderValue);
        }

        void FillShadow(float value, float dif)
        {
            _isMove = true;
            fillShadow.gameObject.SetActive(true);
            if (dif > 0)
            {
                //++
                fillShadow.color = shadowPlusColor;
                fillShadow.fillAmount = value;
                _moveType = ShadowMoveType.SliderMove;
            }
            else
            {
                //--
                fillShadow.color = shadowMinusColor;
                fill.fillAmount = value;
                _moveType = ShadowMoveType.ShadowMove;
            }
        }

        void FillInstant(float value)
        {
            fill.fillAmount = value;
            fillShadow.fillAmount = value;
            RefreshColor();
        }


        void RefreshColor()
        {
            if (useGradient)
                fill.color = gradient.Evaluate(fill.fillAmount);
        }


        void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                fill.fillAmount = sliderValue;
                RefreshColor();
            }


            if (testPlus)
            {
                testPlus = false;
                SetValue(sliderValue + 0.2f);
            }

            if (testMinus)
            {
                testMinus = false;
                SetValue(sliderValue - 0.2f);
            }

#endif

            if (_isMove)
            {
                var gap = fillShadow.fillAmount - fill.fillAmount;

                if (_moveType == ShadowMoveType.SliderMove)
                {
                    if (Mathf.Abs(gap) > 0.02f)
                    {
                        float dir = gap > 0 ? 1 : -1;
                        fill.fillAmount += dir * shadowCatchSpeed * Time.deltaTime;
                        fillShadow.fillAmount = sliderValue;
                    }
                    else
                    {
                        _isMove = false;
                        fill.fillAmount = sliderValue;
                        fillShadow.gameObject.SetActive(false);
                    }

                    RefreshColor();
                }

                if (_moveType == ShadowMoveType.ShadowMove)
                {
                    if (Mathf.Abs(gap) > 0.02f)
                    {
                        float dir = gap > 0 ? -1 : 1;
                        fillShadow.fillAmount += dir * shadowCatchSpeed * Time.deltaTime;
                        fill.fillAmount = sliderValue;
                        RefreshColor();
                    }
                    else
                    {
                        _isMove = false;
                        fillShadow.fillAmount = sliderValue;
                        fillShadow.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
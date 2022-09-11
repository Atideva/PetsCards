using DG.Tweening;
using UnityEngine;

namespace __PUBLISH_v1.Scripts
{
    [ExecuteInEditMode]
    public class BlindCutout : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] Canvas canvas;
        [SerializeField] CanvasGroup cutoutGroup;
        [SerializeField] CanvasGroup cutoutBackGroup;
        [Header("Size")]
        [SerializeField] float cutoutMinSize;
        [SerializeField] float cutoutMaxSize;
        [SerializeField] float cutoutScaleDuration = 1;
        [SerializeField] float screenFadeDuration;
        [Header("Show")]
        [SerializeField] float cutoutShowDelay;
        [SerializeField] float cutoutBackShowDelay;
        [SerializeField] float cutoutBackShowDuration;
        [SerializeField] float screenShowDelay;
        [Header("Hide")]
        [SerializeField] float cutoutHideDelay;
        [SerializeField] float cutoutBackHideDelay;
        [SerializeField] float cutoutBackHideDuration;
        [SerializeField] float screenHideDelay;
        public float ShowTime => cutoutBackShowDuration;
        [Header("Test")]
        public bool testHide;
        public bool testShow;
        [SerializeField] AudioClip testSound;

        void Awake()
        {
            cutoutGroup.transform.localScale = new Vector3(cutoutMaxSize, cutoutMaxSize, cutoutMaxSize);
            cutoutBackGroup.transform.localScale = new Vector3(cutoutMinSize, cutoutMinSize, cutoutMinSize);
            Disable();
        }

        void Disable()
        {
            canvas.enabled = false;
            cutoutGroup.alpha = 0;
            cutoutBackGroup.alpha = 0;
        }

        public void Show()
        {
            canvas.enabled = true;
            cutoutBackGroup
                .DOFade(1, cutoutBackShowDuration)
                .SetDelay(cutoutBackShowDelay);
            cutoutGroup.transform.DOLocalRotate(new Vector3(0, 0, rotateAngle), cutoutScaleDuration).SetRelative(true);
            cutoutGroup.transform
                .DOScale(cutoutMinSize, cutoutScaleDuration)
                .SetDelay(cutoutShowDelay);
            cutoutGroup
                .DOFade(1, screenFadeDuration)
                .SetDelay(screenShowDelay);
        }

        public float rotateAngle = 360;
        public void Hide()
        {
            KillTween();
            UnityMagic();

            cutoutBackGroup
                .DOFade(0, cutoutBackHideDuration)
                .SetDelay(cutoutBackHideDelay);
            cutoutGroup.transform
                .DOScale(cutoutMaxSize, cutoutScaleDuration)
                .SetDelay(cutoutHideDelay);
            cutoutGroup
                .DOFade(0, screenFadeDuration)
                .SetDelay(screenHideDelay)
                .OnComplete(Disable);
        }

        void KillTween()
        {
            cutoutBackGroup.DOKill();
            cutoutGroup.DOKill();
        }

        void UnityMagic()
        {
            canvas.enabled = false;
            canvas.enabled = true;
        }

#if UNITY_EDITOR
        void Update()
        {
            if (testHide)
            {
                testHide = false;
                Hide();
            }

            if (testShow)
            {
                testShow = false;
                var go = new GameObject();
                var asc = go.AddComponent<AudioSource>();

                asc.clip = testSound;
                asc.Play();
                Show();
            }
        }
#endif
    }
}
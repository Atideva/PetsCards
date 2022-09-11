using System.Collections;
using __PUBLISH_v1.Scripts;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using game.managers;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;

namespace game.cards.managers
{
    public class Flipper : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static Flipper Instance;



        void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Debug.LogError("Did you open it in prefab mode, or there's really 2 instanses of Singleton here?",
                    gameObject);
                //gameObject.SetActive(false);
            }
        }

        //-------------------------------------------------------------

        #endregion

        public enum FlipDirection
        {
            Left,
            Right
        }

        [Header("Speed")]
        [SerializeField] bool flipCardImage;
        [SerializeField] float rotationTime = 0.65f;
        [SerializeField] float rotationTimeBack = 1f;
        [SerializeField] float rotationSize = 1.1f;
        [SerializeField] Vector3 flipOffset;
        [SerializeField] Vector3 flipBackOffset;
        [Range(0, 1)]
        [SerializeField] float randomMultiplier = 0.2f;
        [Header("Ease")]
        [SerializeField] Ease easeType;
        [SerializeField] Ease easeTypeBack;
        [Header("Directions")]
        [SerializeField] FlipDirection flipDirection;
        [SerializeField] FlipDirection flipBackDirection;
        [Header("Half rotation triggers")]
        [Range(0, 1)] public float flipFrontTrigger;
        [Range(0, 1)] public float flipBackTrigger;
        SoundData FlipSound => GameManager.Instance.Config.Sound.CardFlip;
        SoundData FlipBackSound => GameManager.Instance.Config.Sound.CardFlipBack;

        Vector3 RotateScaleUp(float baseScale) =>
            new(baseScale * rotationSize, baseScale * rotationSize, baseScale * rotationSize);

        public float RotationTime => rotationTime;

        public float RotationTimeBack => rotationTimeBack;


        const float ANGLE_ROT_MAX = 180;

        void Start()
        {
            Events.Instance.OnFlip += DoFlip;
            Events.Instance.OnFlipBack += DoFlipBack;
        }


        void DoFlip(Card card, bool isRandomTime, Ease customEase, float customTime)
        {
            PlaySound(FlipSound);
            SetCardOrientation(card, CardOrientation.IsFlip);

            var rotateVector = GetRotateVector(flipDirection);
            var time = customTime < 0 ? RotationTime : customTime;

            if (isRandomTime)
            {
                time = RandomTime(time);
            }


            var pos = card.transform.position+flipOffset;
            var offset = new Vector3(0, card.transform.localScale.y / 15f, 0);
            var ease = customEase == Ease.Unset ? easeType : customEase;

            card.transform
                .DOMove(pos + offset, time / 2)
                .OnComplete(() => card.transform.DOMove(pos, time / 2));

            DoRotate(card.gameObject, rotateVector, time, ease)
                .OnComplete(() => FlipEnd(card));

            DoScale(card.gameObject, time);

            if (flipCardImage) StartCoroutine(FlipImage(card.MainArt, time * flipFrontTrigger));
            if (GameManager.Instance.IsPvP) StartCoroutine(ShowDoubleCard(card, time * flipFrontTrigger));
        }


        void DoFlipBack(Card card, bool isRandomTime)
        {
            PlaySound(FlipBackSound);
            SetCardOrientation(card, CardOrientation.IsFlip);

            var rotateVector = GetRotateVector(flipBackDirection);
            var time = rotationTimeBack;

            if (isRandomTime)
            {
                time = RandomTime(time);
            }

            card.transform.position -= flipOffset;
            DoRotate(card.gameObject, rotateVector, time, easeTypeBack)
                  .OnComplete(() => FlipBackEnd(card));

            DoScale(card.gameObject, time);


            if (flipCardImage) StartCoroutine(FlipImage(card.MainArt, time * flipBackTrigger));
            if (GameManager.Instance.IsPvP) StartCoroutine(HideDoubleCard(card, time * flipBackTrigger));
        }

        void FlipEnd(Card card)
        {
            card.FlipEnd();
            SetCardOrientation(card, CardOrientation.Face);
            Events.Instance.FlipEnd(card);
           // card.transform.position += flipOffset;
        }

        void FlipBackEnd(Card card)
        {
            card.FlipBackEnd();
            if (GameManager.Instance.IsPvP) card.Double.EnableSprites();
            SetCardOrientation(card, CardOrientation.Back);
            Events.Instance.FlipBackEnd(card);
       //     card.transform.position -= flipOffset;
        }

        void DoScale(GameObject card, float time)
        {
            var size = card.transform.localScale.x;

            card.transform
                .DOScale(RotateScaleUp(size), time * 0.5f)
                .OnComplete(() => card.transform.DOScale(size, time * 0.5f));
        }

        void SetCardOrientation(Card card, CardOrientation orientation)
        {
            card.Orientation = orientation;
            //  Events.Instance.CardStateChanged(card, orientation);
        }

 

        void PlaySound(SoundData audioEvent) => AudioManager.Instance.PlaySound(audioEvent);
        float GetDirection(FlipDirection dir) => dir == FlipDirection.Left ? 1 : -1;

        Vector3 GetRotateVector(FlipDirection flipDir)
        {
            var dir = GetDirection(flipDir);
            return new Vector3(0, ANGLE_ROT_MAX * dir, 0);
        }

        static TweenerCore<Quaternion, Vector3, QuaternionOptions>
            DoRotate(GameObject card, Vector3 rotateVector, float time, Ease easeType) =>
            card.transform
                .DOLocalRotate(rotateVector, time, RotateMode.LocalAxisAdd)
                .SetEase(easeType)
                .SetRelative(true);

        IEnumerator FlipImage(SpriteRenderer sprite, float delay)
        {
            yield return new WaitForSeconds(delay);
            sprite.flipX = !sprite.flipX;
        }
        IEnumerator ShowDoubleCard(Card card, float delay)
        {
            yield return new WaitForSeconds(delay);
            card.Double.Show();
        }
        IEnumerator HideDoubleCard(Card card, float delay)
        {
            yield return new WaitForSeconds(delay);
            card.Double.Hide();
        }
        float RandomTime(float time)
        {
            var randomMult = Random.Range(1 - randomMultiplier, 1 + randomMultiplier);
            time *= randomMult;
            return time;
        }
    }
}
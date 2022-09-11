using System;
using System.Collections.Generic;
using __PUBLISH_v1.Scripts;
using DG.Tweening;
using EPOOutline;
using game.cards.data;
using game.cards.glow;
using game.cards.interfaces;
using JetBrains.Annotations;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using TMPro;
using UnityEngine;

namespace game.cards
{
    [ExecuteInEditMode]
    public class Card : MonoBehaviour, ICard
    {
        [Header("DEBUG")]
        [HideInInspector] public string debugCardName;
        [HideInInspector] public bool useDebugText;
        public List<TextMeshProUGUI> debugText = new();
        [HideInInspector] public bool useDebugRibbon;
        public List<SpriteRenderer> debugImage = new();
        [SerializeField] BoxCollider cardCollider;
        [Header("WolfBlock")]
        [SerializeField] bool useWolfBlock;
        [SerializeField] SpriteRenderer wolfBlock;
        [SerializeField] DOTweenAnimation wolfAnim;
        [Header("HideObjects")]
        [SerializeField] bool hideObjectsAtCombination;
        [SerializeField] bool hideArtAtCombination;
        [SerializeField] float hideObjectsDelay;
        [SerializeField] List<GameObject> hideObjects = new();
        [SerializeField] float hideArtDelay;
        [Header("ShowObjects")]
        [SerializeField] SoundData showObjectsSound;
        [SerializeField] bool showObjectsAtCombination;
        [SerializeField] float showObjectsDelay;
        [SerializeField] float showObjectsTime;
        [SerializeField] Ease showObjectsType;
        [SerializeField] List<SpriteRenderer> showObjects = new();
        [Header("Animation")]
        [SerializeField] DOTweenAnimation failAnimation;
        [SerializeField] float talkSoundDelay;
        [SerializeField] bool scaleArtAnimation;
        [SerializeField] bool shakeArtAnimation;
        [SerializeField] float scaleY = 1.1f;
        [SerializeField] float scaleYdur = 1.1f;
        [SerializeField] float artScaleMax;
        [SerializeField] float artScaleTime;
        [Header("Main art")]
        [SerializeField] SpriteRenderer mainArt;
        [SerializeField] SpriteRenderer mainFrame;
        [Header("Double mode")]
        [SerializeField] DoubleCard doubleCard;
        [Header("Back")]
        [SerializeField] [CanBeNull] SpriteRenderer cardBackImage;
        // [Header("Frames")]
        // [SerializeField] SpriteRenderer cardFrameFront;
        // [SerializeField] SpriteRenderer cardFrameBack;
        [Header("Tap")]
        [SerializeField] TapCatch tapCatch;
        [Header("Outline")]
        [SerializeField] Outlinable outline;
        [SerializeField] SpriteRenderer petSignBack;
        [Header("Glow")]
        [SerializeField] Glow glow;
        [Header("Inaactive state")]
        [SerializeField] Inactive inactiveImage;
        [Header("SparksOut")]
        [SerializeField] GameObject sparksOut;
        [SerializeField] List<ParticleSystem> sparksOutVfx = new();
        [Header("Coin")]
        [SerializeField] bool showPetCoin;
        [SerializeField] bool collectPetCoin;
        [SerializeField] SpriteRenderer petCoin;
        [Header("Fake Shadow")]
        [SerializeField] bool useShadow;
        [SerializeField] FakeShadow shadow;
        public event Action OnDisabled = delegate { };
        public event Action OnCardAnimate = delegate { };
        public DoubleCard Double => doubleCard;
        public SpriteRenderer MainArt => mainArt;
        public CardData Data { get; private set; }
        public CardOrientation Orientation { get; set; }
        bool _isOutlined;
        DeckData _deckType;
        public SpriteRenderer piedestalImageCircle;
        void OnDisable() => mainArt.DOKill();

        void Start()
        {
            if (!Application.isPlaying) return;
            if (glow) glow.Disable();
            if (outline) DisableOutline();
            if (!showPetCoin) DisableCoin();
            if (sparksOut) sparksOut.SetActive(false);
            if (!useShadow) DisableShadow();
            else EnableShadow();
            foreach (var obj in showObjects) obj.enabled = false;
            wolfBlock.enabled = false;

#if UNITY_EDITOR
            useDebugText = GameManager.Instance.Config.useCardsDebugText;
            useDebugRibbon = GameManager.Instance.Config.useCardsDebugRibbon;
#else
            useDebugText = false;
            useDebugRibbon = false;
#endif
            foreach (var txt in debugText)
            {
                txt.enabled = useDebugText;
                txt.transform.parent.gameObject.SetActive(useDebugText);
            }

            foreach (var img in debugImage)
            {
                img.enabled = useDebugRibbon;
                img.transform.parent.gameObject.SetActive(useDebugRibbon);
            }
        }

        public void WolfBlock()
        {
            if (useWolfBlock)
            {
                wolfBlock.enabled = true;
                wolfAnim.DOPlay();
            }
        }

        public void DisableWolfBlock()
        {
            wolfAnim.DOPause();
            wolfBlock.enabled = false;
        }

        public void SetMainArt(Sprite sprite)
        {
            piedestalImageCircle.enabled = false;
            mainArt.sprite = sprite;
        }

        public void DoubleArt()
        {
            petCoin.transform.parent.gameObject.SetActive(false);
            mainFrame.enabled = false;
            mainArt.transform.parent.gameObject.SetActive(false);
            doubleCard.Init(mainArt.sprite);
        }

        public void SetCard(DeckData deckType, CardData cardData)
        {
            Data = cardData;
            _deckType = deckType;
            SetArt(deckType);
        }

        public void Active() => inactiveImage.Active();

        public void Inactive() => inactiveImage.Innactive();

        public void Disable()
        {
            tapCatch.Disable();
            Inactive();
            OnDisabled();
        }

        public void Fail()
        {
            failAnimation.DORestart();
            failAnimation.DOPlay();
        }

        public void ReturnToPool()
        {
            gameObject.SetActive(false);
        }


        public void CombineAnimation()
        {
            if (showPetCoin && collectPetCoin) DisableCoin();
            cardCollider.enabled = false;
            if (hideObjectsAtCombination)
            {
                Invoke(nameof(HideObjets), hideObjectsDelay);
            }

            if (hideArtAtCombination)
            {
                Invoke(nameof(HideArt), hideArtDelay);
            }

            if (showObjectsAtCombination)
            {
                Invoke(nameof(ShowObecjts), showObjectsDelay);
            }

            if (scaleArtAnimation)
            {
                mainArt.transform
                    .DOScale(artScaleMax, artScaleTime / 2)
                    .OnComplete(()
                        => mainArt.transform
                            .DOScale(1, artScaleTime / 2));
            }
        }

        public void Animate(bool useSound)
        {
            OnCardAnimate();
            if (useSound && Data) AudioManager.Instance.PlaySound(Data.TalkSound, talkSoundDelay);

            if (sparksOut)
            {
                sparksOut.SetActive(true);
                foreach (var vfx in sparksOutVfx) vfx.Play();
            }
        }


        void ShowObecjts()
        {
            AudioManager.Instance.PlaySound(showObjectsSound);
            foreach (var obj in showObjects) obj.enabled = true;
            //      showObjects[0].DOFade(0, showObjectsTime * 1 / 6).SetDelay(showObjectsTime * 5 / 6);
            //     showObjects[1].DOFade(0, showObjectsTime * 1 / 6).SetDelay(showObjectsTime * 5 / 6);
            showObjects[0].transform
                .DOLocalMove(new Vector3(-0.1f, 0.1f, showObjects[0].transform.localPosition.z), showObjectsTime)
                .SetEase(showObjectsType)
                .OnComplete(() => showObjects[0].enabled = false);
            showObjects[1].transform
                .DOLocalMove(new Vector3(0.1f, -0.1f, showObjects[1].transform.localPosition.z), showObjectsTime)
                .SetEase(showObjectsType)
                .OnComplete(() => showObjects[1].enabled = false);
        }

        void HideArt() => mainArt.gameObject.SetActive(false);

        void HideObjets()
        {
            foreach (var hideObject in hideObjects) hideObject.SetActive(false);
        }

        public void SetSparkOutColor(Color clr)
        {
            foreach (var vfx in sparksOutVfx)
            {
                var main = vfx.main;
                main.startColor = clr;
            }
        }

        public void FlipEnd() => PlayArtShake();
        public void FlipBackEnd() => StopArtShake();

        void PlayArtShake()
        {
            mainArt.transform.parent.localScale = Vector3.one;
            mainArt.transform.parent
                .DOScaleX(scaleY, scaleYdur)
                .SetLoops(-1, LoopType.Yoyo);
        }

        void StopArtShake()
            => mainArt.transform.parent.DOKill();

        public void EnableOutline()
        {
            if (_isOutlined) return;
            _isOutlined = true;
            if (GameManager.Instance.IsPvP) doubleCard.EnableOutline();
            else outline.enabled = true;
            petSignBack.enabled = false;
        }

        public void DisableOutline()
        {
            _isOutlined = false;
            if (GameManager.Instance.IsPvP) doubleCard.DisableOutline();
            else outline.enabled = false;
            petSignBack.enabled = true;
        }

        public void SetOutlineColor(Color clr)
        {
            if (GameManager.Instance.IsPvP) doubleCard.SetOutlineColor(clr);
            else outline.OutlineParameters.Color = clr;
        }

        public void EnableGlow() => glow.Enable();
        public void DisableGlow(float delay = 0) => glow.Disable(delay);
        public void HalfGlow(Color sparksClr) => glow.Half(sparksClr);
        public void SetGlowColor(Color frameColor, Color sparkColor) => glow.SetColor(frameColor, sparkColor);

        void SetArt(DeckData deckType)
        {
            if (!deckType) return;
            if (cardBackImage) cardBackImage.sprite = this._deckType.CardBackImage;
            //   if (cardFrameFront) cardFrameFront.sprite = this._deckType.CardFrame;
            //    if (cardFrameBack) cardFrameBack.sprite = this._deckType.CardFrame;
        }


        public void Back()
        {
            DisableWolfBlock();
            petCoin.enabled = true;
            if (hideObjectsAtCombination)
                foreach (var obj in hideObjects)
                    obj.SetActive(true);

            if (hideArtAtCombination)
                mainArt.gameObject.SetActive(true);

            if (showObjectsAtCombination)
                foreach (var obj in showObjects)
                    obj.enabled = false;
        }


        void DisableCoin() => petCoin.enabled = false;
        void EnableShadow() => shadow.Enable();
        void DisableShadow() => shadow.Disable();
    }
}
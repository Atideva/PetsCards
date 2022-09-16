using System.Collections;
using System.Collections.Generic;
using __PUBLISH_v1.Scripts;
using DG.Tweening;
using fromWordSearch;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.TestBug
{
    public class WinPopupUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] SoundData winPopupSound;
        [SerializeField] SoundData itemShowSound;
        [SerializeField] SoundData buttonCollectSound;
        [SerializeField] SoundData buttonPlaySound;
        [SerializeField] float popupDelay;
        [SerializeField] float confettiDelay;
        [SerializeField] float fadeTime = 0.3f;
        [SerializeField] float delayBetweenItems = 0.3f;
        [SerializeField] float pompSize = 1.2f;
        [SerializeField] float pompTime = .5f;
        [Header("Buttons")]
        [SerializeField] Button collectButton;
        [SerializeField] Button playButton;
        [SerializeField] Button mainMenuButton;
        [Header("Canvas groups")]
        [SerializeField] CanvasGroup winPopup;
        [SerializeField] CanvasGroup popupWindow;
        [SerializeField] CanvasGroup playCanvas;
        [SerializeField] CanvasGroup collectCanvas;
        [SerializeField] CanvasGroup menuCanvas;
        [SerializeField] CanvasGroup coinScoreCanvas;
        [Header("Particle")]
        [SerializeField] ParticleSystem confettiVfxLeft;
        [SerializeField] ParticleSystem confettiVfxRight;
        [SerializeField] DOTweenAnimation starsAnimation;

        [SerializeField] CanvasGroup starsGroup;
        [SerializeField] CanvasGroup applauseGroup;
        [Header("UI")]
        [SerializeField] WinCollectUI collectUI;
        [SerializeField] WInPlayUI playUI;
        [SerializeField] CatAnimation catAnim;
        [SerializeField] DOTweenAnimation catHeadAnim;
        [Header("TEST")]
        public LevelConfig testCurrent;
        public LevelConfig testNext;
        public bool testWin;
        public List<DOTweenAnimation> animations = new();
        bool _isCollected;
        LevelConfig _curLvl;
        LevelConfig _nextLvl;


#if UNITY_EDITOR
        void Update()
        {
            if (testWin)
            {
                testWin = false;
                _curLvl = testCurrent;
                _nextLvl = testNext;
                Show(100, 10,1, _curLvl, _nextLvl);
            }
        }
#endif


        void Awake()
        {
            DisableGroup(winPopup);
            DisableGroup(collectCanvas);
            DisableGroup(playCanvas);
            DisableGroup(menuCanvas);
            DisableGroup(starsGroup);
            DisableGroup(applauseGroup);
        }

        public void Show(int coins, int petCoins,int gems, LevelConfig curLvl, LevelConfig nextLvl)
        {
            _curLvl = curLvl;
            _nextLvl = nextLvl;
            SubscribeButtons();
            Sound(winPopupSound, popupDelay);
            PopupAnimation(popupDelay);
            //      ApplauseAnimation(applauseDelay);
            if (coins == 0 && petCoins == 0 && gems == 0)
            {
                ShowNextLevelUI();
            }
            else
            {
                ShowCollectUI(coins, petCoins,gems, popupDelay);
            }
            coinScoreCanvas.DOFade(0, fadeTime).SetDelay(popupDelay + 0.5f);
           // EnableGroup(starsGroup);
            Show(starsGroup,0.2f,0.2f);
            //   StarsAnimation(popupDelay + 0.4f);
            catAnim.Enable();
            catHeadAnim.DOPlay();
            ConfettiAnimation(confettiDelay);
        }

        void ApplauseAnimation(float delaySec)
        {
            EnableGroup(applauseGroup);
            Show(applauseGroup, fadeTime, delaySec);
            applauseGroup.transform.DOScale(pompSize, pompTime / 2)
                .SetDelay(delaySec)
                .OnComplete(() =>
                    applauseGroup.transform.DOScale(1, pompTime / 2));
            foreach (var anim in animations)
                anim.DOPlay();
        }

        void ShowCollectUI(int coins,int petCoins, int gems, float delaySec)
        {
            collectUI.Init(coins,petCoins, gems, fadeTime, delayBetweenItems, itemShowSound);
            collectUI.Show(delaySec);
        }

        void PopupAnimation(float delaySec)
        {
            EnableGroup(winPopup);
            Show(winPopup, fadeTime, delaySec);
            popupWindow.transform.localScale = Vector3.zero;
            popupWindow.transform
                .DOScale(pompSize, pompTime / 2)
                .SetDelay(delaySec)
                .OnComplete(() =>
                    popupWindow.transform
                        .DOScale(1, pompTime / 2));
        }

        void StarsAnimation(float delaySec)
        {
            Show(starsGroup, 0.5f, delaySec);
            var starsRect = (RectTransform) starsAnimation.transform;
            starsRect.anchoredPosition = Vector2.zero;
            starsAnimation.DOPlay();
        }

        void Sound(SoundData soundData, float delay = 0) => AudioManager.Instance.PlaySound(soundData, delay);

        void Collect()
        {
            if (_isCollected) return;
            collectUI.Hide();
            Sound(buttonCollectSound);
            ShowNextLevelUI();
            _isCollected = true;
        }

        void ShowNextLevelUI()
        {
            if (_nextLvl)
            {
                playUI.Init(_nextLvl, fadeTime, delayBetweenItems, itemShowSound);
                playUI.Show(delayBetweenItems);
            }
            else
            {
                ShowMainMenu();
            }

        }
        void PlayNext()
        {
            Sound(buttonPlaySound);
            GameManager.Instance.LoadNextLevel();
        }

        void BackToMainMenu() => GameManager.Instance.LoadMainMenu();

        void ShowMainMenu()
        {
            menuCanvas.interactable = true;
            menuCanvas.blocksRaycasts = true;
            StartCoroutine(ShowCanvasGroup(menuCanvas, 0.1f, 1.3f));
        }

        IEnumerator HideCanvasGroup(CanvasGroup canvasGroup, float hideTime, float delay = 0)
        {
            if (delay > 0) yield return new WaitForSeconds(delay);
            var a = 1f;
            while (a > 0)
            {
                a -= Time.deltaTime / hideTime;
                canvasGroup.alpha = a;
                yield return null;
            }

            canvasGroup.alpha = 0;
        }

        IEnumerator ShowCanvasGroup(CanvasGroup canvasGroup, float hideTime, float delay = 0)
        {
            if (delay > 0) yield return new WaitForSeconds(delay);
            var a = 0f;
            while (a < 1)
            {
                a += Time.deltaTime / hideTime;
                canvasGroup.alpha = a;
                yield return null;
            }

            canvasGroup.alpha = 1;
        }

        void Show(CanvasGroup canvasGroup, float showTime, float delaySec = 0) =>
            StartCoroutine(ShowCanvasGroup(canvasGroup, showTime, delaySec));

        void Hide(CanvasGroup canvasGroup, float hideTime, float delaySec = 0) =>
            StartCoroutine(HideCanvasGroup(canvasGroup, hideTime, delaySec));

        void DisableGroup(CanvasGroup group)
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }

        void EnableGroup(CanvasGroup group)
        {
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        void SubscribeButtons()
        {
            collectButton.onClick.AddListener(Collect);
            playButton.onClick.AddListener(PlayNext);
            mainMenuButton.onClick.AddListener(BackToMainMenu);
        }

        void ConfettiAnimation(float delaySec) => Invoke(nameof(PlayConfetti), delaySec);

        void PlayConfetti()
        {
            confettiVfxLeft.Play();
            confettiVfxRight.Play();
        }
    }
}
using DG.Tweening;
using game.managers;
using UnityEngine;
using UnityEngine.UI;

namespace __PUBLISH_v1.Scripts
{
    public class LosePopupUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float fadeTime = 0.3f;
        [SerializeField] float delay = 1;
        [SerializeField] float delaySmall = 0.3f;
        [Header("Buttons")]
        [SerializeField] Button retryButton;
        [SerializeField] Button mainMenuButton;
        [Header("Canvas groups")]
        [SerializeField] CanvasGroup popup;
        [SerializeField] CanvasGroup joke;
        [SerializeField] CanvasGroup retry;
        [SerializeField] CanvasGroup menu;


        void Awake()
        {
            Hide();
        }

        public void Hide()
        {
            DisableGroup(popup);
            DisableGroup(joke);
            DisableGroup(retry);
            DisableGroup(menu);  
        }
        public void Show()
        {
            EnableGroup(popup);
            EnableGroup(retry);
            EnableGroup(menu);
            SubscribeButtons();
            popup.DOFade(1, fadeTime).SetDelay(delay);
            joke.DOFade(1, fadeTime).SetDelay(delay + delaySmall);
            menu.DOFade(1, fadeTime).SetDelay(delay + delaySmall * 2);
            retry.DOFade(1, fadeTime).SetDelay(delay + delaySmall * 3);
        }

        void EnableGroup(CanvasGroup group)
        {
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        void DisableGroup(CanvasGroup group)
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }

        void SubscribeButtons()
        {
            retryButton.onClick.AddListener(Retry);
            mainMenuButton.onClick.AddListener(BackToMainMenu);
        }

        void BackToMainMenu()
        {
            Events.Instance.BackToMainMenu();
            GameManager.Instance.LoadMainMenu();
        }

        void Retry()
        {
            Events.Instance.Retry();
            GameManager.Instance.RestartLevel();
        }
    }
}
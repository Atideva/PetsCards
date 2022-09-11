using System.Collections.Generic;
using __PUBLISH_v1.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace game.levels
{
    public class WinPvpUI : MonoBehaviour
    {
        [SerializeField] List<Button> mainMenuButtons = new();
        [SerializeField] List<Button> restartButtons = new();
        [SerializeField] CanvasGroup playerOneWin;
        [SerializeField] CanvasGroup playerOneLose;
        [SerializeField] CanvasGroup playerTwoWin;
        [SerializeField] CanvasGroup playerTwoLose;
        [SerializeField] CanvasGroup container;
        void Awake()
        {
            DisableGroup(container);
            DisableGroup(playerOneWin);
            DisableGroup(playerOneLose);
            DisableGroup(playerTwoWin);
            DisableGroup(playerTwoLose);

        }
        public void Show(int winnerId)
        {
            EnableGroup(container);

            foreach (var item in restartButtons)
                item.onClick.AddListener(Restart);
            foreach (var item in mainMenuButtons)
                item.onClick.AddListener(BackToMainMenu);

            if (winnerId == 0)
            {
                EnableGroup(playerOneWin);
                EnableGroup(playerOneLose);
                EnableGroup(playerTwoWin);
                EnableGroup(playerTwoLose);
            }
            if (winnerId == 1)
            {
                EnableGroup(playerOneWin);
                EnableGroup(playerTwoLose);
            }
            if (winnerId == 2)
            {
                EnableGroup(playerTwoWin);
                EnableGroup(playerOneLose);
            }
        }


        void BackToMainMenu() => GameManager.Instance.LoadMainMenu();
        void Restart() => GameManager.Instance.RestartLevel();
        void DisableGroup(CanvasGroup group)
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
        void EnableGroup(CanvasGroup group)
        {
            group.alpha = 1;
            group.interactable = true;
            group.blocksRaycasts = true;
        }
    }
}

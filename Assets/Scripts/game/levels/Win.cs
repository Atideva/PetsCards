using __PUBLISH_v1.Scripts;
using game.managers;
using game.player;
using Scenes.TestBug;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;

namespace game.levels
{
    public class Win : MonoBehaviour
    {
        [SerializeField] SoundData levelWinSound;
        [SerializeField] Score score;
        [SerializeField] PetCoinScore petCoinScore;
        [SerializeField] WinPopupUI winPopupUI;
        [SerializeField] WinPvpUI winPvpUI;
        [SerializeField] LevelPVPUI pvp;

        void Start()
        {
            Events.Instance.OnWin += OnWin;
        }

        void OnWin()
        {
            AudioManager.Instance.PlaySound(levelWinSound);
            Invoke(GameManager.Instance.IsPvP ? nameof(ShowPvpUI) : nameof(ShowUI), 0.65f);
        }

        void ShowPvpUI()
        {
            winPvpUI.Show(pvp.GetWinnerID());
        }
        void ShowUI()
        {
            var curLvl = GameManager.Instance ? GameManager.Instance.CurrentLevel : null;
            var nextLvl = GameManager.Instance ? GameManager.Instance.NextLevel : null;
            var coins = score.TotalScore;
            var petCoins = petCoinScore.TotalCoins;
            var lvlData = GameManager.Instance ? GameManager.Instance.FindData(curLvl) : null;
            var gems = lvlData != null
                ? lvlData.isComplete
                    ? 0
                    : 1
                : 0;

            winPopupUI.Show(coins,petCoins, gems, curLvl, nextLvl);
        }
    }
}
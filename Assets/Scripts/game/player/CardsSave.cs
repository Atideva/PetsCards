using System.Linq;
using app.keys;
using game.cards.data;
using UnityEngine;

namespace game.player
{
    public class CardsSave : MonoBehaviour
    {
        GameConfig _gameConfig;
        RuntimeData _runtimeData;
        [SerializeField] CardsUnlockedData save;
        const string KEY = ConstantsKeys.PlayerCardsUnlocked;

        public void Init(GameConfig gameConfig, RuntimeData runtimeData)
        {
            _gameConfig = gameConfig;
            _runtimeData = runtimeData;
            LoadData();
        }

        // void Start()
        // {
        //     Events.Instance.OnPlayerUnlockCard += Unlock;
        // }

        void SaveData()
        {
            PlayerPrefs.SetString(KEY, JsonUtility.ToJson(save));
            RefreshData();
        }

        void RefreshData() => _runtimeData.SetOwnedCards(save.unlockedCards);

        void LoadData()
        {
            if (PlayerPrefs.HasKey(KEY))
            {
                save = JsonUtility.FromJson<CardsUnlockedData>(PlayerPrefs.GetString(KEY));
                ReadData();
                RefreshData();
            }
            else
            {
                FirstLaunch();
            }
        }


        void FirstLaunch()
        {
            var list = _gameConfig.BaseCards.ToList();
            save = new CardsUnlockedData(list);
            SaveData();
        }

        void ReadData()
        {
            foreach (var item in _gameConfig.BaseCards)
            {
                if (!save.unlockedCards.Contains(item))
                {
                    save.unlockedCards.Add(item);
                }
            }
        }


        void Unlock(CardData card)
        {
            if (save.unlockedCards.Contains(card))
            {
                return;
            }

            save.unlockedCards.Add(card);
            SaveData();
        }

        public bool IsUnlock(CardData card) => save.unlockedCards.Contains(card);
    }
}
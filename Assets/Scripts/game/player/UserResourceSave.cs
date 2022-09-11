using System;
using app.keys;
using UnityEngine;

namespace game.player
{
    public class UserResourceSave : MonoBehaviour
    {
        RuntimeData _runtimeData;
        [SerializeField] UserResourceData save;
        const string RESOURCES_KEY = ConstantsKeys.PlayerResources;
        public event Action OnValueChange = delegate { };

        public void Init(RuntimeData runtimeData)
        {
            _runtimeData = runtimeData;
            LoadData();
        }

        public int Gold => save.gold;
        public int PeCoin => save.petCoin;
        public int Gem => save.gem;

        void LoadData()
        {
            save = PlayerPrefs.HasKey(RESOURCES_KEY)
                ? JsonUtility.FromJson<UserResourceData>(PlayerPrefs.GetString(RESOURCES_KEY))
                : new UserResourceData();

            WriteToLiveData();
        }

        void SaveData()
        {
            PlayerPrefs.SetString(RESOURCES_KEY, JsonUtility.ToJson(save));
            WriteToLiveData();
        }

        void WriteToLiveData() => _runtimeData.SetResources(save);


        public void AddGold(int value) => ChangeGold(value);
        public void RemoveGold(int value) => ChangeGold(-value);
        void ChangeGold(int value) => Change(value, 0, 0);


        public void AddPetCoin(int value) => ChangePetCoin(value);
        public void RemovePetCoin(int value) => ChangePetCoin(-value);
        void ChangePetCoin(int value) => Change(0, value, 0);


        public void AddGem(int value) => ChangeGem(value);
        public void RemoveGem(int value) => ChangeGem(-value);
        void ChangeGem(int value) => Change(0, 0, value);


        public void Add(int coin, int petCoin, int gem) => Change(coin, petCoin, gem);
        public void Subtract(int coin, int petCoin, int gem) => Change(-coin, petCoin, -gem);

        void Change(int coin, int petCoin, int gem)
        {
            save.gold += coin;
            save.petCoin += petCoin;
            save.gem += gem;
            SaveData();
            OnValueChange();
        }
    }
}
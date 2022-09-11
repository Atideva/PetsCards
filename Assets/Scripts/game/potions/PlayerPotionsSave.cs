using System.Collections.Generic;
using app.keys;
using UnityEngine;

namespace game.potions
{
    [System.Serializable]
    public class SavePlayerStashPotions
    {
        public Dictionary<PotionData, int> PotionsCount = new Dictionary<PotionData, int>();
        #region modifiers
        public int GetPotionCount(PotionData potion)
        {
            if (potion == null) return 0;
            return PotionsCount.ContainsKey(potion) ? PotionsCount[potion] : 0;
        }
        public void SetPotionCount(PotionData potion, int count)
        {
            if (PotionsCount.ContainsKey(potion))
                PotionsCount[potion] = count;
            else
                PotionsCount.Add(potion, count);
        }
        #endregion

    }


    [System.Serializable]
    public class SavePlayerSlotPotion
    {
        List<PotionData> _potionInSlot = new List<PotionData>();
        #region modifiers
        public PotionData GetPotionInSlot(int slot)
        {
            CheckSlotsCount(slot);
            return _potionInSlot[slot];
        }
        public void SetPotionInSlot(PotionData potion, int slot)
        {
            CheckSlotsCount(slot);
            _potionInSlot[slot] = potion;
        }
        void CheckSlotsCount(int slot)
        {
            int slotsTotal = _potionInSlot.Count;
            if (slot >= slotsTotal)
            {
                for (int i = slotsTotal; i <= slot; i++)
                {
                    _potionInSlot.Add(null);
                }
            }
        }
        #endregion

    }


    public class PlayerPotionsSave : MonoBehaviour
    {

        #region Awake Singleton
        //-------------------------------------------------------------
        public static PlayerPotionsSave Instance;
        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);

            Debug.LogWarning("Make me DontDestroyOnLoad please, to prevent load huge data for all levels at each OnLevelLoad",gameObject);
            AwakeJob();
        }
        //-------------------------------------------------------------
        #endregion


        [SerializeField] PotionBundle allPotions;

 
        SavePlayerSlotPotion _slotsPotion;
        const string SLOTS = ConstantsKeys.DataPlayerSlotPotion;
 
        SavePlayerStashPotions _potionStash;
        const string STASH = ConstantsKeys.DataPlayerStashPotions;


        void AwakeJob() => LoadData();
        void LoadData()
        {
            Debug.LogWarning("Make it DontDestroyOnLoad", gameObject);
            LoadStash();
            LoadSlots();
        }

        void LoadStash() => _potionStash = PlayerPrefs.HasKey(STASH) ? JsonUtility.FromJson<SavePlayerStashPotions>(PlayerPrefs.GetString(STASH)) : new SavePlayerStashPotions();
        void LoadSlots() => _slotsPotion = PlayerPrefs.HasKey(SLOTS) ? JsonUtility.FromJson<SavePlayerSlotPotion>(PlayerPrefs.GetString(SLOTS)) : new SavePlayerSlotPotion();

        void SaveStash() => PlayerPrefs.SetString(STASH, JsonUtility.ToJson(_potionStash));
        void SaveSlots() => PlayerPrefs.SetString(SLOTS, JsonUtility.ToJson(_slotsPotion));

        //SLOT
        public PotionData Get_PotionType_InSlot(int slot) => _slotsPotion.GetPotionInSlot(slot);
        public void Set_PotionType_InSlot(PotionData potion, int slot)
        {
            _slotsPotion.SetPotionInSlot(potion, slot);
            SaveSlots();
        }

        //STASH
        public int Get_Stash_PotionCount(PotionData potion) => _potionStash.GetPotionCount(potion);
        public void Set_Stash_PotionCount(PotionData potion, int count)
        {
            _potionStash.SetPotionCount(potion, count);
            SaveStash();
        }


        //ALL POTIONS BUNDLE
        public GameObject GetPotionPrefab(PotionData potion) => allPotions.GetPotionPrefab(potion);


    }
}
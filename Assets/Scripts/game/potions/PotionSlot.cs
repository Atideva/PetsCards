using TMPro;
using UnityEngine;

namespace game.potions
{
    public class PotionSlot : MonoBehaviour
    {
        [Header("Setup")]
        public TextMeshProUGUI countTxt;
        public Transform potionPosition;

        [Header("Inspector")]
        public GameObject potionObject;

        int _slotID;
        PotionData _myPotion;
        int _potionCount;
        Potion _potion;

        public int SlotID { get => _slotID; }
        public void SetupSlotId(int id) => _slotID = id;



        void Start()
        {
            LoadSlot();
            CreatePotion();
            RefreshTxt(_potionCount);
        }
        public void SlotTouched()
        {
            TryUsePotion();
        }



        void LoadSlot()
        {
            _myPotion = LoadPotion(_slotID);
            _potionCount = PotionCount(_myPotion);
        }
        void CreatePotion()
        {
            var prefab = PlayerPotionsSave.Instance.GetPotionPrefab(_myPotion);
            if (prefab)
            {
                potionObject = Instantiate(prefab, potionPosition);
                _potion = potionObject.GetComponent<Potion>();
            }
        }



        void TryUsePotion()
        {
            if (!_potion) return;

            UsePotion();

        }

        void UsePotion()
        {
            _potion.UsePotion();
            _potionCount--;

            SaveData();
            RefreshTxt(_potionCount);

            if (_potionCount == 0)
            {
                DisableText();
                DestroyPotion();
            }
        }

        void DestroyPotion()
        {
            _potion = null;
            _myPotion = null;
            potionObject.SetActive(false);
        }

        PotionData LoadPotion(int slot) => PlayerPotionsSave.Instance.Get_PotionType_InSlot(slot);
        int PotionCount(PotionData potion) => PlayerPotionsSave.Instance.Get_Stash_PotionCount(potion);
        void RefreshTxt(int count)
        {
            countTxt.enabled = count != 0;
            countTxt.text = TextFormat(count);
        }

        void DisableText() => countTxt.gameObject.SetActive(false);
        string TextFormat(int count) => count.ToString();
        void SaveData() => PlayerPotionsSave.Instance.Set_Stash_PotionCount(_myPotion, _potionCount);

    }
}

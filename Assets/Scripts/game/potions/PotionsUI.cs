using System.Collections.Generic;
using UnityEngine;

namespace game.potions
{
    public class PotionsUI : MonoBehaviour
    {
        [Header("Use")]
        public bool usePotionsUI;

        [Header("Setup")]
        public List<PotionSlot> slots = new List<PotionSlot>();

        void Awake()
        {
            SetupSlotIDs();

            if (!usePotionsUI) DisableSlots();
        }

        void SetupSlotIDs()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].SetupSlotId(i);
            }
        }
        void DisableSlots()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }
}

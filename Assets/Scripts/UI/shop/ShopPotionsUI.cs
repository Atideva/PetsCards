using System.Collections.Generic;
using UnityEngine;

namespace   UI.shop
{
    public class ShopPotionsUI : MonoBehaviour
    {
        [Header("Prefab")]
        public GameObject potionSlotPrefab;
        [Header("Slots")]
        public List<ShopPotionSlot> slots = new List<ShopPotionSlot>();

        void Start()
        {
            PutGoods();
        }

        void PutGoods()
        {
            var stock = ShopPotions.Instance.Stock.Potions;
            int stockSize = stock.Count;
            for (int i = 0; i < slots.Count; i++)
            {
                if (i < stockSize)
                    slots[i].PutPotionToSlot(stock[i]);
                else
                {
                    Debug.Log("Slots count are less than potion stock");
                    break;
                }
            }
        }
    }
}

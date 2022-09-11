using game.potions;
using UnityEngine;

namespace UI.shop
{
    public class ShopPotionSlot : MonoBehaviour
    {

        [SerializeField] Transform potionPosition;
        [SerializeField] SlotPrice goldSlotPrice;
        [SerializeField] SlotPrice gemSlotPrice;

        [SerializeField] int gold;
        [SerializeField] int gem;
        [SerializeField] int butterfly;

        PotionData _potion;
        GameObject _potionObject;
        public void PutPotionToSlot(PotionData potion)
        {
            this._potion = potion;

            if (_potionObject)
            {
                Destroy(_potionObject);
                Debug.LogWarning("Better use poolsystem here, and return instead of destroy", gameObject);
            }
            _potionObject = Instantiate(this._potion.Prefab, potionPosition);

            gold = _potion.shopPrice.coins;
            gem = _potion.shopPrice.gems;

            SetPrice(goldSlotPrice, gold);
            SetPrice(gemSlotPrice, gem);
        }

        void SetPrice(SlotPrice slotPrice, int value)
        {
            if (value > 0)
            {
                slotPrice.gameObject.SetActive(true);
                slotPrice.SetPrice(value);
            }
            else
            {
                slotPrice.gameObject.SetActive(false);
            }
        }
        public void SlotClicked()
        {
            if (Vendor.Instance.TryBuy_UseMultiResources(gold, gem ))
                PurchuaseSuccess();
            else
                PurchuaseFail();
        }
        void PurchuaseSuccess()
        {
            PlayerPotionsSave.Instance.Set_Stash_PotionCount(_potion, PlayerPotionsSave.Instance.Get_Stash_PotionCount(_potion) + 1);
        }
        void PurchuaseFail()
        {
            Debug.LogWarning("Not enough resources");
        }
    }
}
 

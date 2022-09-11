using game.potions;
using UnityEngine;

namespace UI.shop
{
    public class ShopPotions : MonoBehaviour
    {

        #region Awake Singleton
        //-------------------------------------------------------------
        public static ShopPotions Instance;
        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);

            Debug.LogWarning("Make me DontDestroyOnLoad please, to prevent load huge data for all levels at each OnLevelLoad");
        }
        //-------------------------------------------------------------
        #endregion

        [SerializeField] PotionBundle stock;

        public PotionBundle Stock => stock;
    }
}


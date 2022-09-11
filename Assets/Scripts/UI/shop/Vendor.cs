using __PUBLISH_v1.Scripts;
using game.player;
using UnityEngine;

namespace UI.shop
{
    public class Vendor : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static Vendor Instance;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);

            Debug.LogWarning(
                "Make me DontDestroyOnLoad please, to prevent load huge data for all levels at each OnLevelLoad");
        }

        //-------------------------------------------------------------

        #endregion

        UserResourceSave _userResourceSave;

        void Start()
        {
            _userResourceSave = GameManager.Instance.UserResourceSave;
        }


        public bool TryBuy_UseGold(int value)
        {
            if (_userResourceSave.Gold >= value)
            {
                _userResourceSave.RemoveGold(value);
                return true;
            }
            else
                return false;
        }

        public bool TryBuy_UseGem(int value)
        {
            if (_userResourceSave.Gem >= value)
            {
                _userResourceSave.RemoveGem(value);
                return true;
            }
            else
                return false;
        }

        public bool TryBuy_UseMultiResources(int gold, int gem)
        {
            if (_userResourceSave.Gold >= gold &&
                _userResourceSave.Gem >= gem)
            {
                _userResourceSave.Subtract(gold, 0, gem);
                return true;
            }

            return false;
        }
    }
}
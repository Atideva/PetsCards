using __PUBLISH_v1.Scripts;
using TMPro;
using UnityEngine;

namespace game.player
{
    public class PlayerResourcesUI : MonoBehaviour
    {
        public TextMeshProUGUI txtCoin;
        public TextMeshProUGUI txtGem;
        public TextMeshProUGUI txtPetCoin;
        UserResourceSave _res;

        void Start()
        {
            _res = GameManager.Instance.UserResourceSave;
            _res.OnValueChange += OnValueChange;
            RefreshText();
        }

        void OnDisable() 
            => _res.OnValueChange -= OnValueChange;

        void OnValueChange() 
            => RefreshText();

        void RefreshText()
        {
            if (txtCoin) txtCoin.text = _res.Gold.ToString();
            if (txtGem) txtGem.text = _res.Gem.ToString();
            if (txtPetCoin) txtPetCoin.text = _res.PeCoin.ToString();
        }
    }
}
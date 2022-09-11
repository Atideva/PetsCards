using systems.audio_manager.audio_Event;
using UI.shop;
using UnityEngine;

namespace game.potions
{
    [CreateAssetMenu(fileName = "Potion_", menuName = "Potions/New Potion")]
    public class PotionData : ScriptableObject 
    {
        [SerializeField] GameObject prefab;
        [SerializeField] SoundData useSound;
        [Header("Shop price")]
        public ShopPrice shopPrice;

        public GameObject Prefab { get => prefab; }
        public SoundData UseSound { get => useSound; }


 
    }
}

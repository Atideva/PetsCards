using System.Collections.Generic;
using systems.audio_manager.audio_Event;
using UI.shop;
using UnityEngine;

namespace game.cards.data
{
    [CreateAssetMenu(fileName = "NewCard", menuName = "Cards/New Card")]
    public class CardData : ScriptableObject
    {
        [Header("Color")]
        [SerializeField] Color glowColor = Color.white;
        [Header("Setup")]
        [SerializeField] Card cardPrefab;
        [SerializeField] SoundData talkSound;
        [Header("CraftMode")]
        [SerializeField] Sprite firstCraft;
        [SerializeField] Sprite secondCraft;
        [SerializeField] Sprite resultCraft;
        [Header("Shop price")]
        public ShopPrice shopPrice;

        [SerializeField] [TextArea]
        string description;
        [Header("Abilities")]
        [SerializeField] List<AbilityData> abilities;

        public Card CardPrefab => cardPrefab;
        public SoundData TalkSound => talkSound;

        public string Description => description;

        public IReadOnlyList<AbilityData> Abilities => abilities;

        public Color GlowColor => glowColor;

        public Sprite FirstCraft => firstCraft;

        public Sprite SecondCraft => secondCraft;

        public Sprite ResultCraft
        {
            get => resultCraft;
            set => resultCraft = value;
        }
    }
}
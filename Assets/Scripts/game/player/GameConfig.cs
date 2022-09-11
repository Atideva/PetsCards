using System.Collections.Generic;
using app.settings;
using game.cards.data;
using game.managers;
using systems.level_loader;
using UnityEngine;

namespace game.player
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Data/PlayerConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("DEBUG")]
        public bool useCardsDebugText;
        public bool useCardsDebugRibbon;
        public bool unlockAllCards;
        [Header("Configs")]
        [SerializeField] RuntimeData runtimeData;
        [SerializeField] CardList baseUnlockCards;
        [SerializeField] LevelList campaignLevels;
        [SerializeField] LevelList otherLevel;
        [SerializeField] LevelList baseUnlockLevels;
        [SerializeField] GameSettings settings;
        [SerializeField] SoundConfig sound;
        [SerializeField] MusicConfig music;
        [SerializeField] ColorConfig pairColors;
        [SerializeField] GameVisual visual;
        public IEnumerable<CardData> BaseCards => baseUnlockCards.Cards;
        public GameSettings Settings => settings;
        public LevelList CampaignLevels => campaignLevels;
        public LevelList BaseUnlockLevels => baseUnlockLevels;
        public SoundConfig Sound => sound;
        public MusicConfig Music => music;

        public RuntimeData RuntimeData => runtimeData;

        public LevelList OtherLevel => otherLevel;

        public ColorConfig PairColors => pairColors;

        public GameVisual Visual => visual;
    }
}
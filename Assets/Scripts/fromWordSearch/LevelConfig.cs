using System.Collections.Generic;
using game.cards.data;
using UnityEngine;


namespace fromWordSearch
{
    [CreateAssetMenu(fileName = "Level", menuName = "Game/LevelData")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] LevelMode mode;
        [SerializeField] GameMode cards;
        [Header("Settings")]
        [SerializeField] [Range(0.1f, 3f)] float difficulty = 1f;

        [Header("Cards")]
        [SerializeField] List<DeckData> decks;
        [ContentColor(0.45f, 0.45f, 0.45f, 1f)]
        [Header("________________________________________________________________________________")]
        [SerializeField]
        List<int> uniqueCards;
        [SerializeField] string lvlName;
        [SerializeField] Sprite icon;
        [SerializeField] bool autoSetScene = true;
        [SerializeField] string sceneToLoad;


        void OnValidate()
        {
            if (autoSetScene)
                sceneToLoad = name;
            uniqueCards = new List<int>();
            foreach (var deck in decks)
            {
                uniqueCards.Add(deck.UniqueCardsDebug);
            }
        }


        public float Difficulty => difficulty;
        public string SceneToLoad => sceneToLoad;
        public string LvlName => lvlName;
        public Sprite Icon => icon;


        public LevelMode Mode => mode;

        public IReadOnlyList<DeckData> Decks => decks;

        public GameMode GameMode => cards;
    }
}
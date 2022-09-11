using System.Collections.Generic;
using game.cards.data;
using UnityEngine;

namespace game.player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
    public class RuntimeData : ScriptableObject
    {
        [SerializeField] UserResourceData resources;
        [SerializeField] List<CardData> ownedCards = new();
        [Header("Gameplay")]
        [SerializeField] ActionData action;
        [SerializeField] AttemptData attempt;
        [SerializeField] ScoreData score;
        [SerializeField] ComboData combo;
        [Header("Level")]
        [SerializeField] WinPointsData winPointsData;
        [SerializeField] RoundsData rounds;


        public IReadOnlyList<CardData> OwnedCards => ownedCards;

        public UserResourceData Resources => resources;
        public ActionData Action => action;

        public AttemptData Attempt => attempt;

        public ScoreData Score => score;

        public ComboData Combo => combo;

        public WinPointsData WinPointsData => winPointsData;

        public RoundsData Rounds => rounds;


        void Awake()
        {
            ownedCards = new List<CardData>();
            resources = new UserResourceData();
        }

        public void AddOwnedCard(CardData card)
        {
            ownedCards.Add(card);
        }

        public void SetOwnedCards(IReadOnlyList<CardData> cards)
        {
            ownedCards = (List<CardData>) cards;
        }

        public void SetResources(UserResourceData res)
        {
            resources.gem = res.gem;
            resources.coin = res.coin;
        }
    }
}
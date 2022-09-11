using game.cards.data;
using game.cards.layout;
using game.cards.managers;
using UnityEngine;

namespace game.cards.create
{
    [ExecuteInEditMode]
    public class CreatorTest : MonoBehaviour
    {
        [Header("Test")]
        public bool createCards;
        public bool layoutCards;

        [Header("Setup")]
        [Range(4, 20)] public int totalCards;
        public DeckData deckType;
        public CardCreator cardCreator;
        public Layout layouter;
        public OnTableCards deck;



        void Update()
        {
#if UNITY_EDITOR
            if (totalCards % 2 != 0) totalCards++;
            if (totalCards == 14) totalCards = 16;

            if (createCards)
            {
                createCards = false;
                cardCreator.TestCreate(totalCards, deckType);
            }
            if (layoutCards)
            {
                layoutCards = false;
                layouter.LayoutCards(deck.onTable);
            }
#endif
        }

    }
}

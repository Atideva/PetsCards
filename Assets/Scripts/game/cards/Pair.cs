using game.cards.data;
using UnityEngine;

namespace game.cards
{
    [System.Serializable]
    public class Pair
    {
        public CardData cardType;
        public DeckData deckType;
        public Card card1;
        public Card card2;

        public Pair(Card card1, Card card2, CardData type = null, DeckData deck = null)
        {
            this.card1 = card1;
            this.card2 = card2;
            cardType = type;
            deckType = deck;
        }
    }
}
using System.Collections.Generic;
using game.cards.data;

namespace game.player
{
    [System.Serializable]
    public class CardsUnlockedData
    {
        public List<CardData> unlockedCards;
        public CardsUnlockedData(List<CardData> cards) => unlockedCards = cards;
    }
}
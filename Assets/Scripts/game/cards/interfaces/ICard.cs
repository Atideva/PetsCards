using System;
using game.cards.data;
using UnityEngine;

namespace game.cards.interfaces
{
    public interface ICard
    {
        event Action OnDisabled;
        void Disable();
        void Animate(bool useSound);
        void SetCard(DeckData deckType, CardData cardData);

        void Active();
        void Inactive();

        void EnableOutline();
        void DisableOutline();
        void SetOutlineColor(Color clr);
        void EnableGlow( );
        void DisableGlow(float delay);
        void HalfGlow(Color sparksClr);
    }
}
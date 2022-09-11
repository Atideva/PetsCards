using game.managers;
using UnityEngine;

namespace game.cards
{
    public class TapCatch : MonoBehaviour
    {
        public Card thisCard;

        void OnMouseDown()
        {
            if (_disabled)
            {
                return;
            }

            Events.Instance.CardClick(thisCard);
        }

        bool _disabled;

        public void Disable()
        {
            _disabled = true;
        }
    }
}
using System;
using UnityEngine;

namespace game.cards
{
    public abstract class CardAbilityVFX : MonoBehaviour
    {
        protected Card Card1 { get; private set; }
        protected Card Card2 { get; private set; }
        public event Action OnUseAbility = delegate { };
        public event Action OnFinish = delegate { };

        protected void Finish()
        {
            Reset();
            Disable();
            OnFinish();
        }

        protected void UseAbility()
        {
            OnUseAbility();
        }

        public void BindToCards(Card card1, Card card2)
        {
            Card1 = card1;
            Card2 = card2;
        }

        public void Init()
        {
            Reset();
            Disable();
        }

        public void Play()
        {
            gameObject.SetActive(true);
            OnVfxPlay();
        }

        protected abstract void OnVfxPlay();
        protected abstract void Reset();
        void Disable() => gameObject.SetActive(false);
    }
}
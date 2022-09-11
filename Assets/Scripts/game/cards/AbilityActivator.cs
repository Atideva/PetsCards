using System.Collections.Generic;
using System.Linq;
using game.cards.data;
using game.managers;
using UnityEngine;

namespace game.cards
{
    [RequireComponent(typeof(CardAbilityPool))]
    [RequireComponent(typeof(AbilityNotifyPool))]
    public class AbilityActivator : MonoBehaviour
    {
        [Header("DEBUG")]
        [SerializeField] Card card1;
        [SerializeField] Card card2;
        [SerializeField] AbilityConfig _ability;
        [Header("Prefabs")]
        [SerializeField] CardAbility abilityPrefab;

        [SerializeField] AbilityNotify notifyPrefab;


        [Header("Pools")]
        [SerializeField] int poolPrewarm = 2;
        [SerializeField] CardAbilityPool abilityPool;
        [SerializeField] AbilityNotifyPool notifyPool;
        HashSet<Pair> _match = new();
        //  HashSet<Card> _firstFlips = new();
        HashSet<CardData> _triggerCards = new();

        public CardAbility AbilityPrefab => abilityPrefab;

        public void Init(AbilityConfig initAbility, AbilityTriggerType type)
        {
            if (!abilityPool) abilityPool = GetComponent<CardAbilityPool>();
            if (!notifyPool) notifyPool = GetComponent<AbilityNotifyPool>();
            abilityPool.Init(abilityPrefab, poolPrewarm);
            notifyPool.Init(notifyPrefab, poolPrewarm);
            Events.Instance.OnFlipEnd += OnFlipEnd;


            _ability = initAbility;
            //   abilityPrefab.Init(ability);

            //  BindToCards(bind1, bind2);
            Activation(type);
        }

        public void AddTrigger(CardData triggerCard)
        {
            _triggerCards.Add(triggerCard);
            var notify = notifyPool.Get();
            notify.Init(_ability, triggerCard);
        }

        void SetCards(Card c1, Card c2)
        {
            card1 = c1;
            card2 = c2;

            //    notifyPrefab.BindToCards(c1, c2);
        }

        void Activation(AbilityTriggerType type)
        {
            switch (type)
            {
                case AbilityTriggerType.OnPairOpen:
                    Events.Instance.OnPairSuccess += OnPairSuccess;
                    break;
                // case AbilityActivateType.BothCardsTouchedOnce:
                //     BothCardsTouchOnceMode();
                // break;
            }
        }

        void OnPairSuccess(Card match1, Card match2)
        {
            // if (myCard1 == card1 && myCard2 == card2 ||
            //     myCard1 == card2 && myCard2 == card1)

            //   if (card2.Data != match2.Data) return;
            //    if (card1 != match1 && card2 != match2 && card1 != match2 && card2 != match1) return;

            //  Events.Instance.OnPairMatch -= OnPairMatch;

            if (!_triggerCards.Contains(match2.Data)) return;
            _match.Add(new Pair(match1, match2));
            SetCards(match1, match2);

            //  Invoke(nameof(UseAbility), Flipper.Instance.RotationTime + 0.05f);
        }


        void OnFlipEnd(Card card)
        {
            var pair = _match.FirstOrDefault(m => m.card2 == card);
            if (pair == null) return;
            UseAbility(pair.card1, pair.card2);
            _match.Remove(pair);
            // if (card == _lastFlips)
            // {
            //     UseAbility();
            //     Events.Instance.OnFlipEnd -= OnFlipEnd;
            // }
        }

        void UseAbility(Card c1, Card c2)
        {
            var ability = abilityPool.Get();
            ability.Init(_ability);
            ability.BindToCards(c1, c2);
            ability.UseAbility();

            //  abilityPrefab.UseAbility();
            Events.Instance.UseAbility(this._ability, card1, card2);
        }

        /*
        //TODO: короче, предлагаю к отказаться от этой идеи 1) Сложна в реализации 2) Прерывает flow игрока
        // void BothCardsTouchOnceMode() => Events.Instance.OnFlipCard_ENDED += OnCardFlipEnd;
       bool _card1Flip, _card2Flip;
        bool BothCards => _card1Flip && _card2Flip;
        void OnCardFlipEnd(GameObject card)
        {
            if (card == myCard1)
            {
                _card1Flip = true;
            }

            if (card == myCard2)
            {
                _card2Flip = true;
            }

            if (BothCards)
            {
                _card1Flip = false;
                _card2Flip = false;
                UseAbility();
            }
        }
*/
    }
}
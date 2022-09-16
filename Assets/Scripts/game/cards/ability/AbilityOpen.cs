using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using game.cards.managers;
using game.managers;
using UnityEngine;

namespace game.cards.ability
{
    public class AbilityOpen : CardAbility
    {
        [Header("Settings")]
        [SerializeField] int pairsOpen;
        [SerializeField] float delayBeforeOpen;
        [SerializeField] float enableTurnsDelay = 1;

        public override void UseAbility()
        {
            StartCoroutine(Use());
        }

        IEnumerator Use()
        {
            DisableTurns();

            PlayVFX();
            yield return new WaitForSeconds(0.6f);
            PlaySound(UseSound);

            yield return new WaitUntil(() => VfxFinish);


            #region DoAction

            var opened = 0;
            var cardToOpen = new List<Card>();
            // var closedPairs = ((List<Pair>) PairsManager.Instance.Close).Where(pair =>
            //     CardOrientationManager.Instance.GetCardState(pair.card1) == CardOrientation.Back &&
            //     CardOrientationManager.Instance.GetCardState(pair.card2) == CardOrientation.Back).ToList();
            var closed =  PairsManager.Instance.Close.ToList();
            if (closed.Count > 0)
            {
                var shuffled = closed.OrderBy(x => Guid.NewGuid()).ToList();
                foreach (var pair in shuffled)
                {
                    if (opened < pairsOpen)
                    {
                        opened++;
                        cardToOpen.Add(pair.card1);
                        cardToOpen.Add(pair.card2);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(delayBeforeOpen);
            foreach (var card in cardToOpen)
            {
                Flip(card);
            }

            #endregion

            yield return new WaitForSeconds(enableTurnsDelay);
            Finish();

            EnableTurns();
            VfxFinish = false;
        }

        void Flip(Card card) => Events.Instance.Flip(card, true, Ease.OutElastic);
    }
}
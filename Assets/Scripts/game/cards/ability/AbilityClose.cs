using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using game.cards.managers;
using game.managers;
using UnityEngine;

namespace game.cards.ability
{
    public class AbilityClose : CardAbility
    {
        [Header("Settings")]
        [SerializeField] float delayBeforeClose;
        [SerializeField] float enableTurnsDelay = 1;

        public override void UseAbility()
        {
            StartCoroutine(Use());
        }

        IEnumerator Use()
        {
            DisableTurns();

            PlayVFX();
        //  AudioManager.Instance.SubduedSnapshot(0.1f);
            yield return new WaitForSeconds(0.3f);
            PlaySound(UseSound);

            yield return new WaitUntil(() => VfxFinish);


            #region Close

            var cardToClose = new List<Card>();
            var openedPairs = PairsManager.Instance.Open.Where(p => p.cardType != Card1.Data).ToList();

            if (openedPairs.Count > 0)
            {
                var shuffled = openedPairs.OrderBy(x => Guid.NewGuid()).ToList();
                foreach (var pair in shuffled)
                {
                    // var myPair = IsMyPair(item.card1, item.card2);
                    // if (myPair) continue;

                    cardToClose.Add(pair.card1);
                    cardToClose.Add(pair.card2);
                }
            }

            yield return new WaitForSeconds(delayBeforeClose);
            foreach (var card in cardToClose)
            {
                card.Fail();
                FlipBack(card);
            }

            #endregion

            yield return new WaitForSeconds(enableTurnsDelay);
            Finish();

            EnableTurns();
            yield return new WaitForSeconds(1);
         //   AudioManager.Instance.NormalSnapshot(3f);
        }

        // bool IsMyPair(Card card1, Card card2) =>
        //     Card1 == card1 || Card1 == card2;

        void FlipBack(Card card)
        {
            Events.Instance.FlipBack(card, false);
            StartCoroutine(EnablePetCoin(card));
        }

        IEnumerator EnablePetCoin(Card card)
        {
            yield return new WaitForSeconds(Flipper.Instance.RotationTimeBack);
            card.Back();
        }
    }
}
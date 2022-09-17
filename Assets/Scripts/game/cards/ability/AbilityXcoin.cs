using System;
using game.managers;
using UnityEngine;


namespace game.cards.ability
{
    public class AbilityXcoin : CardAbility
    {
        [SerializeField] float coinMultiplier = 2;

        public override void UseAbility()
        {
            Events.Instance.OnCombine += OnCombine;
        }

        void OnCombine(Card c1, Card c2)
        {
            Events.Instance.OnCombine -= OnCombine;
            Events.Instance.CreateCoins(Card1, Card2);
            Finish(false);
        }


        // public override void UseAbility()
        // {
        //     Events.Instance.AddScoreMultiplier(coinMultiplier);
        //     Events.Instance.OnRoundWin += OnRoundWin;
        //     Finish(false);
        // }
        //
        // void OnRoundWin(int totalPairs)
        // {
        //    Events.Instance.RemoveScoreMultiplier(coinMultiplier);
        //    Events.Instance.OnRoundWin -= OnRoundWin;
        //    Return();
        // }
    }
}
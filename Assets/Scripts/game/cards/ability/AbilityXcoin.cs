using game.managers;
using UnityEngine;


namespace game.cards.ability
{
    public class AbilityXcoin : CardAbility
    {
        [SerializeField] float coinMultiplier = 2;

        public override void UseAbility()
        {
            Events.Instance.AddScoreMultiplier(coinMultiplier);
            Events.Instance.OnRoundWin += OnRoundWin;
            Finish(false);
        }

        void OnRoundWin(int totalPairs)
        {
           Events.Instance.RemoveScoreMultiplier(coinMultiplier);
           Events.Instance.OnRoundWin -= OnRoundWin;
           Return();
        }
    }
}
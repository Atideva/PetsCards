using game.managers;
using UnityEngine;

namespace game.potions.ability
{
    public class HealSmall : MonoBehaviour, IPotionAbility
    {

        public void UsePotionAbility()
        {
            Events.Instance.Hitpoints_Heal(1);
        }
 
    }
}

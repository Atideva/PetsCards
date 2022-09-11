using UnityEngine;

namespace game.potions
{
    public class Potion : MonoBehaviour
    {

        IPotionAbility _potionAbility;

        void Start() => _potionAbility = GetComponent<IPotionAbility>();

        public void UsePotion() => _potionAbility.UsePotionAbility();

    }
}

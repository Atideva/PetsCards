
using UnityEngine;

namespace game.cards.glow
{
    public class Inactive : MonoBehaviour
    {
        // [SerializeField] Color inactiveColor;
        // [SerializeField] Color activeColor;
        [SerializeField] SpriteRenderer front;
        [SerializeField] SpriteRenderer back;
        // [SerializeField] float animTime;
        
        public void Innactive()
        {
            front.enabled = true;
            back.enabled = true;
        }

        public void Active()
        {
            front.enabled = false;
            back.enabled = false;
        }

    }
}

using TMPro;
using UnityEngine;

namespace UI.shop
{
    public class SlotPrice : MonoBehaviour
    {
        public TextMeshProUGUI txt;

        public void SetPrice(int count)
        {
            txt.text = count.ToString();
        }
 
    }
}

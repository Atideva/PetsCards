#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;

namespace game.potions
{
    [ExecuteInEditMode]
    public class PotionsEditorAutoFinder : MonoBehaviour
    {
        public PotionBundle allPotionsBundle;
 
        void Update()
        {
            if (Application.isPlaying) return;

            FindAll();
        }

        public void FindAll()
        {
            Resources.LoadAll<PotionData>("Potions");
            PotionData[] potionsSo = Resources.FindObjectsOfTypeAll<PotionData>();
            Debug.Log("Im all potions: " + potionsSo.Length + " items  ", this);

            allPotionsBundle.potions = new List<PotionData>();
            foreach (var item in potionsSo)
            {
                allPotionsBundle.potions.Add(item);
            }
        }
    }
}

#endif
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace game.potions
{
    [CreateAssetMenu(fileName = "PotionBundle_", menuName = "Potions/New Bundle")]
    public class PotionBundle : ScriptableObject
    {
        [Header("Bundle named 'PotionsAll' will auto find all potions")]
        public List<PotionData> potions = new List<PotionData>();
        public ReadOnlyCollection<PotionData> Potions => potions.AsReadOnly();
        const string PATH = "_Data/_Potions";
        void OnEnable()
        {
#if UNITY_EDITOR
            if (name == "PotionsAll")
            {
                FindAll();
            }
#endif
        }
        public void FindAll()
        {
            potions = new List<PotionData>();
            potions.AddRange(Resources.FindObjectsOfTypeAll<PotionData>());
            Debug.Log("Im all potions: " + potions.Count + " items  ", this);
        
            // Resources.LoadAll<PotionData>("Potions");
            // PotionData[] potionsSO = Resources.FindObjectsOfTypeAll<PotionData>();
            // Debug.Log("Im all potions: " + potionsSO.Length + " items  ", this);

            // potions = new List<PotionData>();
            // foreach (var item in potionsSO)
            // {
            //     potions.Add(item);
            // }
        }
        public GameObject GetPotionPrefab(PotionData potion)
        {
            foreach (var item in potions)
            {
                if (item == potion) return item.Prefab;
            }
            return null;
        }

    }
}

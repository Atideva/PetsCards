using UnityEngine;

namespace __PUBLISH_v1.Scripts
{
    public class PetGMeditor : MonoBehaviour
    {
        [SerializeField]   GameManager singletonPrefab;

        void Awake()
        {
#if UNITY_EDITOR
            if (!GameManager.Instance)
            {
                Instantiate(singletonPrefab).FindCurrentLevelData();
            }
#endif
        }
    }
}
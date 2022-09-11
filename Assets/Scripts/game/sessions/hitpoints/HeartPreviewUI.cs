using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace game.sessions.hitpoints
{
    [ExecuteInEditMode]
    public class HeartPreviewUI : MonoBehaviour
    {
        [Header("Settings")]
        public int heartsInLine = 5;
        public int heartsLines = 2;
        public Vector2 heartSize;

        [Header("Setup")]
        public GameObject heartPrefab;
        public Transform heartContainer;

        [Header("Indicators")]
        public HeartUI[] hearts;
 

#if UNITY_EDITOR
        void Update()
        {
            hearts = heartContainer.GetComponentsInChildren<HeartUI>();

            if (hearts.Length != heartsInLine * heartsLines)
            {
                foreach (var item in hearts)
                {
                    DestroyImmediate(item.gameObject);
                }

                float x = 0, y = 0;
                for (var k = 0; k < heartsLines; k++)
                {
                    for (var i = 0; i < heartsInLine; i++)
                    {
                        var obj = PrefabUtility.InstantiatePrefab(heartPrefab) as GameObject;
                        if (obj != null)
                        {
                            obj.transform.SetParent(heartContainer);
                            var rect = obj.GetComponent<RectTransform>();
                            rect.localPosition = new Vector2(x, y);
                        }

                        x += heartSize.x;
                    }
                    y -= heartSize.y;
                    x = 0;
                }


                hearts = heartContainer.GetComponentsInChildren<HeartUI>();
            }

        }
#endif
    }
}

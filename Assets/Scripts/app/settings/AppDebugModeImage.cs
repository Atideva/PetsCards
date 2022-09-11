using UnityEngine;
using UnityEngine.UI;

namespace app.settings
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Image))]
    public class AppDebugModeImage : MonoBehaviour
    {
#if UNITY_EDITOR
        public Image debugImage;
        void Start()
        {
            debugImage = GetComponent<Image>();
        }
        void Update()
        {
            if (!debugImage) debugImage = GetComponent<Image>();
        }
#endif

    }
}

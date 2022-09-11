using UnityEngine;

namespace app.settings
{
    public class AppDebugLogger : MonoBehaviour
    {
        [Header("Disable for better performance")]
        [SerializeField] bool disableDebugLogger = true;

        void Awake()
        {
            if (disableDebugLogger) Debug.unityLogger.logEnabled = false;

#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#endif
        }
    }
}

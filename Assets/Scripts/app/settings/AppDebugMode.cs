#if UNITY_EDITOR
using app.keys;
using UnityEditor;
using UnityEngine;



namespace app.settings
{
    [ExecuteInEditMode]
    public class AppDebugMode : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static AppDebugMode Instance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("There's two insances of Singleton?", gameObject);
            }
        }

        //-------------------------------------------------------------

        #endregion

        // [Header("DEBUG MODE")] [SerializeField]
        bool _debugMode;

        const string KEY = ConstantsKeys.AppDebugMode;
        public bool DebugMode => _debugMode;

        void Start()
        {
            // debugMode = PlayerPrefs.GetInt(KEY) == 1;
            // if (debugMode)
            //     Debug.LogWarning("Debug mode: ENABLED", gameObject);
        }


        [MenuItem("Debug mode/ENABLE", false, 0)]
        static void DebugEnable()
        {
            PlayerPrefs.SetInt(KEY, 1);
            // EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        [MenuItem("Debug mode/DISABLE", false, 0)]
        static void DebugDisable()
        {
            PlayerPrefs.SetInt(KEY, 0);
            // EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

#if UNITY_EDITOR
        void LateUpdate()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            _debugMode = PlayerPrefs.GetInt(KEY) == 1;
            // int debugModeInt = debugMode ? 1 : 0;
            // PlayerPrefs.SetInt(KEY, debugModeInt);
        }

        void OnGUI()
        {
            if (!_debugMode) return;

            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 0.98f);
            style.alignment = TextAnchor.UpperCenter;
            style.fontSize = h * 3 / 100;
            style.normal.textColor = Color.red;

            string text = "DEBUG MODE";
            GUI.Label(rect, text, style);
        }

#endif
    }
}

#endif
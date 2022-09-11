using UnityEngine;

namespace systems.font_setup
{
    [ExecuteInEditMode]
    public class TextFontManager : MonoBehaviour
    {

        #region Singleton
        //-------------------------------------------------------------
        public static TextFontManager Instance;
        void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Debug.LogWarning("Two instance of singleton here", gameObject);
                //gameObject.SetActive(false);
            }
        }
        //-------------------------------------------------------------
        #endregion
 
        public TextFontData textFontData;

    }
}

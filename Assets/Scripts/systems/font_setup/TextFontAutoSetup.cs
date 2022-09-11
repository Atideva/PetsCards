using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace systems.font_setup
{
    [ExecuteInEditMode]
    public class TextFontAutoSetup : MonoBehaviour
    {
#if UNITY_EDITOR
        public bool autoFindTextsAddFontSetupComponent;
        public Text[] allTextObejcts;
        public TextMeshProUGUI[] allTextObejctsTMP;


        void Update()
        {
            if (Application.isPlaying) return;

            if (!autoFindTextsAddFontSetupComponent) return;

            allTextObejcts = Resources.FindObjectsOfTypeAll<Text>();
            if (allTextObejcts.Length > 0)
            {
                foreach (var item in allTextObejcts)
                {
                    var fontSetup = item.GetComponent<TextFontSetup>();
                    if (fontSetup) continue;
                    
                    item.gameObject.AddComponent<TextFontSetup>();
                    Debug.LogWarning("There was no FontSetup component, I added it for you, no thanks",
                        item.gameObject);
                }
            }

            allTextObejctsTMP = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
            
            if (allTextObejctsTMP.Length <= 0) return;

            foreach (var item in allTextObejctsTMP)
            {
                var fontSetup = item.GetComponent<TextFontSetup>();
                if (fontSetup) continue;
                
                item.gameObject.AddComponent<TextFontSetup>();
                Debug.LogWarning("There was no FontSetup component, I added it for you, no thanks",
                    item.gameObject);
            }
        }

#endif
    }
}
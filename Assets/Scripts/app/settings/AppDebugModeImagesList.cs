using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace app.settings
{
    [ExecuteInEditMode]
    public class AppDebugModeImagesList : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Colors")]
        public Color normalColor;
        public Color debugColor;

        [Header("Auto List")]
        public List<Image> debugImages = new List<Image>();
        AppDebugModeImage[] _debugImagesArray;

 
        void Update()
        {
            if (!AppDebugMode.Instance) return;

            debugImages = new List<Image>();
            _debugImagesArray = Resources.FindObjectsOfTypeAll<AppDebugModeImage>();
            foreach (var item in _debugImagesArray)
            {
                if (item.debugImage)
                {
                    debugImages.Add(item.debugImage);
                    Color clr = AppDebugMode.Instance.DebugMode ? debugColor : normalColor;
                    item.debugImage.color = clr;
                }
            }
        }
#endif
    }
}

using System.Collections.Generic;
using app.settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI.options
{
    [ExecuteInEditMode]
    public class OptionsPopup : MonoBehaviour
    {


        #region DEBUG MODE
#if UNITY_EDITOR
        [Header("Debug mode setup")]
        public List<Image> triggerImages = new List<Image>();
        void LateUpdate()
        {
            if (triggerImages.Count == 0) return;
            if (!AppDebugMode.Instance) return;
            foreach (var item in triggerImages)
            {
                Color clr = item.color;
                clr.a = AppDebugMode.Instance.DebugMode ? 0.07f : 0;
                item.color = clr;
            }
        }
#endif
        #endregion
 


    
    }
}
 

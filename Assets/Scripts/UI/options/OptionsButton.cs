using app.settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI.options
{
    [ExecuteInEditMode]
    public class OptionsButton : MonoBehaviour
    {
        public GameObject optionsPopup;
        bool _isActive;

        void Awake()
        {
            if (!Application.isPlaying) return;

            _isActive = false;
            optionsPopup.SetActive(false);

            var editorOffset = optionsPopup.transform.parent;
            editorOffset.transform.localPosition = Vector3.zero;
        }

        #region DEBUG MODE
#if UNITY_EDITOR
        [Header("Debug mode setup")]
        public Image triggerImage;
        void LateUpdate()
        {
            if (!triggerImage) return;
            if (!AppDebugMode.Instance) return;

            Color clr = triggerImage.color;
            clr.a = AppDebugMode.Instance.DebugMode ? 0.07f : 0;
            triggerImage.color = clr;
        }
#endif
        #endregion

        public void TooglePopup()
        {
            _isActive = !_isActive;
            optionsPopup.SetActive(_isActive);
        }
    }
}


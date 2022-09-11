using UnityEngine;

namespace UI.tabs
{
    public class TabCanvas : MonoBehaviour
    {
        [SerializeField] Tab parentTab;
        [SerializeField]
        Canvas canvas;
        [SerializeField]
        Transform editorOffset;

        void Awake()
        {
            parentTab.OnTabEnabled += Enable;
            parentTab.OnTabDisabled += Disable;
            Disable();
            ResetOffset();
        }

        void ResetOffset()
        {
            if (editorOffset)
            {
                editorOffset.localPosition = Vector3.zero;
            }
        }

        void Disable()
        {
            canvas.enabled = false;
        }

        void Enable()
        {
            canvas.enabled = true;
        }
    }
}
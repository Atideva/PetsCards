using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace systems.font_setup
{
    [ExecuteInEditMode]
    public class TextFontSetup : MonoBehaviour
    {
        [Header("Setup manualy for beter perfomance")]
        public Text txt;
        public TextMeshProUGUI txtTMP;


        void Start()
        {
            if (!txt) txt = GetComponent<Text>();
            if (!txtTMP) txtTMP = GetComponent<TextMeshProUGUI>();
            if (txt || txtTMP) TrySetupFont();
        }


        void TrySetupFont()
        {
            if (TextFontManager.Instance && TextFontManager.Instance.textFontData)
            {
                if (txt) txt.font = TextFontManager.Instance.textFontData.appTextFont;
                if (txtTMP) txtTMP.font = TextFontManager.Instance.textFontData.appTextFontTMP;
            }
        }


    }
}

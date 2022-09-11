using TMPro;
using UnityEngine;

namespace systems.font_setup
{
    [CreateAssetMenu(fileName = "App text font", menuName = "Data/App text font")]
    public class TextFontData : ScriptableObject
    {
        public Font appTextFont;
        public TMP_FontAsset appTextFontTMP;
    }
}

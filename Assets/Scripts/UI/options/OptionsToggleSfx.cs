using app.keys;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI.options
{
    public class OptionsToggleSfx : MonoBehaviour
    {
        [SerializeField] SoundData clickSound;
        [Header("Mixer group")]
        public AudioMixerGroup mixer;

        [Header("Setup icon")]
        public Image icon;
        public Image back;
        public Color backColor;
        public Color backOffColor;
        
        bool _isActive;
        const string KEY = ConstantsKeys.AudioMixerVolumeSfx; //SFX
        const string FIRSTTIME = ConstantsKeys.AppFirsttimeLaunch;

        void Start()
        {
            bool first = !PlayerPrefs.HasKey(FIRSTTIME + KEY);
            if (first) PlayerPrefs.SetInt(FIRSTTIME + KEY, 1);
            _isActive = first || PlayerPrefs.GetFloat(KEY) == 1;

            ToggleVolume(_isActive);
            Invoke(nameof(Loaded),0.1f);
        }
        void Loaded()
        {
            isLoaded = true;
            
        }
        bool isLoaded;

        public void Toggle()
        {
            if (isLoaded) AudioManager.Instance.PlaySound(clickSound);
            _isActive = !_isActive;
            ToggleVolume(_isActive);
        }
        void ToggleVolume(bool act)
        {
            SetupVolume(act);
            SaveData(act);
            ToogleImageColor(act);
        }


        void SaveData(bool act) => PlayerPrefs.SetFloat(KEY, act ? 1 : 0);
        void SetupVolume(bool act) => mixer.audioMixer.SetFloat(KEY, act ? 0 : -80f);
        void ToogleImageColor(bool on)
        {
            back.color = on ? backColor : backOffColor;
            var clr = icon.color;
            clr.a = on ? 0.85f : 1f;
            icon.color = clr;
        }
    }
}
 

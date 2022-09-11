using UnityEngine;
using UnityEngine.Audio;

namespace systems.audio_manager.audio_Event
{
    public class MusicVolume : MonoBehaviour
    {
        string _key = "VolumeMusic";
        public AudioMixerGroup mixer;
        [Range(0f, 1)] public float defaultVolume;

        void Start()
        {
            //float vol = PlayerPrefs.GetFloat(key);
            //vol = Mathf.Clamp(vol,0, defaultVolume);
            float vol = defaultVolume;
            SetVolume(vol);
        }
        void SetVolume(float vol)
        {
            // nice logarifmyc function to smoothly setup volume, not linear.PROBLEM IS : when is Log10(0) = ERROR! so setup it manualy trhought other func
            float calc = 0;
            if (vol != 0) calc = Mathf.Log10(vol) * 20f;
            mixer.audioMixer.SetFloat(_key, calc);
            PlayerPrefs.SetFloat(_key, vol);
        }

        void ToggleVolume(bool act)
        {
            float calcVol = act ? 0 : -80f;
            mixer.audioMixer.SetFloat(_key, calcVol);
        }


    }
}

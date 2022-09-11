using systems.audio_manager.audio_Event;
using UnityEngine;

namespace systems.audio_manager.audio_Manager
{
    public class AudioManagerTest : MonoBehaviour
    {
        public bool enableTestSound;
        public bool enableTestMusic;
        public AudioClip music;
        public SoundData soundByMouseClick;
        [Range(0, 1)] public float volume = 1f;
        void Start()
        {
            if (enableTestMusic)
            {
                Invoke(nameof(PlayMusic),0.2f);
            }

            if (!enableTestSound && !enableTestMusic)
            {
                enabled = false;
            }
        }

        void PlayMusic()
        {
            AudioManager.Instance.PlayMusic(music, volume,10f);
        }

        void Update()
        {
            if (!enableTestSound) return;
            if (Input.GetMouseButtonDown(0))
                AudioManager.Instance.PlaySound(soundByMouseClick, 1);
        }



    }
}
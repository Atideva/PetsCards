using systems.audio_manager.audio_Manager;
using UnityEngine;
using UnityEngine.Audio;

namespace systems.audio_manager
{
    public class AudioManagerAutoCreator : MonoBehaviour
    {
        [SerializeField] AudioManagerSource poolPrefab;
        [SerializeField] AudioMixerGroup musicMixerGroup;
        [SerializeField] AudioMixerGroup sfxMixerGroup;
        [SerializeField] AudioMixerSnapshot normal;
        [SerializeField] AudioMixerSnapshot subdued;
        [SerializeField] AudioMixerSnapshot timeslow;

        void Start()
        {
            if (AudioManager.Instance) return;

            var obj = new GameObject
            {
                name = "AudioManager"
            };

            var pool = (AudioManagerPool) obj.AddComponent(typeof(AudioManagerPool));
            pool.SetPrefab(poolPrefab);

            var am = (AudioManager) obj.AddComponent(typeof(AudioManager));
            am.Init(pool, musicMixerGroup, sfxMixerGroup, normal, subdued, timeslow);
        }
    }
}
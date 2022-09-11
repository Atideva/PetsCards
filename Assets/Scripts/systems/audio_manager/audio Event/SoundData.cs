using systems.audio_manager.audio_Manager;
using systems.audio_manager.ranged_float;
using UnityEngine;
 

namespace systems.audio_manager.audio_Event
{
    [CreateAssetMenu(menuName = "Audio Events/AudioEvent")]
    public class SoundData : ScriptableObject
    {
        public AudioClip[] sounds;
        [Header("Settings")] public RangedFloat volume;
        [MinMaxRange(0, 3)] public RangedFloat pitch;
        [SerializeField] float maximumFrequency = 0f;
        bool SoundIsBlocked(int id) => AudioManager.Instance.ClipIsBlocked(sounds[id], maximumFrequency);
        public float Volume => Random.Range(volume.minValue, volume.maxValue);
        public float Pitch => Random.Range(pitch.minValue, pitch.maxValue);
        public AudioClip GetClip()
        {
            if (sounds.Length == 0)
            {
                return null;
            }

            var r = Random.Range(0, sounds.Length);
            return sounds[r];
        }

        public void Play(AudioSource source)
        {
            if (sounds.Length == 0) return;
            var id = Random.Range(0, sounds.Length);
            if (SoundIsBlocked(id)) return;

            source.volume = Volume;
            source.pitch = Pitch;
            source.clip = sounds[id];
            source.Play();
        }



        public void EditorTest(AudioSource source)
        {
            var id = Random.Range(0, sounds.Length);

            source.volume = Random.Range(volume.minValue, volume.maxValue);
            source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
            source.PlayOneShot(sounds[id]);
        }
    }
}
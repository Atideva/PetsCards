using System.Collections;
using System.Collections.Generic;
using systems.audio_manager.audio_Event;
using UnityEngine;
using UnityEngine.Audio;

namespace systems.audio_manager.audio_Manager
{
    public class AudioManager : MonoBehaviour
    {
        #region Singleton

        public static AudioManager Instance;

        #endregion

        #region Init

        [Header("Mixers")]
        [SerializeField] AudioMixerGroup musicMixerGroup;
        [SerializeField] AudioMixerGroup sfxMixerGroup;
        [SerializeField] AudioMixerSnapshot normal;
        [SerializeField] AudioMixerSnapshot subdued;
        [SerializeField] AudioMixerSnapshot timeslow;
        [SerializeField] float musicFadeTransitionMult = 0.6f;
        AudioManagerPool _pool;
        AudioSource _musicSource;
        AudioSource _musicSource2;
        AudioSource _currentMusicSource;
        SoundData _currentMusic;
        List<AudioClip> _frequencyList = new List<AudioClip>();

        #endregion

        public void Init(AudioManagerPool pool, AudioMixerGroup musicMixer, AudioMixerGroup sfxMixer,
            AudioMixerSnapshot normalSnapshot,
            AudioMixerSnapshot subduedSnapshot, AudioMixerSnapshot timeSlow)
        {
            musicMixerGroup = musicMixer;
            sfxMixerGroup = sfxMixer;
            normal = normalSnapshot;
            subdued = subduedSnapshot;
            timeslow = timeSlow;

            _pool = pool;
            _pool.SetSfxMixer(sfxMixerGroup);
            _musicSource.outputAudioMixerGroup = musicMixerGroup;
            _musicSource2.outputAudioMixerGroup = musicMixerGroup;
        }


        void Awake()
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.outputAudioMixerGroup = musicMixerGroup;

            _musicSource2 = gameObject.AddComponent<AudioSource>();
            _musicSource2.loop = true;
            _musicSource2.outputAudioMixerGroup = musicMixerGroup;

            _currentMusicSource = _musicSource;

            if (!_pool) _pool = GetComponent<AudioManagerPool>();
            if (_pool) _pool.SetSfxMixer(sfxMixerGroup);

            if (Instance == null)
            {
                Instance = this;
                if (transform.parent) transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
                Debug.Log("<color=yellow>Audio Manager</color> инициализирован и сделан DontDestroyOnLoad", gameObject);
            }
            else
            {
                Debug.Log("не стоит иметь несколько <color=yellow>Audio Manager</color> на сцене",
                    gameObject);
                gameObject.SetActive(false);
            }
        }

        public void NormalSnapshot(float transition = 1f) => normal.TransitionTo(transition);
        public void SubduedSnapshot(float transition = 1f) => subdued.TransitionTo(transition);
        public void TimeSlowSnapshot(float transition = 1f) => timeslow.TransitionTo(transition);

        public void PlayMusic(SoundData musicData, float vol = 1, float transitionTime = 1)
        {
            _currentMusic = musicData;
            StartCoroutine(UpdateMusicWithFade(musicData.GetClip(), vol, transitionTime));
        }

        public void PlayMusic(AudioClip musicClip, float vol = 1, float transitionTime = 1)
        {
            _currentMusic = null;
            StartCoroutine(UpdateMusicWithFade(musicClip, vol, transitionTime));
        }

        public void StopMusic(float fadeTime = 1) => StartCoroutine(StopMusicFade(fadeTime));

        public void SetMusicVolume(float volume, float fadeTime = 1)
        {
            StartCoroutine(MusicVolume(volume, fadeTime));
        }

        IEnumerator MusicVolume(float newVolume, float fadeTime = 1)
        {
            var maxVolume = _currentMusic ? _currentMusic.Volume : 1;
            var volume = _currentMusicSource.volume;
            var dir = newVolume - volume > 0 ? 1f : -1f;
            while (Mathf.Abs(newVolume - volume) > 0.01f)
            {
                volume += Time.deltaTime * dir / fadeTime;
                _currentMusicSource.volume = volume * maxVolume;
                yield return null;
            }
            _currentMusicSource.volume = newVolume * maxVolume;
        }

        IEnumerator StopMusicFade(float fadeTime = 1)
        {
            var volume = _currentMusicSource.volume;
            while (volume > 0)
            {
                volume -= Time.deltaTime / fadeTime;
                _currentMusicSource.volume = volume;
                yield return null;
            }

            _currentMusicSource.Stop();
        }


        IEnumerator UpdateMusicWithFade(AudioClip music, float musicVolume, float transitionTime)
        {
            if (_currentMusicSource.clip == music)
            {
                SetMusicVolume(musicVolume);
                yield break;
            }

            var lastSource = _currentMusicSource;
            _currentMusicSource = _currentMusicSource == _musicSource ? _musicSource2 : _musicSource;
            _currentMusicSource.clip = music;
            _currentMusicSource.volume = 0;
            _currentMusicSource.Play();

            var timer = 0f;
            var lastVolume = lastSource.volume;
            while (timer < transitionTime)
            {
                timer += Time.deltaTime;
                lastSource.volume -= lastVolume * Time.deltaTime / (transitionTime * musicFadeTransitionMult);
                _currentMusicSource.volume += musicVolume * Time.deltaTime / transitionTime;
                yield return null;
            }


            //
            // float volume = 0;
            // if (_musicSource.isPlaying)
            // {
            //     //FADE MUSIC
            //     volume = _musicSource.volume;
            //     while (volume > 0)
            //     {
            //         volume -= Time.deltaTime / (transitionTime * 0.5f);
            //         _musicSource.volume = volume;
            //         yield return null;
            //     }
            //
            //     _musicSource.Stop();
            // }
            //
            // _musicSource.clip = music;
            // _musicSource.Play();
            //
            // volume = 0f;
            // while (volume < musicVolume)
            // {
            //     //INCREASE VOLUME
            //     volume += Time.deltaTime / (transitionTime * 0.5f);
            //     _musicSource.volume = volume;
            //     yield return null;
            // }
            //
            // _musicSource.volume = musicVolume;
        }

        public void PlaySound(SoundData sound)
        {
            if (!sound) return;
            var src = _pool.GetSource();
            src.spatialBlend = 0;
            sound.Play(src);
        }

        public void PlaySound(SoundData sound, float delay)
        {
            if (!sound) return;
            StartCoroutine(PlaySoundDelayed(sound, delay));
        }

        IEnumerator PlaySoundDelayed(SoundData sound, float delay)
        {
            yield return new WaitForSeconds(delay);
            PlaySound(sound);
        }

        public void PlaySound3D(SoundData sound, Vector3 soundPosition)
        {
            if (!sound) return;
            var src = _pool.GetSource();
            src.spatialBlend = 1;
            src.transform.position = soundPosition;
            sound.Play(src);
        }

        public void PlayAudioClip(AudioClip clip, float volume = 1)
        {
            var src = _pool.GetSource();
            src.clip = clip;
            src.volume = volume;
            src.spatialBlend = 0;
            src.Play();
        }

        public bool ClipIsBlocked(AudioClip clip, float sfxFreq)
        {
            if (_frequencyList.Contains(clip))
                return true;
            else
            {
                StartCoroutine(FreqListJob(clip, sfxFreq));
                return false;
            }
        }

        IEnumerator FreqListJob(AudioClip clip, float freq)
        {
            _frequencyList.Add(clip);
            yield return new WaitForSeconds(freq);
            _frequencyList.Remove(clip);
        }
    }
}
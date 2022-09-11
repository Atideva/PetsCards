using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace systems.audio_manager.audio_Manager
{
    public class AudioManagerPool : MonoBehaviour
    {
        #region Init

        [SerializeField]
        AudioManagerSource prefab ;
        readonly Queue<AudioSource> _myQueue = new  ();
        public  AudioMixerGroup _sfxMixerGroup;
        public void SetSfxMixer(AudioMixerGroup mixer) => _sfxMixerGroup = mixer;

        #region Editor

#if (UNITY_EDITOR)
        int _poolLenght = 0;
        string _defaultName;
        void Start() => _defaultName = name;
#endif

        #endregion

        #endregion

        public void SetPrefab(AudioManagerSource newPrefab)
        {
            prefab = newPrefab;
        }
        public AudioSource GetSource()
        {
            if (_myQueue.Count <= 0) return CreateSource();

            var src = _myQueue.Dequeue();
            src.gameObject.SetActive(true);
            return src;
        }

        AudioSource CreateSource()
        {
            var src = Instantiate(prefab, transform);
            src.audioSource.outputAudioMixerGroup = _sfxMixerGroup;
            src.Init(this);

            #region Editor

#if (UNITY_EDITOR)
            //Write number of pooled object to the name of Pool (n).
            _poolLenght++;
            gameObject.name = _defaultName + " (" + _poolLenght + ")";
#endif

            #endregion

            return src.audioSource;
        }

        public void ReturnToPool(AudioSource src)
        {
            _myQueue.Enqueue(src);
            src.gameObject.SetActive(false);
        }
    }
}
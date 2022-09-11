using System.Collections;
using UnityEngine;

namespace systems.audio_manager.audio_Manager
{
    public class AudioManagerSource : MonoBehaviour
    {
        public AudioSource audioSource;
        public void Init(AudioManagerPool pool) => _myPool = pool;
    
        AudioManagerPool _myPool;
        void OnEnable() => StartCoroutine(Kostil());

        IEnumerator Kostil()
        {
            yield return new WaitForEndOfFrame();
            var clp = audioSource.clip;
            var delay = clp == null ? 1f : clp.length / audioSource.pitch;
            yield return new WaitForSeconds(delay * Time.timeScale);
            _myPool.ReturnToPool(audioSource);
        }
    }
}
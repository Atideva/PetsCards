using __PUBLISH_v1.Scripts;
using game.managers;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;

namespace game.levels
{
    public class Lose : MonoBehaviour
    {
        public float delay1;
        public float delay2;
        public float delay3;
        public float musicFadetime;
        [SerializeField] SoundData loseSound;
        [SerializeField] SoundData loseSound2;
        [SerializeField] LosePopupUI loseUI;
        [Header("TEST")]
        public bool testShow;
        public bool testHide;
#if UNITY_EDITOR
        void Update()
        {
            if (testShow)
            {
                testShow = false;
                GameOver();
            }

            if (testHide)
            {
                testHide = false;
                loseUI.Hide();
                if (musicVolume >= 0) AudioManager.Instance.SetMusicVolume(1);
            }
        }
#endif
        void Start()
        {
            Events.Instance.OnLose += GameOver;
        }

        public float musicVolume;

        void GameOver()
        {
            if (musicVolume >= 0) AudioManager.Instance.SetMusicVolume(musicVolume, musicFadetime);
            if (delay1 >= 0) AudioManager.Instance.PlaySound(loseSound, delay1);
            if (delay2 >= 0) AudioManager.Instance.PlaySound(loseSound2, delay2);
            Invoke(nameof(sho), delay3);
        }

        void sho() => loseUI.Show();
    }
}
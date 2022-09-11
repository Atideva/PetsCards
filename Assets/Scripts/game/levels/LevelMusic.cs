using System.Collections;
using __PUBLISH_v1.Scripts;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;

namespace game.levels
{
    public class LevelMusic : MonoBehaviour
    {
        [Header("Transition")]
        [SerializeField] float transitionTime = 5f;
        [Header("Custom music")]
        [SerializeField] bool useCustomMusic;
        [SerializeField] SoundData customMusic;
        [Header("DEBUG")]
        [SerializeField] SoundData currentMusic;
        [SerializeField] [Range(0, 1)] float currentVolume;


        IEnumerator Start()
        {
            if (useCustomMusic && customMusic)
                currentMusic = customMusic;
            else
            {
                var mode = GameManager.Instance.CurrentLevel.Mode;
                var config = GameManager.Instance.Config.Music;
                currentMusic = GameManager.Instance.IsPvP
                    ? config.MusicPvp
                    : GameManager.Instance.IsMainMenu
                        ? config.MusicMainMenu
                        : config.GetCLip(mode);
            }

            currentVolume = currentMusic.Volume;
            yield return null;
            AudioManager.Instance.PlayMusic(currentMusic, currentVolume, transitionTime);
        }
    }
}
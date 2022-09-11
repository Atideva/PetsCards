using System.Collections;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;

namespace systems.level_loader
{
    public class LoaderMenu : MonoBehaviour
    {
        [Header("Use loading scene")]
        public bool useLoadScene;

        [Header("Scenes to load")]
        public LevelList levelData;

        [Header("[ Inspector ]")]
        public int choosenLevel = 1;


        [Header("Click sound")]
        public SoundData clickSound;
        public float loadDelay;
        bool _loadStarted;

        public void LoadLevel(int levelNumer)
        {
            if (loadDelay == 0)
                Load(levelNumer);
            else
            {
                if (!_loadStarted)
                    StartCoroutine(DelayedLoad(levelNumer));
            }
        }
        

        IEnumerator DelayedLoad(int levelNumer)
        {
            _loadStarted = true;
            AudioManager.Instance.PlaySound(clickSound);
            yield return new WaitForSeconds(loadDelay);
            Load(levelNumer);
        }
        void Load(int levelNumer)
        {
            var levelId = levelNumer - 1;
            var sceneName = "";

            //TODO: убрал старую систему
            // if (levelData.levelNames.Count > levelId)
            //     sceneName = levelData.levelNames[levelId];
            // else
            // {
            //     Debug.LogError("There's no scene with such ID in data");
            //     return;
            // }

            Loader.Load(sceneName, useLoadScene);
        }
        public void LoadChoosenLevel()
        {
            LoadLevel(choosenLevel);
        }


    }
}

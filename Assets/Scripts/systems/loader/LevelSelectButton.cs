using app.settings;
using game.managers;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;
using UnityEngine.UI;

namespace systems.level_loader
{
    [ExecuteInEditMode]
    public class LevelSelectButton : MonoBehaviour
    {
        [Header("Mode")]
        public bool loadLevelOnClick;

        [Header("Load level number")]
        [SerializeField] int lvlNumber;

        [Header("Loader setup")]
        [SerializeField] LoaderMenu loaderMenu;

        [Header("Click sound")]
        public SoundData clickSound;

        public GameObject txt;

        #region DEBUG MODE
#if UNITY_EDITOR
        [Header("Debug mode setup")]
        public Image triggerImage;
        void Update()
        {
            if (!triggerImage) return;
            if (!AppDebugMode.Instance) return;

            Color clr = triggerImage.color;
            clr.a = AppDebugMode.Instance.DebugMode ? 0.07f : 0;
            triggerImage.color = clr;
        }
#endif
        #endregion


        void Start()
        {
            if (Application.isPlaying) Invoke(nameof(Delayed), 0.1f);
            txt.SetActive(false);
        }
        void Delayed()
        {
        //    if (Events.Instance) Events.Instance.OnLevelSelected += LevelSelected;
        }
        void LevelSelected(GameObject obj)
        {
            bool act = obj == gameObject  ;
            txt.SetActive(act);
        }

        public void Clicked()
        {
          //  Events.Instance.LevelSelected(gameObject);
            AudioManager.Instance.PlaySound(clickSound);
            if (loadLevelOnClick) Invoke(nameof(Load), 0.3f);
            else loaderMenu.choosenLevel = lvlNumber;
        }
        void Load() => loaderMenu.LoadLevel(lvlNumber);
    }
}

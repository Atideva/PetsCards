using System.Collections;
using System.Linq;
using fromWordSearch;
using game.player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace __PUBLISH_v1.Scripts
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static GameManager Instance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                AwakeInit();
            }
            else
            {
                Debug.LogError("Two singletons", gameObject);
                gameObject.SetActive(false);
            }
        }

        //-------------------------------------------------------------

        #endregion

        [Header("Config")]
        [SerializeField] GameConfig config;
        [SerializeField] RuntimeData runtimeData;
        [SerializeField] LevelConfig mainMenu;
        [Header("Setup")]
        [SerializeField] LevelsSave levelsData;
        [SerializeField] UserResourceSave userResourceSave;
        [SerializeField] CardsSave cardData;
        [SerializeField] BlindCutout blindCutout;
        [Header("DEBUG")]
        [SerializeField] LevelConfig currentLevel;
        [SerializeField] LevelConfig undefinedLevel;
        public LevelConfig CurrentLevel => currentLevel;
        public LevelConfig NextLevel => GetNextLevel(currentLevel);
        public LevelsSave LevelsesData => levelsData;
        public GameConfig Config => config;
        public UserResourceSave UserResourceSave => userResourceSave;
        public CardsSave CardsData => cardData;
        public bool IsPvP => currentLevel.GameMode == GameMode.PlayerVsPlayer;
        public bool IsCraft => currentLevel.GameMode == GameMode.Craft;
        public bool IsMainMenu => currentLevel == mainMenu;
        bool _isLoading;
        bool _useBlind;

        void AwakeInit()
        {
            DontDestroyOnLoad(gameObject);
            levelsData.Init(config);
            userResourceSave.Init(runtimeData);
            cardData.Init(config, runtimeData);
        }


        public void OnLevelLoad()
        {
            _isLoading = false;
            if (_useBlind && config.Settings.UseBlindCutoutOnLevelLoad)
            {
                blindCutout.Hide();
            }
        }


        public void LoadMainMenu() => LoadLevel(mainMenu, 0, false);

        public void RestartLevel() => LoadLevel(currentLevel);

        public void LoadNextLevel(float delay = 0, bool useBlind = true) => LoadLevel(NextLevel, delay, useBlind);


        public void LoadLevel(LevelConfig lvl, float delay = 0, bool useBlind = true)
        {
            if (_isLoading) return;
            if (!lvl) return;
            currentLevel = lvl;
            StartCoroutine(Load(lvl, delay, useBlind));
        }


        IEnumerator Load(LevelConfig lvl, float delay = 0, bool useBlind = true)
        {
            _isLoading = true;
            yield return null;
            yield return new WaitForSeconds(delay);
            if (useBlind && config.Settings.UseBlindCutoutOnLevelLoad)
            {
                _useBlind = true;
                blindCutout.Show();
                yield return new WaitForSeconds(blindCutout.ShowTime);
            }
            else _useBlind = false;

            SceneManager.LoadScene(lvl.SceneToLoad);
        }

        public void FindCurrentLevelData()
        {
            var campaign =
                Config.CampaignLevels.levels.FirstOrDefault(l => l.SceneToLoad == SceneManager.GetActiveScene().name);
            var other = Config.OtherLevel.levels.FirstOrDefault(l => l.SceneToLoad == SceneManager.GetActiveScene().name);
            var curLvl = campaign
                ? campaign
                : other
                    ? other
                    : undefinedLevel;
            SetCurrentLevel(curLvl);
        }

        public void LevelComplete() => levelsData.LevelComplete(currentLevel);
        void SetCurrentLevel(LevelConfig lvl) => currentLevel = lvl;
        public LevelData FindData(LevelConfig lvl) => levelsData.FindData(lvl);

        LevelConfig GetNextLevel(LevelConfig curLevel)
        {
            var all = levelsData.allLevel.levels;
            var cur = all.IndexOf(curLevel);
            return cur + 1 < all.Count ? all[cur + 1] : null;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using __PUBLISH_v1.Scripts;
using fromWordSearch;
using game.player;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[ExecuteInEditMode]
public class LevelSelecterUI : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] Transform containerUI;
    [SerializeField] SoundData loadLevelSound;
    [SerializeField] LevelSelectItemUI levelItemPrefab;
    [SerializeField] Color cardNoneColor;
    [SerializeField] Color cardLockColor;
    [SerializeField] Color cardCompleteColor;
    [SerializeField] Color timerColor;
    [SerializeField] Color livesColor;
    [SerializeField] LevelLockPopup lockPopup;
    [SerializeField] Button playButton;

    [Header("DEBUG")]
    [SerializeField] LevelsSave levelsSave;
    [SerializeField] GameConfig gameConfig;

    List<LevelConfig> _levels = new();

    [Header("IN EDITOR REFRESH")]
    public bool refreshUI;
    [Header("status")]
    public bool _isLock;
    public bool _isComplete;

    [Header("mode")]
    public bool isTimer;
    public bool isLives;

    void LevelClick(LevelConfig lvl, LevelData data)
    {
        if (data == null || data.isLock)
        {
            lockPopup.Show();
        }
        else
        {
            AudioManager.Instance.PlaySound(loadLevelSound);
            GameManager.Instance.LoadLevel(lvl);
        }
    }

    IEnumerator Start()
    {
        if (Application.isPlaying)
        {
            while (!GameManager.Instance)
            {
                yield return null;
            }

            levelsSave = GameManager.Instance.LevelsesData;
            gameConfig = GameManager.Instance.Config;
            _levels = gameConfig.CampaignLevels.levels;

            while (levelsSave.WaitForInit)
            {
                yield return null;
            }

            CreateItems();
            playButton.onClick.AddListener(Play);
        }
    }

    void Play()
    {
        lastItem.Click();
    }

    LevelSelectItemUI lastItem;

    void CreateItems(bool isEditorMode = false)
    {
        DestroyEditorItems();

        _lastUpdatelevels = new List<LevelConfig>();

        int number = 0;
        lastItem = null;
        int i = 0;
        foreach (var l in _levels)
        {
            number++;
            var data = levelsSave ? levelsSave.FindData(l) : null;

            if (isEditorMode)
            {
                if (data != null)
                    data.isLock = true;
                else
                {
                    data = new LevelData {level = l, isLock = _isLock, isComplete = _isComplete};
                }
            }

            var uiItem = Instantiate(levelItemPrefab, containerUI);

            uiItem.Init(l, data, cardNoneColor, cardLockColor, cardCompleteColor, timerColor, livesColor);
            uiItem.name = "Level - " + l.LvlName;
            uiItem.SetName("Level " + number);
            if (data is {isComplete: true}) uiItem.DisableName();

            if (i == 0)
            {
                i++;
                lastItem = uiItem;
            }

            if (data is {isComplete: false, isLock: false}) lastItem = uiItem;

            if (isEditorMode)
            {
                if (isTimer)
                {
                    uiItem.SetMode(LevelMode.Timer);
                }

                if (isLives)
                {
                    uiItem.SetMode(LevelMode.Lives);
                }
            }

            _lastUpdatelevels.Add(l);

            if (Application.isPlaying)
            {
                uiItem.OnClick += LevelClick; //DOES IT REQUIRE UNSUBSCRIBE ON-DISABLE method?
            }
        }


        Debug.LogWarning("LevelsUI: items created");
    }

    void DestroyEditorItems()
    {
        var levelsArray = containerUI.GetComponentsInChildren<LevelSelectItemUI>();
        foreach (var item in levelsArray)
        {
            DestroyImmediate(item.gameObject);
        }
    }


    //EDITOR LOGIC
    List<LevelConfig> _lastUpdatelevels = new();
#if UNITY_EDITOR
    void Update()
    {
        if (!Application.isPlaying)
        {
            var updateRequired = false;

            if (_lastUpdatelevels.Count != _levels.Count)
            {
                updateRequired = true;
            }
            else
            {
                for (int i = 0; i < _levels.Count; i++)
                {
                    if (_lastUpdatelevels[i] != _levels[i])
                    {
                        updateRequired = true;
                        break;
                    }
                }
            }

            if (updateRequired || refreshUI)
            {
                refreshUI = false;
                _levels = new List<LevelConfig>();
                _levels = gameConfig.CampaignLevels.levels;
                CreateItems(true);
            }
        }
    }
#endif
}
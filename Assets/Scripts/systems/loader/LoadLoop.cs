using System;
using System.Collections;
using app.keys;
using game.managers;
using systems.level_loader;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLoop : MonoBehaviour
{
    [SerializeField] private bool dontDestroyOnLoad;
    [Header("Scenes to load")]
    public LevelList levelData;
    [SerializeField] private PlayButton playButton;

    private void Awake()
    {
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }

        playButton.OnButtonClick += Load;
    }
    
    
    private void Start()
    {
   
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        StartCoroutine(Subscribe());
    }

    IEnumerator Subscribe()
    {
        yield return new WaitForSeconds(1f);
        Events.Instance.OnLoadNextLevel += LoadNext;
    }
    private void LoadNext()
    {
        var levelId = PlayerPrefs.GetInt(ConstantsKeys.TEMP_LEVEL_COUNT_PASS);
        
        levelId++;
        
        //TODO: убрал старую систему
            //  if (levelId >= levelData.levelNames.Count) levelId = 0;
        
        PlayerPrefs.SetInt(ConstantsKeys.TEMP_LEVEL_COUNT_PASS, levelId);
        //TODO: убрал старую систему
     //   var sceneName = levelData.levelNames[levelId];
      //  Loader.Load(sceneName, false);
    }

    void Load()
    {
        var levelId = 0;

        if (PlayerPrefs.HasKey(ConstantsKeys.TEMP_LEVEL_COUNT_PASS))
            levelId = PlayerPrefs.GetInt(ConstantsKeys.TEMP_LEVEL_COUNT_PASS);
        else
            PlayerPrefs.SetInt(ConstantsKeys.TEMP_LEVEL_COUNT_PASS, levelId);

        //TODO: убрал старую систему
        // if (levelId >= levelData.levelNames.Count)
        // {
        //     levelId = 0;
        //     PlayerPrefs.SetInt(ConstantsKeys.TEMP_LEVEL_COUNT_PASS, levelId);
        // }
        //TODO: убрал старую систему
     //   var sceneName = levelData.levelNames[levelId];
        Events.Instance.OnLoadNextLevel -= LoadNext;
     //   Loader.Load(sceneName, false);
    }
}
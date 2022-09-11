using app.keys;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game.levels.stars
{
    public class StarsSave : MonoBehaviour
    {
        #region Awake Singleton

        //-------------------------------------------------------------
        public static StarsSave Instance;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);

            Debug.LogWarning(
                "Make me DontDestroyOnLoad please, to prevent load huge data for all levels at each OnLevelLoad");
            AwakeJob();
        }

        //-------------------------------------------------------------

        #endregion

        public StarsData SavelevelStarsData => _savelevelStarsData;

        StarsData _savelevelStarsData;
        const string SAVE_KEY = ConstantsKeys.DataLevelPoints;
        int _thisLevelID;


        void AwakeJob() => LoadData();
        void OnLevelWasLoaded(int level) => _thisLevelID = GetLevelId(SceneManager.GetActiveScene().name);
        public void LevelStarted(StarsRequirementData starsRequirementData) => CheckLevelData(starsRequirementData);


        void LoadData() => _savelevelStarsData = PlayerPrefs.HasKey(SAVE_KEY)
            ? JsonUtility.FromJson<StarsData>(PlayerPrefs.GetString(SAVE_KEY))
            : new StarsData();

        void SaveData() => PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(_savelevelStarsData));


        void CheckLevelData(StarsRequirementData starsRequirementData)
        {
            if (_savelevelStarsData.LevelNames.Contains(starsRequirementData.LevelName))
            {
                if (IsDataLevelChanged(starsRequirementData))
                    SaveData();
            }
            else
            {
                DataAddNewLevel(starsRequirementData);
                SaveData();
            }

            StarsManager.Instance.Init();
        }

        void DataAddNewLevel(StarsRequirementData starsRequirementData)
        {
            _savelevelStarsData.SetLevelNames.Add(starsRequirementData.LevelName);

            _savelevelStarsData.SetPointsRequiredForOneStar.Add(starsRequirementData.Star1_Points);
            _savelevelStarsData.SetPointsRequiredForTwoStars.Add(starsRequirementData.Star2_Points);
            _savelevelStarsData.SetPointsRequiredForThreeStars.Add(starsRequirementData.Star3_Points);

            _savelevelStarsData.SetTimeRemainingForOneStarr.Add(starsRequirementData.Star1_TimeRemain);
            _savelevelStarsData.SetTimeRemainingForTwoStars.Add(starsRequirementData.Star2_TimeRemain);
            _savelevelStarsData.SetTimeRemainingForThreeStars.Add(starsRequirementData.Star3_TimeRemain);
        }

        bool IsDataLevelChanged(StarsRequirementData starsRequirementData) => IsDataChanged(starsRequirementData, _thisLevelID);

        bool IsDataChanged(StarsRequirementData starsRequirementData, int levelID)
        {
            bool dataWasChanged = false;

            int value = starsRequirementData.Star1_Points;
            if (_savelevelStarsData.PointsRequiredForOneStar[levelID] != value)
            {
                _savelevelStarsData.SetPointsRequiredForOneStar[levelID] = value;
                dataWasChanged = true;
            }

            value = starsRequirementData.Star2_Points;
            if (_savelevelStarsData.PointsRequiredForTwoStars[levelID] != value)
            {
                _savelevelStarsData.SetPointsRequiredForTwoStars[levelID] = value;
                dataWasChanged = true;
            }

            value = starsRequirementData.Star3_Points;
            if (_savelevelStarsData.PointsRequiredForThreeStars[levelID] != value)
            {
                _savelevelStarsData.SetPointsRequiredForThreeStars[levelID] = value;
                dataWasChanged = true;
            }

            value = starsRequirementData.Star1_TimeRemain;
            if (_savelevelStarsData.TimeRemainingForOneStar[levelID] != value)
            {
                _savelevelStarsData.SetTimeRemainingForOneStarr[levelID] = value;
                dataWasChanged = true;
            }

            value = starsRequirementData.Star2_TimeRemain;
            if (_savelevelStarsData.TimeRemainingForTwoStars[levelID] != value)
            {
                _savelevelStarsData.SetTimeRemainingForTwoStars[levelID] = value;
                dataWasChanged = true;
            }

            value = starsRequirementData.Star3_TimeRemain;
            if (_savelevelStarsData.TimeRemainingForThreeStars[levelID] != value)
            {
                _savelevelStarsData.SetTimeRemainingForThreeStars[levelID] = value;
                dataWasChanged = true;
            }

            return dataWasChanged;
        }


        public int GetLevelId(string levelName)
        {
            for (int i = 0; i < _savelevelStarsData.LevelNames.Count; i++)
            {
                if (_savelevelStarsData.SetLevelNames[i] == levelName) return i;
            }

            return -1;
        }

        public int Get_StarsEarnedForThisLevel()
        {
            if (_thisLevelID <= SavelevelStarsData.StarsEarned.Count)
            {
                Debug.LogWarning("LevelStarsDoesntLoaded");
                return -1;
            }

            if (_thisLevelID != -1) return SavelevelStarsData.StarsEarned[_thisLevelID];

            Debug.LogWarning("No such level ID in data");
            return -1;
        }

        public void Set_StarsEarnedForThisLevel(int count)
        {
            if (_thisLevelID != -1) SavelevelStarsData.SetStarsEarned[_thisLevelID] = count;
        }
    }
}
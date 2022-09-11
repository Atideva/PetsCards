using System.Collections.Generic;
using System.Linq;
using game.player;
using systems.level_loader;
using UnityEngine;

namespace fromWordSearch
{
    public class LevelsSave : MonoBehaviour
    {
        public LevelSaveData save;
        public LevelList allLevel;
        const string SAVE = "UNLOCKED_LEVELS";

        public bool WaitForInit => !_isInit;
        bool _isInit;

        public void Init(GameConfig config)
        {
            save = new LevelSaveData {levelsData = new List<LevelData>()};
            allLevel = config.CampaignLevels;
            InitData(config);
        }


        void InitData(GameConfig config)
        {
            if (PlayerPrefs.HasKey(SAVE))
            {
                Load();
                RefreshSaveData(config.BaseUnlockLevels.levels);
                Save();
            }
            else
            {
                if (config.BaseUnlockLevels.levels is {Count: > 0})
                {
                    save.levelsData = GetBaseData(config);
                    Save();
                }
            }

            _isInit = true;
        }

        void RefreshSaveData(List<LevelConfig> baseLevels)
        {
            foreach (var lvl in baseLevels)
            {
                if (save.levelsData.Exists(d => d.level == lvl))
                    save.levelsData.Find(d => d.level == lvl).isLock = false;
                else
                    save.levelsData.Add(new LevelData
                    {
                        level = lvl,
                        isLock = false,
                        isComplete = false
                    });
            }
        }

        void Save() => PlayerPrefs.SetString(SAVE, JsonUtility.ToJson(save));
        void Load() => save = JsonUtility.FromJson<LevelSaveData>(PlayerPrefs.GetString(SAVE));
        public LevelData FindData(LevelConfig lvl) => save.levelsData.FirstOrDefault(data => data.level == lvl);


        public void LevelComplete(LevelConfig lvl)
        {
            Complete(lvl);
            Unlock(GetNextLevel(lvl));
            Save();
        }


        void Complete(LevelConfig lvl)
        {
            if (!lvl) return;
            if (save.levelsData.Exists(d => d.level == lvl))
                save.levelsData[IndexOf(lvl)].isComplete = true;
            else
                save.levelsData.Add(new LevelData
                {
                    level = lvl,
                    isLock = false,
                    isComplete = true
                });
        }

        void Unlock(LevelConfig lvl)
        {
            if (!lvl) return;
            if (save.levelsData.Exists(d => d.level == lvl))
                save.levelsData[IndexOf(lvl)].isLock = false;
            else
            {
                save.levelsData.Add(new LevelData
                {
                    level = lvl,
                    isLock = false,
                    isComplete = false
                });
            }
        }

        LevelConfig GetNextLevel(LevelConfig lvl)
        {
            var all = allLevel.levels.ToList();
            if (!all.Contains(lvl)) return null;
            var index = all.IndexOf(lvl);
            return index + 1 < all.Count ? all[index + 1] : null;
        }

        int IndexOf(LevelConfig lvl)
        {
            for (var i = 0; i < save.levelsData.Count; i++)
            {
                if (save.levelsData[i].level == lvl)
                    return i;
            }

            return -1;
        }

        List<LevelData> GetBaseData(GameConfig config) =>
            config.BaseUnlockLevels.levels.Select(lvl =>
                new LevelData
                {
                    level = lvl,
                    isLock = false,
                    isComplete = false
                }).ToList();
    }
}
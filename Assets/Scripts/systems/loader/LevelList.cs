using System.Collections.Generic;
using fromWordSearch;
using UnityEngine;

namespace systems.level_loader
{
    [CreateAssetMenu(fileName = "Levels_", menuName = "Levels/Levels Data")]
    public class LevelList : ScriptableObject
    {
     //   public List<string> levelNames = new();
        public List<LevelConfig> levels = new();
    }
}
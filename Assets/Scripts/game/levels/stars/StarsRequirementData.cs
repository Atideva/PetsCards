using UnityEngine;
// ReSharper disable InconsistentNaming

namespace game.levels.stars
{
    public class StarsRequirementData
    {
        [Header("Name")]
        public string LevelName;

        [Header("Stars settings points")]
        public int Star1_Points;
        public int Star2_Points;
        public int Star3_Points;

        [Header("Stars settings time")]
        public int Star1_TimeRemain;
        public int Star2_TimeRemain;
        public int Star3_TimeRemain;
    }
}
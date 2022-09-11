using UnityEngine;

namespace app.settings
{
    [CreateAssetMenu(fileName = "Game settings", menuName = "Data/Game settings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField]   bool useBlindCutoutOnLevelLoad;
        [Header("Coins")]
        [SerializeField] int coinPerPair;
        [SerializeField] float bonusPerRow;
        [Header("Modes")]
        [SerializeField] float secondsPerPair;
        [SerializeField] float livesPerPair;
        [Header("Animation")]
        [SerializeField] float pairFlipBackDelay = 0.3f;
        [SerializeField]   float firstSessionDelay; //dont ask, let triger Sequence-> CheckLevelMode

        public int CoinPerPair => coinPerPair;
        public float ComboBonusPerRow => bonusPerRow;
        public float PairFlipBackDelay => pairFlipBackDelay;
        public float FirstSessionDelay => firstSessionDelay;
        public float SecondsPerPair => secondsPerPair;
        public float LivesPerPair => livesPerPair;

        public bool UseBlindCutoutOnLevelLoad => useBlindCutoutOnLevelLoad;
    }
}
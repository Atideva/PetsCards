using UnityEngine;
using UnityEngine.SceneManagement;

namespace game.levels.stars
{
    public class StarsConfig : MonoBehaviour
    {

        #region Singleton
        //-------------------------------------------------------------
        public static StarsConfig Instance;
        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);
        }
        //-------------------------------------------------------------
        #endregion

        [Header("Stars")]
        [SerializeField] StarsRequirement starsRequirement;
        public StarsRequirement StarsRequirement => starsRequirement;

        [Header("Stars settings points")]
        [HideInInspector] public int pointsRequiredForOneStar;
        [HideInInspector] public int pointsRequiredForTwoStars;
        [HideInInspector] public int pointsRequiredForThreeStars;

        [Header("Stars settings time")]
        [HideInInspector] public int timeRemainingForOneStar;
        [HideInInspector] public int timeRemainingForTwoStars;
        [HideInInspector] public int timeRemainingForThreeStars;
        StarsRequirementData DataStarsRequirementData => new()
        {
            LevelName = SceneManager.GetActiveScene().name,
            Star1_Points = pointsRequiredForOneStar,
            Star2_Points = pointsRequiredForTwoStars,
            Star3_Points = pointsRequiredForThreeStars,
            Star1_TimeRemain = timeRemainingForOneStar,
            Star2_TimeRemain = timeRemainingForTwoStars,
            Star3_TimeRemain = timeRemainingForThreeStars
        };


        void Start()
        {
            StarsSave.Instance.LevelStarted(DataStarsRequirementData);
            //Invoke(nameof(DelayedEvent), 0.1f);
        }
        //void DelayedEvent() => EventManager.Instance.Level_WinConditionType(winConditionType);

    }
}
using __PUBLISH_v1.Scripts;
using game.cards;
using game.managers;
using UnityEngine;

namespace game.player
{
    public class PlayerActionsStats : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static PlayerActionsStats Instance;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);
        }

        //-------------------------------------------------------------

        #endregion

        [Header("Attepms")] [SerializeField] int pairFindAttemps;
        [SerializeField] int pairFindSuccess;
        [SerializeField] int pairFindMistakes;

        RuntimeData RuntimeData => GameManager.Instance.Config.RuntimeData;


        void Start()
        {
            Events.Instance.OnPairSuccess += PairSuccess;
            Events.Instance.OnPairMiss += PairMistake;
        }


        void PairSuccess(Card card1, Card card2)
        {
            pairFindAttemps++;
            pairFindSuccess++;
            RefreshData();
        }

        void PairMistake(Card card1, Card card12)
        {
            pairFindAttemps++;
            pairFindMistakes++;
            RefreshData();
        }

        void RefreshData()
        {
            if (!RuntimeData)
            {
                return;
            }

            RuntimeData.Attempt.attempts = pairFindAttemps;
            RuntimeData.Attempt.success = pairFindSuccess;
            RuntimeData.Attempt.fails = pairFindMistakes;
        }
    }
}
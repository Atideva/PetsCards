using __PUBLISH_v1.Scripts;
using game.player;
using UnityEngine;

namespace game.managers
{
    public class Turns : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static Turns Instance;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);
        }

        //-------------------------------------------------------------

        #endregion

        [Header("My action points")]
        [SerializeField] int count;
        [SerializeField] int limit;
 
        RuntimeData RuntimeData=>GameManager.Instance.Config.RuntimeData;
 
        bool _disabled;
        private bool _gameOver;

        public void Init(int turnLimit)
        {
            limit = turnLimit;
            count = turnLimit;
            Events.Instance.OnLose += GameOver;
        }
 

        public void Refresh() => count = limit;

        public bool CanUseTurn => !_gameOver && !_disabled && count > 0;

        public void UseTurn()
        {
            count--;
            RefreshData();
        }

        public void GameOver() => _gameOver = true;
        public void GameAreNotOver() => _gameOver = false;
        public void Disable() => _disabled = true;
        public void Enable() => _disabled = false;

        void RefreshData()
        {
            if (!RuntimeData)
            {
                return;
            }

            RuntimeData.Action.actions = count;
            RuntimeData.Action.actionsLimit = limit;
        }
    }
}
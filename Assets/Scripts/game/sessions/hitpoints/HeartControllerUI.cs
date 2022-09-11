using game.managers;
using UnityEngine;

namespace game.sessions.hitpoints
{
    [ExecuteInEditMode]
    public class HeartControllerUI : MonoBehaviour
    {
        [Header("Indicators")]
        public HeartUI[] hearts;

        [Header("Previewer")]
        public HeartPreviewUI previewer;
        public LivesUI hpUIs;
        [Header("Hitpoints main")]
        public Lives hpScript;

        void Awake()
        {
            hpScript.OnMaxLiveChange += MaxHpChanged;
        }
        void MaxHpChanged(int maxHp)
        {
            Debug.LogWarning("UI HEARTS max hp limit is 5 for now");
            //only 5 heart images a created for now...
            for (var i = 0; i < hearts.Length; i++)
            {
                var act = i < maxHp;
                hearts[i].gameObject.SetActive(act);
            }
        }

        void Start()
        {
            if (!Application.isPlaying) return;
            Events.Instance.OnHitpointsChanged += HpChanged;
        }

        void HpChanged(int hpCurrent, int hpMax)
        {
            for (var i = 0; i < hearts.Length; i++)
            {
                if (i >= hpCurrent && hearts[i].IsAlive)
                {
                    hearts[i].HeartDead();
                }
                if (i < hpCurrent && !hearts[i].IsAlive)
                {
                    hearts[i].HeartRestored();
                }
            }
        }


#if UNITY_EDITOR
        void Update()
        {
            if (!previewer) previewer = GetComponent<HeartPreviewUI>();
            hearts = previewer.hearts;
        }
#endif
    }
}

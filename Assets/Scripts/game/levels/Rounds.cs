using __PUBLISH_v1.Scripts;
using game.managers;
using game.player;
using game.sessions;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;

namespace game.levels
{
    public class Rounds : MonoBehaviour
    {
        SoundData completeSound => GameManager.Instance.Config.Sound.RoundComplete;
        [SerializeField] int complete;
        [SerializeField] int total;
        RuntimeData RuntimeData => GameManager.Instance.Config.RuntimeData;

        void Start()
        {
            complete = 0;
            Events.Instance.OnRoundWin += RoundComplete_Pair;

            if (Sequence.Instance) Invoke(nameof(DelayedRefresh), 0.1f);
        }

        void DelayedRefresh()
        {
            total = Sequence.Instance.TotalSessions;
            RefreshData();
        }

        void RoundComplete_Pair(int totalPairs)
        {
            complete++;
            RefreshData();
            AudioManager.Instance.PlaySound(completeSound);
        }

        void RefreshData()
        {
            if (!RuntimeData)
            {
                return;
            }

            RuntimeData.Rounds.complete = complete;
            RuntimeData.Rounds.total = total;
        }
    }
}
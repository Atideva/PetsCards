using Firebase.Analytics;
using game.managers;
using game.sessions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Analytics
{
    public class OnRoundComplete : MonoBehaviour
    {
        Parameter _level;
        Parameter _total;
        int _roundID;

        void Start()
        {
            var activeScene = SceneManager.GetActiveScene();
            var sceneName = activeScene.name;
            _level = new Parameter(Params.LEVEL_NAME, sceneName);

            Events.Instance.OnRoundWin += OnWin;
            Invoke(nameof(GetTotalRounds), 0.3f);
        }

        void GetTotalRounds()
        {
            var totalRounds = Sequence.Instance.TotalSessions;
            _total = new Parameter(Params.ROUNDS_TOTAL, totalRounds);
        }

        void OnWin(int roundPairs)
        {
            _roundID++;
            var id = new Parameter(Params.ROUND_ID, _roundID);
            var pairs = new Parameter(Params.ROUND_PAIRS, roundPairs);
            Parameter[] parameters = {_level, id, pairs, _total};

            FirebaseAnalytics.LogEvent(EventName.ROUND_COMPLETE, parameters);
        }
    }
}
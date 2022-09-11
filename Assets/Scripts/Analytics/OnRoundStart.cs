using Firebase.Analytics;
using game.managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Analytics
{
    public class OnRoundStart : MonoBehaviour
    {
        Parameter _level;
        int _roundID;

        void Start()
        {
            var activeScene = SceneManager.GetActiveScene();
            var sceneName = activeScene.name;
            _level = new Parameter(Params.LEVEL_NAME, sceneName);

            Events.Instance.OnSessionPairStart += OnStart;
        }

        void OnStart(int roundPairs)
        {
            _roundID++;
            var id = new Parameter(Params.ROUND_ID, _roundID);
            var pairs = new Parameter(Params.ROUND_PAIRS, roundPairs);
            Parameter[] parameters = {_level, id, pairs};
            FirebaseAnalytics.LogEvent(EventName.ROUND_START, parameters);
        }
        
    }
}
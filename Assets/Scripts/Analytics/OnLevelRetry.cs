using Firebase.Analytics;
using game.managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Analytics
{
    public class OnLevelRetry : MonoBehaviour
    {
        void Start()
            => Events.Instance.OnRetry += OnRetry;

        void OnRetry()
        {
            var activeScene = SceneManager.GetActiveScene();
            var sceneName = activeScene.name;
            var level = new Parameter(Params.LEVEL_NAME, sceneName);

            FirebaseAnalytics.LogEvent(EventName.LEVEL_RETRY, level);
        }
    }
}

using Firebase.Analytics;
using game.managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Analytics
{
    public class OnLevelLose : MonoBehaviour
    {
        void Start()
            => Events.Instance.OnLose += OnLose;

        void OnLose()
        {
            var activeScene = SceneManager.GetActiveScene();
            var sceneName = activeScene.name;
            var level = new Parameter(Params.LEVEL_NAME, sceneName);

            FirebaseAnalytics.LogEvent(EventName.LEVEL_LOSE, level);
        }
    }
}
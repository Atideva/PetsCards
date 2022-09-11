using Firebase.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Analytics
{
    public class OnLevelStart : MonoBehaviour
    {
        void Start()
        {
            var activeScene = SceneManager.GetActiveScene();
            var sceneName = activeScene.name;
            var level = new Parameter(Params.LEVEL_NAME, sceneName);
            FirebaseAnalytics.LogEvent(EventName.LEVEL_START, level);
        }
    }
}
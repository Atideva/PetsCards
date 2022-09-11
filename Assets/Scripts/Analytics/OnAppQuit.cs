using Firebase.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Analytics
{
    public class OnAppQuit : MonoBehaviour
    {
        void OnApplicationQuit()
        {
            var activeScene = SceneManager.GetActiveScene();
            var sceneName = activeScene.name;
            var level = new Parameter(Params.LEVEL_NAME, sceneName);
        
            FirebaseAnalytics.LogEvent(EventName.QUIT, level);
        }
    }
}
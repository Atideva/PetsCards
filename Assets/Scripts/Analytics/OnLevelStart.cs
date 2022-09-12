using Analytics;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Analitics
{
    public class OnLevelStart : MonoBehaviour
    {
        int _sceneIndex;
        string _sceneName;

        void Start()
        {
            var activeScene = SceneManager.GetActiveScene();
            var sceneName = activeScene.name;
            var level = new Parameter(Params.LEVEL_NAME, sceneName);
        
            FirebaseAnalytics.LogEvent(EventName.LEVEL_START, level);
        }

    
    }
}

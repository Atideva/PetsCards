using Firebase.Analytics;
using game.managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Analytics
{
    public class OnLevelWin : MonoBehaviour
    {
    
        void Start() 
            =>Events.Instance.OnWin+=OnWin;
        
        void OnWin()
        {
            var activeScene = SceneManager.GetActiveScene();
            var sceneName = activeScene.name;
            var level = new Parameter(Params.LEVEL_NAME, sceneName);
        
            FirebaseAnalytics.LogEvent(EventName.LEVEL_WIN, level);
        }

     
    }
}

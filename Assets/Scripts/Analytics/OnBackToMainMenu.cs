using Firebase.Analytics;
using game.managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Analytics
{
    public class OnBackToMainMenu : MonoBehaviour
    {

        void Start()
            => Events.Instance.OnBackToMainMenu += OnBack;
        
        void OnBack()
        {
            var activeScene = SceneManager.GetActiveScene();
            var sceneName = activeScene.name;
            var level = new Parameter(Params.LEVEL_NAME, sceneName);
        
            FirebaseAnalytics.LogEvent(EventName.BACK_TO_MAINMENU, level);
        }

  
    }
}

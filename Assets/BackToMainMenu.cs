 
using __PUBLISH_v1.Scripts;
using game.managers;
using UnityEngine;
using UnityEngine.UI;

public class BackToMainMenu : MonoBehaviour
{
    [SerializeField] Button button;
 
    void Start()
    {
        button.onClick.AddListener(Back);
    }

    void Back()
    {
        Events.Instance.BackToMainMenu();
        GameManager.Instance.LoadMainMenu();
    }
   
}

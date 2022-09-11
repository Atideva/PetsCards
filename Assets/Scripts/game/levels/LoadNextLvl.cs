 using game.managers;
using UnityEngine;

public class LoadNextLvl : MonoBehaviour
{

    public void NextLevel()
    {
        Events.Instance.LoadNextLevel();
    }


}

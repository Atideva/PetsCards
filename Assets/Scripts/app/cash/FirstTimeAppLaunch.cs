using app.keys;
using UnityEngine;

namespace app.cash
{
    public class FirstTimeAppLaunch : MonoBehaviour
    {

        void Awake()
        {
            if (!PlayerPrefs.HasKey(ConstantsKeys.AppFirsttimeLaunch))
            {
                Debug.LogWarning("App was launched first time");
                PlayerPrefs.SetInt(ConstantsKeys.AppFirsttimeLaunch, 1);
            }
        }


    }
}

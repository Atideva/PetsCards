#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class AutoSetPassword : MonoBehaviour
{

    void Start()
    {
        PlayerSettings.Android.keystorePass = "w351551537";
        PlayerSettings.Android.keyaliasPass = "w351551537";
    }

}
#endif
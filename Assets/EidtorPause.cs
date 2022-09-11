#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode]
public class EidtorPause : MonoBehaviour
{
    public KeyCode pauseKey;
    public KeyCode scaleKey;
    public float timescale = 0.1f;
    public bool pause;
    public bool scale;

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            pause = !pause;
            EditorApplication.isPaused = pause;
        }
        if (Input.GetKeyDown(scaleKey))
        {
            scale = !scale;
            Time.timeScale = scale ? timescale : 1;
        }
    }
}

#endif
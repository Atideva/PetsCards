using __PUBLISH_v1.Scripts;
using fromWordSearch;
using UnityEngine;

public class AutoLoad : MonoBehaviour
{
    public LevelConfig level;
    public bool autoLoad;

    void Start()
    {
        if (autoLoad)
            GameManager.Instance.LoadLevel(level, 0, false);
    }
}
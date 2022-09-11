using __PUBLISH_v1.Scripts;
using UnityEngine;

public class OnLevelLoad : MonoBehaviour
{
    void Start() => GameManager.Instance.OnLevelLoad();
}
using UnityEngine;

public class ResetLocalPosition : MonoBehaviour
{
    public bool resetToZero = true;

    void Start()
    {
        if (resetToZero)
            transform.localPosition = Vector3.zero;
    }
}
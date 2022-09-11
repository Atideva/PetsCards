using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class test_camSize : MonoBehaviour
{
    public float _x;
    public float _y;

    void Update()
    {
        var _cam = Camera.main;
        _x = _cam.orthographicSize * _cam.aspect;
        _y = _x / _cam.aspect;
    }
}
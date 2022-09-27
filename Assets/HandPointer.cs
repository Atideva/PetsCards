using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

 
public class HandPointer : MonoBehaviour
{
public RectTransform hand;
public Canvas canvas;
public DOTweenAnimation anim;
    void Start()
    {
        
    }

 
    void Update()
    {
        
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition, canvas.worldCamera,
            out movePos);

        hand.position = canvas.transform.TransformPoint(movePos);
 
        if (Input.GetMouseButtonDown(0))
        {
            anim.DORestart();
            anim.DOPlay();
        }
    }
}

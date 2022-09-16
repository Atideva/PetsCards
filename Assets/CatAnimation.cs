using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CatAnimation : MonoBehaviour
{
    public Image eye;
    public Image eye_2;
    public float cd = 1;
    bool enabled;
    float timer;

    public float margaiTime;

    public void Enable()
    {
        timer = cd;
        enabled = true;
    }

    public void Disable()
    {
        enabled = false;
    }

    void FixedUpdate()
    {
        if (!enabled) return;
        timer -= Time.fixedDeltaTime;

        if (timer <= 0)
        {
            timer = cd;
            eye.DOFillAmount(0.85f, margaiTime / 2)
                .OnComplete(() =>
                    eye.DOFillAmount(0, margaiTime / 2));
            eye_2.DOFillAmount(0.85f, margaiTime / 2)
                .OnComplete(() =>
                    eye_2.DOFillAmount(0, margaiTime / 2));
        }
    }
}
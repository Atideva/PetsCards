using System;
using DG.Tweening;
using game.managers;
using UnityEngine;
using UnityEngine.UI;

namespace game.entertainment
{
    public class Vignette : MonoBehaviour
    {
        [SerializeField] private bool useVignette;
        [SerializeField] Image vignette;
        [SerializeField] Canvas canvas;
        [SerializeField] Color dangerColor;
        [SerializeField] float animTime = 0.65f;

        void Start()
        {
            if (useVignette)
            {
                Events.Instance.OnEnableVignette += Enable;
                Events.Instance.OnDisableVignette += Disable;
            }

            Disable();
        }

        void Enable(VignetteType type)
        {
            switch (type)
            {
                case VignetteType.Danger:
                    vignette.DOColor(dangerColor, animTime);
                    break;
            }

            // canvas.enabled = true;
        }

        void Disable()
        {
            vignette.DOColor(Color.clear, animTime);
            // canvas.enabled = false;
        }
    }
}
using System;
using System.Collections;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;

namespace UI
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] SpriteRenderer buttonSprite;
        [SerializeField] SoundData sound;

        [SerializeField]
        Color pressedColor;
        Color _normalColor;
        [SerializeField]
        float colorChangeTime;
        public event Action OnButtonClick = delegate { };

        void Start()
        {
            _normalColor = buttonSprite.color;
        }

        public void Press()
        {
            StartCoroutine(PressRoutine());
        }

        IEnumerator PressRoutine()
        {
            AudioManager.Instance.PlaySound(sound);

            buttonSprite.color = pressedColor;
            yield return new WaitForSeconds(colorChangeTime);
            buttonSprite.color = _normalColor;

            yield return new WaitForSeconds(0.3f);
            OnButtonClick();
        }
    }
}
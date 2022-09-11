using System.Collections;
using DG.Tweening;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace game.cards.abilityVfx
{
    public class LeavesWind : CardAbilityVFX
    {
        [SerializeField] bool autoSize;
        [SerializeField] GameObject leaves;
        [SerializeField] GameObject wind;
        [SerializeField] GameObject blow;

        Camera _cam;

        float x = 0;
        float y = 0;
        float z = -5;
        const float LEAVES_BASE_SIZE = 4;
//
// #if UNITY_EDITOR
//         void Update()
//         {
//             if (!test) return;
//             test = false;
//             Play();
//         }
//
//         bool test;
// #endif


        protected override void OnVfxPlay()
        {
            StartCoroutine(PlayVfx());
        }

        protected override void Reset()
        {
            if (!_cam) _cam = Camera.main;

            // _y = _x / _cam.aspect; это я засунул в Particle System , shape offset
            x = _cam.orthographicSize * _cam.aspect;
            transform.position = new Vector3(-x - 3f, y, z);

            if (autoSize)
            {
                var size = _cam.orthographicSize / LEAVES_BASE_SIZE;
                leaves.transform.DOScale(size, 0);
                wind.transform.DOScale(size, 0);
                blow.transform.DOScale(size, 0);
            }

            leaves.SetActive(false);
            wind.SetActive(false);
            blow.SetActive(false);

            Debug.Log("AbilityVFX reset", this);
        }

        IEnumerator PlayVfx()
        {
            yield return null;
            leaves.SetActive(true);
            wind.SetActive(true);
            blow.SetActive(true);
            yield return new WaitForSeconds(1);
            UseAbility();
            yield return new WaitForSeconds(5);
            Finish();
        }
    }
}
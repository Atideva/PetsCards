using System;
using System.Collections;
using __PUBLISH_v1.Scripts;
using DamageNumbersPro;
using DG.Tweening;
using game.cards;
using game.managers;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;

namespace game.player
{
    public class Combo : MonoBehaviour
    {
        [Header("My combo row")]
        [SerializeField] int successRow;
        [SerializeField] int minimumRow;
        [SerializeField] int failRow;
        [SerializeField] float duration;

        static RuntimeData Runtimedata => GameManager.Instance.Config.RuntimeData;
        [Header("Setup")]
        [SerializeField] bool useComboLabel;
        [SerializeField] DamageNumber comboBig;
        [SerializeField] RectTransform bigPosition;
        SoundData ComboUpSound => GameManager.Instance.Config.Sound.ComboUp;
        [Header("Collect VFX")]
        [SerializeField] bool useVfx;
        [SerializeField] Transform moveToPosition;
        [SerializeField] GameObject pointsVfx;
        [SerializeField] float vfxMoveTime;
        SoundData ComboTrailSound => GameManager.Instance.Config.Sound.ComboTrail;
        public Vector3 MovePosition => _mainCam.ScreenToWorldPoint(moveToPosition.position);

        public int MinimumRow => minimumRow;

        public bool UseComboLabel => useComboLabel;

        public float Duration => duration;

        bool _comboMode;
        DamageNumber _comboBig;
        Camera _mainCam;
        Vector3 _bigPosition;

        bool _isDisable;
        public void Disable() => _isDisable = true;

        void Start()
        {
            successRow = 0;
            Events.Instance.OnPairSuccess += Success;
            Events.Instance.OnPairMiss += Miss;
            Events.Instance.OnWin += OnWin;
            comboBig.PrewarmPool();
            _mainCam = Camera.main;
        }

        void OnWin()
        {
            if (_isDisable) return;
            if (_comboBig) _comboBig.lifetime = 0.01f;
        }

        void Success(Card card1, Card card2)
        {
            if (_isDisable) return;
            successRow++;
            failRow = 0;
            if (successRow >= minimumRow)
            {
                if (useComboLabel)
                {
                    if (!_comboMode)
                    {
                        _comboMode = true;
                        _bigPosition = _mainCam.ScreenToWorldPoint(bigPosition.position);
                        _bigPosition.z = 0;
                        _comboBig = comboBig.CreateNew(0, _bigPosition);
                        _comboBig.lifetime = duration > 0 ? duration : 3600;
                        _comboBig.OnCombination += Combined;
                        _comboBig.OnDead += Dead;
                    }
                }
                else if (!_comboMode)
                    _comboMode = true;
            }

            RefreshData();
            Events.Instance.ComboSuccess(successRow, card1, card2);
        }

        void Dead(DamageNumber dn) => EndCombo();
        void Combined(DamageNumber dn) => PlaySound(ComboUpSound);

        void Miss(Card card1, Card card2)
        {
            if (_isDisable) return;
            if (_comboMode) EndCombo();
            successRow = 0;
            failRow++;
            RefreshData();
        }

        void EndCombo()
        {
            if (successRow > 1)
            {
                if (useComboLabel && useVfx)
                    StartCoroutine(CreateVfx(_bigPosition, true, successRow));
                else
                    Events.Instance.ComboBreak(successRow);
            }

            successRow = 0;
            _comboMode = false;

            if (!_comboBig) return;
            _comboBig.OnCombination -= Combined;
            if (_comboBig.lifetime > 0.1f) _comboBig.lifetime = 0f;
        }

        IEnumerator CreateVfx(Vector3 position, bool sendEventAtEnd, int row)
        {
            PlaySound(ComboTrailSound);
            var vfx = Instantiate(pointsVfx, position, Quaternion.identity);
            yield return vfx.transform.DOMove(MovePosition, vfxMoveTime).WaitForCompletion();
            if (sendEventAtEnd) Events.Instance.ComboBreak(row);
            yield return new WaitForSeconds(1f);
            vfx.SetActive(false);
        }

        void RefreshData()
        {
            if (!Runtimedata) return;
            Runtimedata.Combo.successRow = successRow;
            Runtimedata.Combo.failRow = failRow;
        }

        void PlaySound(SoundData sound) => AudioManager.Instance.PlaySound(sound);
    }
}
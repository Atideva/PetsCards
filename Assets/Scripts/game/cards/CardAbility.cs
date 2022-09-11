using System.Collections;
using System.Collections.Generic;
using game.managers;
using Misc;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;

namespace game.cards
{
    public abstract class CardAbility : PoolObject
    {
        [Header("VFX")]
        public List<CardAbilityVFX> vfxPrefab = new();
        readonly List<CardAbilityVFX> _createdVfx = new();
        protected SoundData UseSound => _ability.Sound;

        protected bool VfxFinish;
        protected Card Card1 { get; private set; }
        protected Card Card2 { get; private set; }
        AbilityConfig _ability;
 

        public void BindToCards(Card bind1, Card bind2)
        {
            Card1 = bind1;
            Card2 = bind2;
            foreach (var vfx in _createdVfx)
            {
                vfx.BindToCards(bind1, bind2);
            }
        }


        public void Init(AbilityConfig config)
        {
            _ability = config;
            _vfxFinish = 0;
            _abilityFinish = false;
            _returnToPool = false;

            VfxFinish = true;
            if (vfxPrefab.Count == 0) return;
            VfxFinish = false;


            if (_createdVfx.Count > 0) return;
            foreach (var vfx in vfxPrefab)
            {
                var fx = Instantiate(vfx, transform)
                    .With(f => f.Init())
                    .With(f => f.OnFinish += OnVfxFinish);

                _createdVfx.Add(fx);
            }

            _createdVfx[0].OnUseAbility += () => VfxFinish = true;
            // OnInit();
        }

        int _vfxFinish;
        bool _abilityFinish;
        bool _returnToPool;

        void OnVfxFinish()
        {
            _vfxFinish++;
            TryReturn();
        }

        public abstract void UseAbility();

        protected void PlayVFX()
        {
            foreach (var vfx in _createdVfx)
                vfx.Play();
        }

        void TryReturn()
        {
            if (_returnToPool && _abilityFinish && _vfxFinish == _createdVfx.Count)
                ReturnToPool();
        }

        protected void Finish(bool autoReturnToPool = true) 
            => StartCoroutine(FinishRoutine(autoReturnToPool));

        protected void Return()
        {
            _returnToPool = true;
            TryReturn();
        }

        IEnumerator FinishRoutine(bool autoReturnToPool)
        {
            if (autoReturnToPool) _returnToPool = true;
            yield return null;
            _abilityFinish = true;
            Events.Instance.FinishAbility(Card1, Card2);
            TryReturn();
        }

        protected void PlaySound(SoundData sound) => AudioManager.Instance.PlaySound(sound);
        protected void EnableTurns() => Turns.Instance.Enable();
        protected void DisableTurns() => Turns.Instance.Disable();
 
    }
}
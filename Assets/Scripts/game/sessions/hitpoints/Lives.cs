using System;
using __PUBLISH_v1.Scripts;
using game.cards;
using game.managers;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;

namespace game.sessions.hitpoints
{
    public class Lives : MonoBehaviour
    {
        enum HealMode
        {
            None,
            OnPairSuccess,
            OnRoundTurn
        }

        enum DamageMode
        {
            OnPairMiss
        }

        public event Action<int> OnMaxLiveChange = delegate { };

        [Header("Settings")]
        [SerializeField] HealMode healMode;
        [SerializeField] DamageMode damageMode;


        SoundData HealSound => GameManager.Instance.Config.Sound.LiveHeal;
        SoundData DamageSound => GameManager.Instance.Config.Sound.LiveDamage;
        
        [Header("Setup")]
        public LivesUI livesUI;
        int _lives, _livesMax;
        int _shield, _shieldMax;
        float _perPair;
        bool _isSubscribed;


        void Start()
        {
            Events.Instance.OnHitpointsEnable += Enable;
            Events.Instance.OnHitpointsDisable += Disable;
            Events.Instance.OnSessionPairStart += OnRoundStart;

            _perPair = GameManager.Instance.Config.Settings.LivesPerPair /
                       GameManager.Instance.CurrentLevel.Difficulty;
        }

        private void OnRoundStart(int totalPairs)
        {
            var add = (int) (totalPairs * _perPair);
            _lives = add;
            if (_lives < 1) _lives = 1;
            Set(_lives);
        }


        void Set(int livesMax) //, int shieldMax)
        {
            _lives = livesMax;
            _livesMax = livesMax;
            OnMaxLiveChange(_livesMax);
            livesUI.Refresh(_lives, _lives);
            AudioManager.Instance.PlaySound(HealSound);
            // if (shieldMax > 0)
            // {
            //     _shield = shieldMax;
            //     _shieldMax = shieldMax;
            //     //EventManager.Instance.Hitpoints_ShieldRestored(shieldMax);
            // }
        }

        void Subscribe(bool subscribe)
        {
            if (subscribe)
            {
                if (_isSubscribed) return;
                _isSubscribed = true;

                Events.Instance.OnHitpointsHeal += Heal;
                Events.Instance.OnHitpointsDamaged += Damage;
            }
            else
            {
                _isSubscribed = false;
                Events.Instance.OnHitpointsHeal -= Heal;
                Events.Instance.OnHitpointsDamaged -= Damage;
            }
        }



        void Enable()
        {
            Subscribe(true);
            Modes();
        }

        void Disable() => Subscribe(false);


        void Heal(int value)
        {
            Change(value);
            AudioManager.Instance.PlaySound(HealSound);
        }

        void Damage(int value)
        {
            // if (_shield > 0)
            // {
            //     _shield--;
            //     Events.Instance.Hitpoints_ShieldChanged(_shield);
            // }
            // else
            // {
            Change(-value);
            AudioManager.Instance.PlaySound(DamageSound);
            //     if (_shieldMax > 0)
            //     {
            //         _shield = _shieldMax;
            //         Events.Instance.Hitpoints_ShieldRestored(_shieldMax);
            //     }
            // }
        }

        void RestoreFull()
        {
            _lives = _livesMax;
            Events.Instance.LivesChange(_lives, _livesMax);
        }

        void Change(int value)
        {
            _lives += value;

            if (_lives <= 0)
            {
                _lives = 0;
                Events.Instance.NoMoreLives();
            }

            if (_lives > _livesMax)
            {
                _lives = _livesMax;
            }

            Events.Instance.LivesChange(_lives, _livesMax);
        }


        void Modes()
        {
            DamageOn();
            HealOn();
        }

        void DamageOn()
        {
            switch (damageMode)
            {
                case DamageMode.OnPairMiss:
                    OnPairMiss();
                    break;
            }
        }

        void HealOn()
        {
            switch (healMode)
            {
                case HealMode.OnPairSuccess:
                    OnPairSuccess();
                    break;
                case HealMode.OnRoundTurn:
                    OnRoundEnd();
                    break;
            }
        }



        void OnPairMiss()
        {
            Events.Instance.OnPairMiss += PairMiss;
        }

        void PairMiss(Card card1, Card card2)
        {
            Damage(1);
        }


        void OnRoundEnd()
        {
            Events.Instance.OnRoundWin += RoundComplete;
        }

        void RoundComplete(int totalPairs)
        {
            RestoreFull();
        }

        void OnPairSuccess()
        {
            Events.Instance.OnPairSuccess += PairSuccess;
        }

        void PairSuccess(Card card1, Card card2)
        {
            Heal(1);
        }
    }
}
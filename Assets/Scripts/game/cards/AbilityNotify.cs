using System.Collections.Generic;
using DG.Tweening;
using game.cards.data;
using game.cards.interfaces;
using game.cards.managers;
using game.entertainment;
using game.managers;
using UnityEngine;

namespace game.cards
{
    public class AbilityNotify : PoolObject
    {
        // Card _card1;
        // Card _card2;
        bool _vigneteActive;
        bool _card1Flip;
        bool _card2Flip;
        bool BothCards => _card1Flip && _card2Flip;
        bool AnyCard => _card1Flip || _card2Flip;

        int _cardsFlipped;
        AbilityConfig _config;

        private CardData _data;
 
        // public void BindToCards(Card bind1, Card bind2)
        // {
        //     _card1 = bind1;
        //     _card2 = bind2;
        //     //   var icard1 = OnTableCards.Instance.GetCard(_myCard1);
        //     ICard icard = bind1;
        //     icard.OnDisabled -= StopNotify;
        //     icard.OnDisabled += StopNotify;
        // }

        public void Init(AbilityConfig config,CardData data)
        {
            _data = data;
            _config = config;
            Events.Instance.OnFlip += OnFlipStart;
            Events.Instance.OnPairSuccess += PairSuccess;
            Events.Instance.OnPairMiss += PairMiss;
        }

        void StopNotify()
        {
            Events.Instance.OnFlip -= OnFlipStart;
            Events.Instance.OnPairSuccess -= PairSuccess;
            Events.Instance.OnPairMiss -= PairMiss;
        }

        void PairMiss(Card card1, Card card2)
        {
            _cardsFlipped -= 2;
            Invoke(nameof(Reset), 0.1f);
        }

        void PairSuccess(Card card1, Card card2)
        {
            _cardsFlipped -= 2;
            Invoke(nameof(Reset), 0.1f);
        }

        void Reset()
        {
            _card1Flip = false;
            _card2Flip = false;
        }


        void OnFlipStart(Card flip, bool isRandomTime, Ease customEase, float customTime)
        {
            _cardsFlipped++;

            var touch = false;

            if (flip.Data == _data)
            {
                if (!_card1Flip)
                {
                    _card1Flip = true; 
                }
                else
                {
                    _card2Flip = true;
                }

                touch = true;
            }
            //
            // if (flip == _card2)
            // {
            //     _card2Flip = true;
            //     touch = true;
            // }

            if (_vigneteActive)
            {
                if (BothCards)
                {
                    DisableVignette();
                }
                else
                {
                    if (!AnyCard) return;
                    DisableVignette();
                    DisableWarning();
                }
            }
            else
            {
                if (touch
                    && _cardsFlipped == 1 
                    && !_vigneteActive)
                {
                    EnableWarning(VignetteType.Danger);
                }
            }
        }

        void EnableWarning(VignetteType type)
        {
            _vigneteActive = true;

            if (_config.Type == AbilityType.Evil)
                Events.Instance.EnableVignette(type);

            Events.Instance.ShowAbilityMessage(_config);
        }

        void DisableVignette()
        {
            _vigneteActive = false;
            Events.Instance.DisableVignette();
        }

        void DisableWarning()
        {
            Events.Instance.HideAbilityMessage();
        }
 
    }
}
using DG.Tweening;
using game.cards.data;
using game.cards.interfaces;
using game.cards.managers;
using game.managers;
using UnityEngine;

namespace game.cards
{
    public class AbilityCardOutline : MonoBehaviour
    {
        Color _color;
        // Card _myCard1;
        // Card _myCard2;

        // bool BothCards => _card1Flip && _card2Flip;
        // bool _card1Flip, _card2Flip;
        CardData _data;



        public void Init(CardData data, Color color)
        {
            _data = data;
            _color = color;

            // _myCard1 = card1;
            // _myCard2 = card2;

            // Events.Instance.OnAbilityFinish += AbilityUsed;
            Events.Instance.OnFlipEnd += OnFlipEnd;
            // Events.Instance.OnFlipBackFinish += OnFlippedBack;

            // var icard1 = OnTableCards.Instance.GetCard(card1);
            // icard1.OnDisabled += Unsubscribe;
        }

        // void Unsubscribe()
        // {
        //     // Events.Instance.OnAbilityFinish -= AbilityUsed;
        //     Events.Instance.OnFlip -= OnFlipEnd;
        //     // Events.Instance.OnFlipBackFinish -= OnFlippedBack;
        // }

        // void AbilityUsed(Card card1, Card card2)
        // {
        //     if (card1 == _myCard1 && card2 == _myCard2 ||
        //         card1 == _myCard2 && card2 == _myCard1)
        //     {
        //         GlowHalfIntensity();
        //     }
        // }

        // void OnFlippedBack(Card card)
        // {
        //     if (card == _myCard1 || card == _myCard2)
        //     {
        //         EnableGlow(card);
        //     }
        // }


        void OnFlipEnd(Card card)
        {
            if (card.Data != _data) return;
            card.SetOutlineColor(_color);
            card.EnableOutline();
            //
            // if (card == _myCard1)
            // {
            //     EnableGlow(card);
            //     _card1Flip = true;
            // }
            //
            // if (card == _myCard2)
            // {
            //     EnableGlow(card);
            //     _card2Flip = true;
            // }

            // if (BothCards)
            // {
            //     Events.Instance.OnFlip -= OnFlip;
            // }
        }


        // public void EnableGlow(Card card)
        // {
        //     // var icard = OnTableCards.Instance.GetCard(card);
        //     if (card is not ICard iCard) return;
        //     iCard.SetOutlineColor(_color);
        //     iCard.EnableOutline();
        //     // var frameColor = _abilityType == AbilityType.Evil ? Color.red : Color.green;
        //     // var sparksColor = _abilityType == AbilityType.Evil ? Color.red : Color.yellow;
        //     //  icard.EnableGlow(_color, _color);
        // }

        void DisableGlow(Card card)
        {
         //   var icard = OnTableCards.Instance.GetCard(card);
        //    if (icard != null)
         //   {
                //   icard.DisableGlow();
                // icard.Outline_Color(Color.red);
                card.DisableOutline();
         //   }
        }

        // void GlowHalfIntensity()
        // {
        //     HalfIntensity(_myCard1);
        //     HalfIntensity(_myCard2);
        // }

        // void HalfIntensity(Card card)
        // {
        //    // var icard = OnTableCards.Instance.GetCard(card);
        //   //  if (icard != null)
        // //    {
        //   //      var sparksColor = _abilityType == AbilityType.Evil ? Color.red : Color.yellow;
        //
        //       //  icard.GlowHalfIntensity(sparksColor);
        //         // icard.Outline_Color(Color.red);
        //         // icard.Outline_Enable();
        // //    }
        // }
    }
}
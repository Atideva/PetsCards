using System.Collections.Generic;
using game.managers;
using UnityEngine;

namespace game.cards.managers
{
    public class CardOrientationManager : MonoBehaviour
    {
        // #region Singleton
        // //-------------------------------------------------------------
        // public static CardOrientationManager Instance;
        // void Awake()
        // {
        //     if (Instance == null) Instance = this;
        //     else
        //     {
        //         Debug.LogError("Did you open it in prefab mode, or there's really 2 instanses of Singleton here?", gameObject);
        //         //gameObject.SetActive(false);
        //     }
        // }
        // //-------------------------------------------------------------
        // #endregion

        Dictionary<Card, CardOrientation> _cardState = new  ();
        List<Card> _onceTouchedCards = new  ();

        void Start()
        {
       //     Events.Instance.OnCardStateChanged += CardStateChanged;
        }

        void CardStateChanged(Card card, CardOrientation cardOrientation)
        {
            if (_cardState.ContainsKey(card))
                _cardState[card] = cardOrientation;
            else
                _cardState.Add(card, cardOrientation);

            if (cardOrientation == CardOrientation.Face)
            {
                if (!_onceTouchedCards.Contains(card)) _onceTouchedCards.Add(card);
            }
        }
        public CardOrientation GetCardState(Card card)
        {
            if (!_cardState.ContainsKey(card))
            {
                _cardState.Add(card, CardOrientation.Back);
                Debug.LogError("Requested card isnt in state dictionary!");
            }
            return _cardState[card];
        }


        public bool IsOnceTouched(Card card) => _onceTouchedCards.Contains(card);


    }
}
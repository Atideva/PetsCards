using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using __PUBLISH_v1.Scripts;
using DG.Tweening;
using game.cards;
using game.cards.create;
using game.cards.layout;
using game.cards.managers;
using game.managers;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;

namespace game.sessions.type
{
    [ExecuteInEditMode]
    public class FindPairSession : MonoBehaviour, ISession
    {
        public bool craftMode;
        [Header("Cards")]
        int _totalCards;
        [SerializeField] int totalPairs;
        public List<SessionDeck> cards = new();
        private SoundData startSound => GameManager.Instance.Config.Sound.CardsLayout;
        Card _firstCard;
        Card _secondCard;
        public int TotalPairs => cards.Sum(item => item.pairs);

        List<ThisPair> _miss = new();
        List<ThisPair> _success = new();
        List<ThisPair> _ability = new();
        readonly Dictionary<Card, ThisPair> _cardsPair = new();
        List<Card> _usedAbilities = new();
        float _flipBackDelay;
        bool _isWinCheckRoutine;

        [Serializable]
        public class ThisPair
        {
            public Card card1;
            public Card card2;
            public bool flip1;
            public bool flip2;
            public bool IsBothFlip => flip1 && flip2;

            public ThisPair(Card c1, Card c2)
            {
                card1 = c1;
                card2 = c2;
                flip1 = c1.Orientation == CardOrientation.Face;
                flip2 = c2.Orientation == CardOrientation.Face;
            }
        }


        public void AddSessionDeck(SessionDeck deck) => cards.Add(deck);

        void Start()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            _flipBackDelay = GameManager.Instance.Config.Settings.PairFlipBackDelay;
            Events.Instance.OnAbilityUse += OnAbilityUse;
            Events.Instance.OnAbilityFinish += OnAbilityFinish;
        }

        void OnAbilityUse(AbilityConfig abiConfig, Card card1, Card card2)
        {
            _usedAbilities.Add(card1);
            _usedAbilities.Add(card2);
        }

        void OnAbilityFinish(Card card1, Card card2)
        {
            if (_usedAbilities.Contains(card1))
            {
                _usedAbilities.Remove(card1);
            }

            if (_usedAbilities.Contains(card2))
            {
                _usedAbilities.Remove(card2);
            }
        }

        void Subscribe(bool subscribe)
        {
            if (subscribe)
            {
                Events.Instance.OnFlip += OnFlip;
                Events.Instance.OnFlipEnd += OnFlipped;
                Events.Instance.OnCardClick += OnClick;
            }
            else
            {
                Events.Instance.OnFlip -= OnFlip;
                Events.Instance.OnFlipEnd -= OnFlipped;
                Events.Instance.OnCardClick -= OnClick;
            }
        }

#if UNITY_EDITOR
        void Update()
        {
            if (_totalCards % 2 != 0) _totalCards++;
            // if (_totalCards == 14)
            // {
            //     cards[0].pairs++;
            //     _totalCards = 16;
            // }

            totalPairs = TotalPairs;
        }
#endif


        public void StartSession()
        {
            SetupTurns();
            Subscribe(true);
            CreateCards();
            LayoutCards();
            Sound(startSound);
            StartEvent(TotalPairs);
        }

        void SessionWin()
        {
            AddWinPoint();
            CompleteEvent();
            Subscribe(false);
        }

        void OnClick(Card card)
        {
            if ( //OnTableCards.Instance.onTable.Contains(card) &&
                card.Orientation == CardOrientation.Back &&
                Turns.Instance.CanUseTurn)
            {
                Turns.Instance.UseTurn();
                Events.Instance.Flip(card);
            }
        }

        void OnFlip(Card card, bool isRandomTime, Ease customEase, float customTime)
        {
            if (!_firstCard)
                _firstCard = card;
            else
            {
                _secondCard = card;
                MatchCheck();
            }
        }

        void OnFlipped(Card card)
        {
            var pair = GetPair(card);
            if (pair == null) return;

            RefreshPair(pair, card);
            if (!pair.IsBothFlip) return;

            if (_ability.Contains(pair))
            {
                NextTurn();
                _ability.Remove(pair);
            }

            if (_miss.Contains(pair))
            {
                FlipBack(pair);
                Events.Instance.PairMiss(pair.card1, pair.card2);
                pair.card1.Fail();
                pair.card2.Fail();
                NextTurn();

                _miss.Remove(pair);
            }

            if (_success.Contains(pair))
            {
                var open = PairsManager.Instance.Open.Count;
                var total = PairsManager.Instance.All.Count;

                if (open >= total)
                {
                    StartCoroutine(WinCheckRoutine());
                }

                _success.Remove(pair);
            }

            _cardsPair.Remove(pair.card1);
            _cardsPair.Remove(pair.card2);
        }


        void MatchCheck()
        {
            var newPair = CreatePair(_firstCard, _secondCard);

            if (_firstCard.Data == _secondCard.Data)
            {
                _success.Add(newPair);
                Match();
                TryEndTurn(newPair);
            }
            else
            {
                _miss.Add(newPair);
            }
        }

        void TryEndTurn(ThisPair newThisPair)
        {
            if (_firstCard.Data.Abilities.Count == 0)
                NextTurn();
            else
                _ability.Add(newThisPair);
        }


        void Match()
        {
            Animate(_firstCard, _secondCard);
            Events.Instance.PairSuccess(_firstCard, _secondCard);
        }


        IEnumerator WinCheckRoutine()
        {
            if (_isWinCheckRoutine) yield break;
            _isWinCheckRoutine = true;

            yield return null; //let card to use ability
            while (_usedAbilities.Count > 0)
            {
                yield return null;
            }

            var open = PairsManager.Instance.Open.Count;
            var total = PairsManager.Instance.All.Count;
            if (open >= total) Complete(total);
            _isWinCheckRoutine = false;
        }


        void Complete(int total)
        {
            Events.Instance.RoundWin(total);
            Invoke(nameof(SessionWin), 1f);
        }


        void NextTurn()
        {
            _firstCard = null;
            _secondCard = null;
            Turns.Instance.Refresh();
        }

        void Animate(Card card1, Card card2)
        {
            card1.Animate(true);
            card2.Animate(false);
        }


        void FlipBack(ThisPair missPair)
        {
            StartCoroutine(FlipBackRoutine(missPair.card1));
            StartCoroutine(FlipBackRoutine(missPair.card2));
            _miss.Remove(missPair);
        }

        ThisPair GetPair(Card card) => !_cardsPair.ContainsKey(card) ? null : _cardsPair[card];

        void RefreshPair(ThisPair thisPair, Card card)
        {
            if (thisPair.card1 == card) thisPair.flip1 = true;
            if (thisPair.card2 == card) thisPair.flip2 = true;
        }

        IEnumerator FlipBackRoutine(Card card)
        {
            yield return new WaitForSeconds(_flipBackDelay);
            Events.Instance.FlipBack(card);
        }

        ThisPair CreatePair(Card card1, Card card2)
        {
            var newPair = new ThisPair(card1, card2);
            _cardsPair.Add(card1, newPair);
            _cardsPair.Add(card2, newPair);
            return newPair;
        }

        void Sound(SoundData sound) => AudioManager.Instance.PlaySound(sound);

        void CreateCards()
        {
            CardCreator.Instance.CreateCards(cards);
        }

        void LayoutCards() => Layout.Instance.LayoutCards(OnTableCards.Instance.onTable);
        void SetupTurns() => Turns.Instance.Init(2);
        void StartEvent(int pairs) => Events.Instance.SessionPairStart(pairs);
        public void Request() => Events.Instance.WinPointRequest();
        void AddWinPoint() => Events.Instance.WinPointEarned();
        void CompleteEvent() => Events.Instance.SessionComplete();
    }
}
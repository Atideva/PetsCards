using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using game.cards.data;
using game.cards.layout;
using game.managers;
using UnityEngine;

namespace game.cards.managers
{
    [ExecuteInEditMode]
    public class OnTableCards : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static OnTableCards Instance;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Debug.LogWarning("Did you open it in prefab mode, or there's really 2 instanses of Singleton here?",
                    gameObject);
            }
        }

        //-------------------------------------------------------------

        #endregion

        public List<Card> onTable = new();
        int _movedOutCards;
        int _totalCardsToMove;

        void Start()
        {
            if (onTable.Count > 0) DestroyEditorDeck();
            if (Application.isPlaying) Events.Instance.OnWin += OnWin;
        }

        void OnWin()
        {
            foreach (var card in onTable)
                card.DisableOutline();
        }

        public void AddCard(DeckData deckType, Card card, CardData cardData)
        {
            if (onTable.Contains(card)) return;

            onTable.Add(card);
            card.Orientation = CardOrientation.Back;
            card.SetCard(deckType, cardData);
        }

        public void ClearDeck()
        {
            onTable = new List<Card>();
            PairsManager.Instance.Clear();
        }

        public void DisableCurrentDeck()
        {
            foreach (var card in onTable) card.Disable();
            StartCoroutine(MoveCards(onTable.ToList()));
            ClearDeck();
        }

        IEnumerator MoveCards(List<Card> moveCards)
        {
            if (moveCards.Count > 0)
                Layout.Instance.waitLastCardsDisappear = true;

            yield return new WaitForSeconds(0.1f);

            _movedOutCards = 0;
            _totalCardsToMove = moveCards.Count;

            foreach (var card in moveCards)
            {
                // const float r = 0.2f;
                // const float moveTime = 0.5f;
                // var rand = Random.Range(1 - r, 1 + r);

                card.transform
                    .DOScale(0, 0.1f)
                    .SetDelay(deSpawnDelay)
                    .OnStart(() => Events.Instance.DeSpawn(card, card.transform.localScale.x))
                    .OnComplete(() => CardMoved(card));
                // card.transform
                //     .DOMove(Layout.Instance.SpawnPosit, moveTime * rand)
                //     .OnComplete(() => CardMovedOut(card));
                // card.transform
                //     .DOScale(Layout.Instance.CardsStartSize, moveTime + r);
            }
        }

        [SerializeField] float deSpawnDelay = 1f;

        void CardMoved(Card card)
        {
            _movedOutCards++;
            card.gameObject.SetActive(false);
            if (_movedOutCards >= _totalCardsToMove)
                Layout.Instance.waitLastCardsDisappear = false;
        }

        public void DestroyEditorDeck()
        {
            foreach (var card in onTable.Where(card => card))
                DestroyImmediate(card);

            ClearDeck();
        }
    }
}
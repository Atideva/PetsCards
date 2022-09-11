using System.Collections.Generic;
using __PUBLISH_v1.Scripts;
using game.cards.data;
using game.cards.managers;
using game.managers;
using game.sessions;
using UnityEngine;

namespace game.cards.create
{
    [ExecuteInEditMode]
    public class CardCreator : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static CardCreator Instance;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Debug.Log("Did you open it in prefab mode, or there's really 2 instanses of Singleton here?",
                    gameObject);
                //gameObject.SetActive(false);
            }
        }

        //-------------------------------------------------------------

        #endregion

        [SerializeField] private AbilityCreator abilityCreator;
        Dictionary<DeckData, List<int>> _cardsPicked = new Dictionary<DeckData, List<int>>();
        GameObject _createdCardContainer;
        string _containerName = "Playing cards";

        void Start()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            _createdCardContainer = GameObject.Find(_containerName);
            if (!_createdCardContainer)
            {
                _createdCardContainer = new GameObject(_containerName);
            }
        }


        public void TestCreate(int totalCards, DeckData fromDeckType)
        {
            //editor mode: CLEAR CARDS CREATED BEFORE
            if (!Application.isPlaying) OnTableCards.Instance.DestroyEditorDeck();
            ResetPLayingDeck();
            ResetPickedCards(fromDeckType);
            Create(totalCards, fromDeckType);
            RenameContainer(totalCards);
        }

        public void CreateCards(List<SessionDeck> sessionDecks)
        {
            //editor mode: CLEAR CARDS CREATED BEFORE
            if (!Application.isPlaying) OnTableCards.Instance.DestroyEditorDeck();

            ResetPLayingDeck();

            var totalCards = 0;
            foreach (var deck in sessionDecks)
            {
                ResetPickedCards(deck.deck);
                if (deck.pairs > 0)
                {
                    var deckCards = deck.pairs * 2;
                    Create(deckCards, deck.deck);
                    totalCards += deckCards;
                }
            }

            RenameContainer(totalCards);
        }

        int cardId;

        void Create(int totalCards, DeckData deckType)
        {
            var totalPairs = totalCards / 2;
            for (var i = 0; i < totalPairs; i++)
            {
                var cardType = GetRandomType(deckType);

                var card1 = CreateObject(cardType, GameManager.Instance.IsCraft ? 1 : 0);
                var card2 = CreateObject(cardType, GameManager.Instance.IsCraft ? 2 : 0);

                OnTableCards.Instance.AddCard(deckType, card1, cardType);
                OnTableCards.Instance.AddCard(deckType, card2, cardType);

                abilityCreator.CreateAbility(cardType);

                Events.Instance.PairCreated(card1, card2, cardType, deckType);
            }
        }

        Card CreateObject(CardData cardData, int craftId = 0)
        {
            var card = Instantiate(cardData.CardPrefab);
            if (GameManager.Instance.IsPvP) card.DoubleArt();
            if (craftId != 0) card.SetMainArt(craftId == 1 ? cardData.FirstCraft : cardData.SecondCraft);
            if (_createdCardContainer) card.transform.SetParent(_createdCardContainer.transform);

#if UNITY_EDITOR
            cardId++;
            var cardName = cardData.name + " : " + cardId;
            card.debugCardName = cardName;
            card.gameObject.name = cardName;
            foreach (var txt in card.debugText)
            {
                txt.text = cardId.ToString();
            }
#endif
            return card;
        }


        CardData GetRandomType(DeckData deck)
        {
            CheckDeck(deck);

            var freeIDs = GetFreeCards(deck);
            var r = Random.Range(0, freeIDs.Count);
            var randomId = freeIDs[r];

            AddToDictionary(deck, randomId);

            return deck.Cards[randomId];
        }

        List<int> GetFreeCards(DeckData deck)
        {
            var cards = GetAvailableCards(deck);
            if (cards.Count != 0)
            {
                return cards;
            }

            ResetPickedCards(deck);
            return GetAvailableCards(deck);
        }

        List<int> GetAvailableCards(DeckData deck)
        {
            var availableCards = new List<int>();
            for (var i = 0; i < deck.Cards.Count; i++)
            {
                if (_cardsPicked[deck].Contains(i)) continue;
                if (GameManager.Instance.Config.unlockAllCards)
                    availableCards.Add(i);
                else if (GameManager.Instance.CardsData.IsUnlock(deck.Cards[i]))
                    availableCards.Add(i);
            }

            return availableCards;
        }

        void CheckDeck(DeckData deck)
        {
            if (!_cardsPicked.ContainsKey(deck)) _cardsPicked.Add(deck, new List<int>());
        }

        void AddToDictionary(DeckData fromDeckType, int randomCardID) =>
            _cardsPicked[fromDeckType].Add(randomCardID);

        void ResetPickedCards(DeckData fromDeckType) => _cardsPicked[fromDeckType] = new List<int>();

        void ResetPLayingDeck() => OnTableCards.Instance.DisableCurrentDeck();

        void RenameContainer(int totalCards) =>
            _createdCardContainer.name = _containerName + " (" + totalCards + ")";
    }
}
using System.Collections.Generic;
using System.Linq;
using game.cards.data;
using game.player;
using TMPro;
using UI.tabs;
using UnityEngine;
using UnityEngine.UI;

namespace UI.collection
{
    public class Collection : MonoBehaviour
    {
        [SerializeField]
        List<DeckData> decks = new List<DeckData>();
        [SerializeField]
        List<Tab> tabs = new List<Tab>();
        List<List<CardData>> _cardLists = new List<List<CardData>>();
        [SerializeField] RuntimeData runtimeData;
        [SerializeField]
        Tab collectionTab;
        [Header("Setup")] [SerializeField]
        Image cardImage;
        [SerializeField]
        TextMeshProUGUI nameLabel;
        [SerializeField]
        TextMeshProUGUI decription;
        [SerializeField]
        TextMeshProUGUI abilityDesc;
        [SerializeField]
        CanvasGroup buttonLeft;
        [SerializeField]
        CanvasGroup buttonRight;
        int _currentListId = 0;
        List<int> _lastCardId = new List<int>();

        public List<CardData> testList1 = new List<CardData>();
        public List<CardData> testList2 = new List<CardData>();

        void Start()
        {
            foreach (var item in tabs)
            {
                item.OnTabClicked += TabClicked;
                _lastCardId.Add(0);
            }

            collectionTab.OnTabEnabled += CollectionEnable;
            InitLists();

            var lastId = 0;
            var list = _cardLists[0];
            var card = list[lastId];
            RefreshBookContent(card);
            RefreshButtons(lastId, list.Count);
        
        }

        void CollectionEnable()
        {
            RefreshLists();
        }

        void InitLists()
        {
            foreach (var deck in decks)
            {
                var ownedList = deck.Cards.Where(card => runtimeData.OwnedCards.Contains(card)).ToList();
                _cardLists.Add(ownedList);
            }

            testList1 = _cardLists[0];
            testList2 = _cardLists[1];
        }

        void RefreshLists()
        {
            for (var i = 0; i < decks.Count; i++)
            {
                var deck = decks[i];
                var ownedList = deck.Cards.Where(card => runtimeData.OwnedCards.Contains(card)).ToList();
                _cardLists[i] = ownedList;
            }
        }

        void TabClicked(Tab tab)
        {
            var id = tabs.IndexOf(tab);
            var list = _cardLists[id];
            _currentListId = id;

            if (list.Count > 0)
            {
                var lastId = _lastCardId[id];
                var card = list[lastId];
                RefreshBookContent(card);
                RefreshButtons(lastId, list.Count);
            }
        }

        void RefreshButtons(int lastId, int listLenght)
        {
            if (lastId >= listLenght - 1)
            {
                DisableButton(buttonRight);
            }
            else
            {
                EnableButton(buttonRight);
            }

            if (lastId <= 0)
            {
                DisableButton(buttonLeft);
            }
            else
            {
                EnableButton(buttonLeft);
            }
        }

        void RefreshBookContent(CardData card)
        {
            cardImage.sprite = card.CardPrefab.MainArt.sprite;
            nameLabel.text = card.name;
            decription.text = card.Description;
            if (card.Abilities.Count > 0)
            {
                var abiName = card.Abilities[0].config.AbilityName;
                var abiDesc = card.Abilities[0].config.Description;
                abilityDesc.text = abiName + ": " + abiDesc;
            }
        }

        public void SwipeRight()
        {
            var lastId = _lastCardId[_currentListId];
            var list = _cardLists[_currentListId];

            if (lastId + 1 < list.Count)
            {
                _lastCardId[_currentListId]++;
                lastId = _lastCardId[_currentListId];

                var card = list[lastId];
                RefreshBookContent(card);
                RefreshButtons(lastId, list.Count);
            }
        }

        public void SwipeLeft()
        {
            var lastId = _lastCardId[_currentListId];
            var list = _cardLists[_currentListId];

            if (lastId > 0)
            {
                _lastCardId[_currentListId]--;
                lastId = _lastCardId[_currentListId];

                var card = list[lastId];
                RefreshBookContent(card);
                RefreshButtons(lastId, list.Count);
            }
        }

        void DisableButton(CanvasGroup buttonCanvasGroup)
        {
            buttonCanvasGroup.alpha = 0;
            buttonCanvasGroup.interactable = false;
        }

        void EnableButton(CanvasGroup buttonCanvasGroup)
        {
            buttonCanvasGroup.alpha = 1;
            buttonCanvasGroup.interactable = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using systems.audio_manager.audio_Event;
using UnityEngine;

namespace game.cards.data
{
    [CreateAssetMenu(fileName = "Deck_", menuName = "Cards/New Deck")]
    public class DeckData : ScriptableObject
    {
        [Header("Type")]
        [SerializeField] DeckType deckType;

        [Header("Style")]
        [SerializeField] Sprite cardFrame;
        [SerializeField] Sprite cardBackImage;

        [Header("Sound")]
        [SerializeField] SoundData rotateSound;

        [Header("Cards")]
        [SerializeField] int uniqueCardsDebug;
        [SerializeField] List<CardData> cards = new  ();


        public DeckType DeckType => deckType;
        public Sprite CardFrame => cardFrame;
        public Sprite CardBackImage => cardBackImage;
        public SoundData RotateSound => rotateSound;
        public ReadOnlyCollection<CardData> Cards => cards.AsReadOnly();

        public int UniqueCardsDebug => uniqueCardsDebug;

        void OnValidate()
        {
            var uniqueList = new List<CardData>();
            foreach (var c in cards.Where(c => !uniqueList.Contains(c)))
            {
                uniqueList.Add(c);
            }
            uniqueCardsDebug = uniqueList.Count;
        }

        public void AddCard(CardData card)
        {
            if (!cards.Contains(card))
                cards.Add(card);
            else
                Debug.LogError("List already contains such card");
        }
    }
}
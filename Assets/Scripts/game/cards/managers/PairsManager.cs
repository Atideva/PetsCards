 using System.Collections.Generic;
using System.Linq;
 using game.cards.ability;
 using game.cards.data;
using game.managers;
using UnityEngine;
 

// ReSharper disable InconsistentNaming

namespace game.cards.managers
{
    public class PairsManager : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static PairsManager Instance;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Debug.LogError("Did you open it in prefab mode, or there's really 2 instanses of Singleton here?",
                    gameObject);
                //gameObject.SetActive(false);
            }
        }

        //-------------------------------------------------------------

        #endregion

        [SerializeField] List<Pair> ALL = new();
        [SerializeField] List<Pair> CLOSE = new();
        [SerializeField] List<Pair> OPEN = new();
        //    [SerializeField] List<Pair> common = new();
        //    [SerializeField] List<Pair> goodAbilities = new();
        //    [SerializeField] List<Pair> evilAbilities = new();


       public bool AnyWolf 
           => CLOSE.SelectMany(pair => pair.cardType.Abilities)
               .Any(data => data.config.Prefab.AbilityPrefab is AbilityClose);


       public IReadOnlyList<Pair> All => ALL;
        public IReadOnlyList<Pair> Close => CLOSE;
        public IReadOnlyList<Pair> Open => OPEN;
 


        void Start()
        {
            Clear();
            // Events.Instance.OnSessionComplete += Clear;
            Events.Instance.OnPairCreate += OnCreate;
            Events.Instance.OnPairSuccess += OnSuccess;
            Events.Instance.OnFlipBackEnd += OnFlipBackEnd;
        }

        void OnFlipBackEnd(Card card)
        {
            Pair pair = null;
            foreach (var p in OPEN)
            {
                if (card == p.card1)
                    pair = p;

                if (card == p.card2)
                    pair = p;
            }

            if (pair == null)
            {
                return;
            }
            
            OPEN.Remove(pair);
            CLOSE.Add(pair);
            
#if UNITY_EDITOR
            pair.card1.gameObject.name = "CLOSE - " + pair.card1.debugCardName;
            pair.card2.gameObject.name = "CLOSE - " + pair.card2.debugCardName;
            var clr = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            foreach (var img in pair.card1.debugImage)
                img.color = clr;
            foreach (var img in pair.card2.debugImage)
                img.color = clr;
#endif

        }

        Pair FindPair(Card card1) => CLOSE.FirstOrDefault(p => p.card1 == card1 || p.card2 == card1);

        void OnSuccess(Card card1, Card card2)
        {
#if UNITY_EDITOR
            card1.gameObject.name = "OPEN - " + card1.debugCardName;
            card2.gameObject.name = "OPEN - " + card2.debugCardName;

#endif
            var pair1 = FindPair(card1);
            var pair2 = FindPair(card2);
            if (pair1 != pair2)
            {

                var pairCards = new List<Card>
                {
                    pair1.card1,
                    pair1.card2,
                    pair2.card1,
                    pair2.card2
                };
                pairCards.Remove(card1);
                pairCards.Remove(card2);

                CLOSE.Remove(pair1);
                CLOSE.Remove(pair2);
                var newClosePair = new Pair(pairCards[0], pairCards[1], pairCards[0].Data);
                CLOSE.Add(newClosePair);
#if UNITY_EDITOR
                var clr = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                foreach (var img in pairCards[0].debugImage)
                    img.color = clr;
                foreach (var img in pairCards[1].debugImage)
                    img.color = clr;
#endif
                var newOpenPair = new Pair(card1, card2, card1.Data);
                OPEN.Add(newOpenPair);
#if UNITY_EDITOR
                  clr = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                foreach (var img in card1.debugImage)
                    img.color = clr;
                foreach (var img in card2.debugImage)
                    img.color = clr;
#endif
                ALL.Remove(pair1);
                ALL.Remove(pair2);
                ALL.Add(newClosePair);
                ALL.Add(newOpenPair);
            }
            else
            {
                if (pair1 == null) return;
                CLOSE.Remove(pair1);
                OPEN.Add(pair1);
            }
        }

        [SerializeField] private List<Color> debugColors = new();

        void OnCreate(Card card1, Card card2, CardData type, DeckData deck)
        {
            var pair = new Pair(card1, card2, type, deck);

            ALL.Add(pair);
            CLOSE.Add(pair);
#if UNITY_EDITOR
            card1.gameObject.name = "CLOSE - " + card1.debugCardName;
            card2.gameObject.name = "CLOSE - " + card2.debugCardName;

            var clr = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            foreach (var img in card1.debugImage)
                img.color = clr;
            foreach (var img in card2.debugImage)
                img.color = clr;
#endif
            // switch (deck.DeckType)
            // {
            //     case DeckType.Common:
            //         common.Add(pair);
            //         break;
            //     case DeckType.GoodAbilities:
            //         goodAbilities.Add(pair);
            //         break;
            //     case DeckType.EvilAbilities:
            //         evilAbilities.Add(pair);
            //         break;
            // }
        }

        public void Clear()
        {
            ALL = new List<Pair>();
            CLOSE = new List<Pair>();
            OPEN = new List<Pair>();
            //           common = new List<Pair>();
            //       goodAbilities = new List<Pair>();
            //      evilAbilities = new List<Pair>();
        }
    }
}
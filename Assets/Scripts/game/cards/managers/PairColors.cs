using System.Collections.Generic;
using System.Linq;
using __PUBLISH_v1.Scripts;
using DG.Tweening;
using game.cards.data;
using game.managers;
using UnityEngine;

namespace game.cards.managers
{
    public class PairColors : MonoBehaviour
    {
        [Header("DEBUG")]
        public List<Color> _colors;
        public Color currentColor;
        public int click;
        public int colorId;
        public ColorMode colorMode;
        Dictionary<CardData, Color> setColors = new();

        public enum ColorMode
        {
            Click,
            Pair,
            Card
        }

        void Start()
        {
            _colors = GameManager.Instance.Config.PairColors.Colors.ToList();
            Events.Instance.OnFlip += OnFlip;
            Events.Instance.OnPairMiss += OnPairMiss;
            Events.Instance.OnPairSuccess += OnPairSuccess;
            if (colorMode is ColorMode.Pair or ColorMode.Card)
            {
                Events.Instance.OnPairCreate += OnPairCreate;
            }
        }

        void OnPairCreate(Card c1, Card c2, CardData cd, DeckData dd)
        {
            Color setColor = Color.white;
            if (colorMode == ColorMode.Pair)
            {
                if (setColors.ContainsKey(cd))
                {
                    currentColor = setColors[cd];
                }
                else
                {
                    currentColor = _colors[colorId];
                    setColors.Add(cd, currentColor);
                    NextColor();
                }

                setColor = currentColor;
            }

            if (colorMode == ColorMode.Card)
            {
                setColor = c1.Data.GlowColor;
            }

            c1.SetGlowColor(setColor, setColor);
            c2.SetGlowColor(setColor, setColor);
            c1.SetSparkOutColor(setColor);
            c2.SetSparkOutColor(setColor);
        }

        void NextColor()
        {
            colorId++;
            if (colorId >= _colors.Count) colorId = 0;
        }

        public float disableGlowDelay = 1;

        void OnPairSuccess(Card c1, Card c2)
        {
            if (disableGlow)
            {
                c1.DisableGlow(disableGlowDelay);
                c2.DisableGlow(disableGlowDelay); 
            }

        }

        public bool disableGlow;

        void OnPairMiss(Card c1, Card c2)
        {
            c1.DisableGlow(disableGlowDelay);
            c2.DisableGlow(disableGlowDelay);
        }

        void OnFlip(Card card, bool isRandomTime, Ease ease, float customTime)
        {
            if (colorMode == ColorMode.Click)
            {
                click++;
                if (click > 2)
                {
                    click = 1;
                    NextColor();
                }
                currentColor = _colors[colorId];
                card.SetGlowColor(currentColor, currentColor);
                card.SetSparkOutColor(currentColor);
            }
            card.EnableGlow();
        }
    }
}
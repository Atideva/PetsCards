using __PUBLISH_v1.Scripts;
using game.cards;
using game.cards.layout;
using game.entertainment;
using game.managers;
using game.player;
using UnityEngine;

namespace game.levels
{
    public class LevelPVPUI : MonoBehaviour
    {
        [SerializeField] Canvas canvas;
        [SerializeField] PlayerPanel player1;
        [SerializeField] PlayerPanel player2;
        [SerializeField] Color passiveColor;
        [SerializeField] Sprite passiveSprite;
        PlayerPanel currentPlayer;
        [SerializeField] PVPwinCondition winCondition;
        public Layout cardsLayout;
        public AbilityMessage abilityMessage;
        public Score score;
        public Combo combo;
        public ScoreUI scoreUI;
        public float switchPlayerDelay = 0.5f;
        public int GetWinnerID()
        {
            return player1.Score == player2.Score
                ? 0
                : player1.Score > player2.Score
                    ? 1
                    : 2;
        }
        public PlayerPanel Player1 => player1;
        public PlayerPanel Player2 => player2;
        void Start()
        {
            if (GameManager.Instance.IsPvP)
            {
                EnableUI();
                Events.Instance.OnPairMiss += OnPairMiss;
                Events.Instance.OnPairSuccess += OnPairSuccess;

                cardsLayout.AutoSizePaddingTop = 0.17f;
                cardsLayout.AutoSizePaddingBottom = 0.17f;
                abilityMessage.Disable();
                score.Disable();
                combo.Disable();
                scoreUI.Disable();
            }
            else
            {
                DisableUI();
            }
        }

        void OnPairSuccess(Card arg1, Card arg2)
        {
            currentPlayer.AddScore();
        }

        void OnPairMiss(Card card, Card card1)
        {
            SwitchPlayer();
        }

        void EnableUI()
        {
            canvas.enabled = true;
            Player1.Init(passiveSprite, passiveColor);
            Player2.Init(passiveSprite, passiveColor);
            currentPlayer = (Random.value > 0.5f) ? Player1 : Player2;
            RefreshUI(currentPlayer);
        }

        void DisableUI()
        {
            canvas.enabled = false;
        }


        void SwitchPlayer()
        {
            currentPlayer = currentPlayer == Player1 ? Player2 : Player1;
            RefreshUI(currentPlayer);

            Turns.Instance.Disable();
            Invoke(nameof(EnableTurns), switchPlayerDelay);
        }

        void EnableTurns()
        {
            Turns.Instance.Enable();
        }
        void RefreshUI(PlayerPanel activePlayer)
        {
            if (Player1 == activePlayer)
                Player1.Active();
            else
                Player1.Passive();

            if (Player2 == activePlayer)
                Player2.Active();
            else
                Player2.Passive();
        }
    }
}

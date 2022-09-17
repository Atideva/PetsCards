using System;
using DG.Tweening;
using game.cards;
using game.cards.data;
using game.cards.interfaces;
using game.entertainment;
using game.sessions.timer;
using UnityEngine;

namespace game.managers
{
    public class Events : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static Events Instance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                // transform.SetParent(null);
                // DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        //-------------------------------------------------------------

        #endregion

        #region WinLose

        //
        //win point required
        public event Action OnWinPointRequest = delegate { };

        public void WinPointRequest() => OnWinPointRequest();

        // 
        //win point get
        public event Action OnWinPointEarn = delegate { };
        public void WinPointEarned() => OnWinPointEarn();

        #endregion

        #region Timer

        //
        //
        // Enable
        public event Action OnTimerEnable = delegate { };

        public void EnableTimer() => OnTimerEnable();

        //
        //
        // Disable
        public event Action OnTimerDisable = delegate { };

        public void DisableTimer() => OnTimerDisable();

        //
        //
        // Start
        public event Action OnTimerContinue = delegate { };

        public void ContinueTimer() => OnTimerContinue();

        //
        //
        // Pause
        public event Action OnTimerPause = delegate { };

        public void PauseTimer() => OnTimerPause();

        //
        //
        // Over
        public event Action OnTimeOver = delegate { };

        public void TimeOver() => OnTimeOver();

        //
        //
        // TimeState
        public event Action<TimerState> OnTimerState = delegate { };

        public void TimerStateChange(TimerState state) => OnTimerState(state);

        public event Action<int> OnSetTimer = delegate { };

        public void SetTimer(int seconds) => OnSetTimer(seconds);
        //
        //
        // second remaining
        public event Action<int> OnTimerUpdate = delegate { };

        public void TimerUpdate(int seconds) => OnTimerUpdate(seconds);

        //
        //
        // +seconds
        public event Action<int> OnTimerAdd = delegate { };

        public void TimerAdd(int seconds) => OnTimerAdd(seconds);

        
        //
        //
        // -seconds
        public event Action<int> OnTimerSubtract = delegate { };
        public void TimerSubtract(int seconds) => OnTimerSubtract(seconds);

        #endregion

        #region Hitpoints

        //
        //
        // Enable
        public event Action OnHitpointsEnable = delegate { };

        public void HitpointsEnable() => OnHitpointsEnable();

        //
        //
        // Disable
        public event Action OnHitpointsDisable = delegate { };

        public void HitpointsDisable() => OnHitpointsDisable();

        //
        //
        // Heal
        public event Action<int> OnHitpointsHeal = delegate { };

        public void Hitpoints_Heal(int value) => OnHitpointsHeal(value);

        //
        //
        // Damage
        public event Action<int> OnHitpointsDamaged = delegate { };

        public void Hitpoints_Damaged(int value) => OnHitpointsDamaged(value);

        //
        //
        // Add maxHP
        public event Action<int> OnHitpointsAddMaxHp = delegate { };

        public void Hitpoints_AddMaxHp(int value) => OnHitpointsAddMaxHp(value);

        //
        //
        // Dead
        public event Action OnNoMoreLives = delegate { };

        public void NoMoreLives() => OnNoMoreLives();

        //
        //
        // Changed
        public event Action<int, int> OnHitpointsChanged = delegate { };

        public void LivesChange(int hitpointsCurrent, int hpMax) => OnHitpointsChanged(hitpointsCurrent, hpMax);

        //
        //
        // Lost 
        public event Action<int> OnHitpointsLost = delegate { };

        public void Hitpoints_Lost(int lostValue) => OnHitpointsLost(lostValue);

        //
        //
        // Restored
        public event Action<int> OnHitpointsRestored = delegate { };

        public void Hitpoints_Restored(int restoreValue) => OnHitpointsRestored(restoreValue);

        //
        //
        // Shield Restored
        public event Action<int> OnHitpointsShieldRestored = delegate { };

        public void Hitpoints_ShieldRestored(int shieldMax) => OnHitpointsShieldRestored(shieldMax);

        //
        //
        // Shield Changed
        public event Action<int> OnHitpointsShieldChanged = delegate { };
        public void Hitpoints_ShieldChanged(int shieldCurrent) => OnHitpointsShieldChanged(shieldCurrent);

        #endregion

        #region Cards actions

        public event Action<Card,Card> OnCombine = delegate { };

        public void  Combine(Card c1, Card c2) => OnCombine(c1,c2);
        
        // Clicked
        public event Action<Card> OnCardClick = delegate { };

        public void CardClick(Card card) => OnCardClick(card);

        // Flip
        public event Action<Card, bool, Ease, float> OnFlip = delegate { };

        public void Flip(Card card)
            => OnFlip(card, false, Ease.Unset, -1);

        public void Flip(Card card, bool isRandomTime)
            => OnFlip(card, isRandomTime, Ease.Unset, -1);

        public void Flip(Card card, bool isRandomTime, Ease easeType) =>
            OnFlip(card, isRandomTime, easeType, -1);

        public void Flip(Card card, bool isRandomTime, Ease easeType, float customTime) =>
            OnFlip(card, isRandomTime, easeType, customTime);

        //
        //
        // Flip Back
        public event Action<Card, bool> OnFlipBack = delegate { };

        public void FlipBack(Card card, bool isRandomTime = false) => OnFlipBack(card, isRandomTime);

        //
        //
        // Flip ENDED
        public event Action<Card> OnFlipEnd = delegate { };

        public void FlipEnd(Card card) => OnFlipEnd(card);

        //
        //
        // FlipBack ENDED
        public event Action<Card> OnFlipBackEnd = delegate { };
        public void FlipBackEnd(Card card) => OnFlipBackEnd(card);

        //
        //
        // Moved on table
        public event Action<Card, Vector3> OnCardPositionChange = delegate { };
        public void CardPositionChange(Card card, Vector3 pos) => OnCardPositionChange(card, pos);
        public event Action<Card, float> OnSpawn = delegate { };
        public void Spawn(Card card, float size) => OnSpawn(card, size);
        public event Action<Card, float> OnDeSpawn = delegate { };
        public void DeSpawn(Card card, float size) => OnDeSpawn(card, size);

        #endregion

        #region Cards

        //
        //
        // Pair created
        public event Action<Card, Card, CardData, DeckData> OnPairCreate = delegate { };

        public void PairCreated(Card card1, Card card2, CardData type, DeckData deck) =>
            OnPairCreate(card1, card2, type, deck);

        //
        //
        // State changed
        // public event Action<Card, CardOrientation> OnCardStateChanged = delegate { };
        //
        // public void CardStateChanged(Card card, CardOrientation cardOrientation) => OnCardStateChanged(card, cardOrientation);

        //
        // Ability warning
        public event Action<AbilityConfig> OnShowAbilityMessage = delegate { };

        public void ShowAbilityMessage(AbilityConfig ability) => OnShowAbilityMessage(ability);

        // Ability warning
        public event Action OnHideAbilityMessage = delegate { };

        public void HideAbilityMessage() => OnHideAbilityMessage();

        // Ability use
        public event Action<AbilityConfig, Card, Card> OnAbilityUse = delegate { };

        public void UseAbility(AbilityConfig config, Card card1, Card card2) =>
            OnAbilityUse(config, card1, card2);

        // Ability use
        public event Action<Card, Card> OnAbilityFinish = delegate { };
        public void FinishAbility(Card card1, Card card2) => OnAbilityFinish(card1, card2);

        #endregion

        #region Player actions

        //
        //
        // FindPair
        public event Action<int, Card, Card> OnComboSuccess = delegate { };

        public void ComboSuccess(int successComboRow, Card card1, Card card2) =>
            OnComboSuccess(successComboRow, card1, card2);

        public event Action<int> OnComboBreak = delegate { };

        public void ComboBreak(int successComboRow) =>
            OnComboBreak(successComboRow);


        // Points earned
        public event Action<int, bool> OnAddScore = delegate { };

        public void AddScore(int earnedPoints, bool isCombo) => OnAddScore(earnedPoints, isCombo);

        //
        //
        // Points total changed
        public event Action<int> OnScoreChange = delegate { };
        public void ScoreChange(int totalPoints) => OnScoreChange(totalPoints);

        #endregion

        #region Player unlocks

        //
        //
        // Unlock card
        public event Action<CardData> OnPlayerUnlockCard = delegate { };
        public void PlayerUnlock_Card(CardData card) => OnPlayerUnlockCard(card);

        #endregion

        #region Sessions

        //
        //
        // Complete
        public event Action OnSessionComplete = delegate { };
        public void SessionComplete() => OnSessionComplete();

        #endregion

        #region Session Pairs

        //
        //
        // Pair Start
        public event Action<int> OnSessionPairStart = delegate { };

        public void SessionPairStart(int totalPairs) => OnSessionPairStart(totalPairs);

        //
        //
        // Pair Win
        public event Action<int> OnRoundWin = delegate { };
        public void RoundWin(int totalPairs) => OnRoundWin(totalPairs);

        public event Action<Card,Card> OnCreateCoins = delegate { };
        public void CreateCoins(Card c1,Card c2) => OnCreateCoins(c1,c2);

        //
        //
        // Pair Finded
        public event Action<Card, Card> OnPairSuccess = delegate { };

        public void PairSuccess(Card card1, Card card2) => OnPairSuccess(card1, card2);


        //
        //
        // Pair Mistake
        public event Action<Card, Card> OnPairMiss = delegate { };
        public void PairMiss(Card card1, Card card2) => OnPairMiss(card1, card2);

        #endregion

        #region Levels

        //
        //
        //  WIN
        public event Action OnWin = delegate { };
        public void Win() => OnWin();
        public event Action OnRetry = delegate { };
        public void Retry() => OnRetry();
        public event Action OnBackToMainMenu = delegate { };
        public void BackToMainMenu() => OnBackToMainMenu();
        //
        //
        //  LOSE
        public event Action OnLose = delegate { };
        public void Lose() => OnLose();

        //LoadNextLevel
        public event Action OnLoadNextLevel = delegate { };
        public void LoadNextLevel() => OnLoadNextLevel();

        #endregion

        #region Entertaiment

        //  Vignette +
        public event Action<VignetteType> OnEnableVignette = delegate { };

        public void EnableVignette(VignetteType type) => OnEnableVignette(type);

        //  Vignette -
        public event Action OnDisableVignette = delegate { };
        public void DisableVignette() => OnDisableVignette();
        public event Action<float> OnAddScoreMultiplier = delegate { };
        public void AddScoreMultiplier(float add) => OnAddScoreMultiplier(add);
        public event Action<float> OnRemoveScoreMultiplier = delegate { };
        public void RemoveScoreMultiplier(float remove) => OnRemoveScoreMultiplier(remove);

        #endregion
    }
}
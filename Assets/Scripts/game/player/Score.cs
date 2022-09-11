using System.Collections;
using System.Collections.Generic;
using __PUBLISH_v1.Scripts;
using DamageNumbersPro;
using DG.Tweening;
using game.cards;
using game.managers;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;

namespace game.player
{
    //TODO: WELCOME TO THE PART OF THE HELL
    public class Score : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static Score Instance;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);
        }

        //-------------------------------------------------------------

        #endregion

        public bool coinForCombineCards = true;
        public bool coinForEachCard;
        [Header("Setup")]
        [SerializeField] Combo comboController;
        [SerializeField] DamageNumber basicPoints;
        [SerializeField] DamageNumber comboPoints;
        [SerializeField] DamageNumber comboPointsNoLabel;
        [SerializeField] Color extraMultiplierColor;
        [SerializeField] Color normalColor;
        [SerializeField] Vector3 offset;
        [SerializeField] Vector3 offsetCombo;
        [SerializeField] float maxPointsSize;
        [Header("Collect VFX")]
        [SerializeField] bool useVfx;
        [SerializeField] Transform moveToPosition;
        [SerializeField] GameObject pointsVfx;
        [SerializeField] float moveTime;
        [SerializeField] [Range(0f, 0.5f)] float moveTimeRange;
        [SerializeField] float onRoundWinDelay = 1;
        [Header("DEBUG")]
        [SerializeField] int totalScore;
        [SerializeField] int scorePerPair;
        [SerializeField] float comboMultiplier;
        [SerializeField] float otherMultiplier;
        [SerializeField] List<DamageNumber> comboNumbers = new();
        [SerializeField] List<float> comboScales = new();
        [SerializeField] List<int> comboCoin = new();
        static SoundData CoinTrailSound => GameManager.Instance.Config.Sound.CoinTrail;
        static RuntimeData RuntimeData => GameManager.Instance.Config.RuntimeData;


        Camera _camera;
        int _comboPointsValue;
        bool _isDisable;
        float _basicSpeed, _basicMinX, _basicMaxX, _basicMinY, _basicMaxY;
        float _comboSpeed, _comboMinX, _comboMaxX, _comboMinY, _comboMaxY;
        readonly List<int> _lastComboRow = new();
        public int TotalScore => totalScore;
      //  Vector3 MovePosition => _camera.ScreenToWorldPoint(moveToPosition.position);
        Vector3 MovePosition =>  moveToPosition.position ;
        float RandomMoveTime => moveTime * Random.Range(1 - moveTimeRange, 1 + moveTimeRange);

        int CalculateScore(int comboRow) =>
            (int) (scorePerPair * otherMultiplier * (1 + comboMultiplier * (comboRow - 1)));


        public void Disable() => _isDisable = true;
        void AddMultiplier(float value) => otherMultiplier += value;

        void RemoveMultiplier(float value) => otherMultiplier -= value;


        void Start()
        {
            otherMultiplier = 1;
            _camera = Camera.main;
            scorePerPair = GameManager.Instance.Config.Settings.CoinPerPair;
            comboMultiplier = GameManager.Instance.Config.Settings.ComboBonusPerRow;
            basicPoints.PrewarmPool();
            if (comboController.UseComboLabel) comboPoints.PrewarmPool();
            else comboPointsNoLabel.PrewarmPool();
            GetStartValues();
            RefreshRuntimeData();

            if (coinForCombineCards)
            {
                Events.Instance.OnCombine += OnCombine;
            }

            Events.Instance.OnRoundWin += OnRoundWin;
            Events.Instance.OnComboSuccess += OnComboSuccess;
            Events.Instance.OnComboBreak += OnComboEnded;


            Events.Instance.OnAddScoreMultiplier += AddMultiplier;
            Events.Instance.OnRemoveScoreMultiplier += RemoveMultiplier;
        }

        void OnCombine(Card c1, Card c2)
        {
            if (_isDisable) return;
            if (_lastComboRow.Count == 0) return;
            var comboRow = _lastComboRow[^1];
            _lastComboRow.Remove(_lastComboRow.Count - 1);
            var earnedPoints = CalculateScore(comboRow);
            var cardScale = c1.transform.localScale.x;
            for (var i = 0; i < earnedPoints; i++)
            {
                var dn = basicPoints.CreateNew(earnedPoints, c1.transform.position + offset * cardScale);
                ScaleBasic(dn, cardScale);
                CreateMoveCoin(earnedPoints, dn, cardScale, RandomMoveTime, dn.lifetime-0.1f);
            }
        }


        void OnRoundWin(int totalPairs)
        {
            if (_isDisable) return;
            Invoke(nameof(MoveComboCoins), onRoundWinDelay); //  MoveComboCoins();
        }


        void OnComboSuccess(int comboRow, Card card1, Card card2)
        {
            if (_isDisable) return;
            _lastComboRow.Add(comboRow);
            if (!coinForEachCard) return;

            var earnedPoints = CalculateScore(comboRow);
            if (earnedPoints % 2 > 0) earnedPoints--;
            var cardScale = card1.transform.localScale.x;

            if (!comboController.UseComboLabel ||
                (comboController.UseComboLabel && comboRow < comboController.MinimumRow))
            {
                //COMMON
                var coinValue = earnedPoints / 2;
                DamageNumber number1 = null;
                DamageNumber number2 = null;
                if (comboRow < comboController.MinimumRow)
                {
                    number1 = basicPoints.CreateNew(coinValue, card1.transform.position + offset * cardScale);
                    number2 = basicPoints.CreateNew(coinValue, card2.transform.position + offset * cardScale);
                    ScaleBasic(number1, cardScale);
                    ScaleBasic(number2, cardScale);

                    if (useVfx)
                    {
                        CreateMoveCoin(coinValue, number1 , cardScale, RandomMoveTime,
                            number1.lifetime);
                        CreateMoveCoin(coinValue, number2 , cardScale, RandomMoveTime,
                            number2.lifetime);
                    }
                    else
                    {
                        totalScore += earnedPoints;
                        EventAdd(earnedPoints);
                        EventChange(totalScore);
                        RefreshRuntimeData();
                    }
                }
                else
                {
                    //COMBO
                    number1 = comboPointsNoLabel.CreateNew(coinValue,
                        card1.transform.position + offsetCombo * cardScale);
                    number2 = comboPointsNoLabel.CreateNew(coinValue,
                        card2.transform.position + offsetCombo * cardScale);


                    number1.enableNumber = coinValue > 1;
                    number2.enableNumber = coinValue > 1;
                    number1.enablePrefix = coinValue > 1;
                    number2.enablePrefix = coinValue > 1;
                    ScaleCombo(number1, cardScale);
                    ScaleCombo(number2, cardScale);
                    StartCoroutine(SetColor(number1, otherMultiplier > 1 ? extraMultiplierColor : normalColor));
                    StartCoroutine(SetColor(number2, otherMultiplier > 1 ? extraMultiplierColor : normalColor));
                    if (keepComboScoreUntilRoundEnd)
                    {
                        number1.lifetime = comboController.Duration;
                        number2.lifetime = comboController.Duration;
                        comboNumbers.Add(number1);
                        comboNumbers.Add(number2);
                    }
                    else
                    {
                        // number1.lifetime = 2;
                        // number2.lifetime = 2;
                        if (useVfx)
                        {
                            CreateMoveCoin(coinValue, number1 , cardScale, RandomMoveTime,
                                number1.lifetime);
                            CreateMoveCoin(coinValue, number2 , cardScale, RandomMoveTime,
                                number2.lifetime);
                        }
                        else
                        {
                            totalScore += earnedPoints;
                            EventAdd(earnedPoints);
                            EventChange(totalScore);
                            RefreshRuntimeData();
                        }
                    }

                    comboScales.Add(cardScale);
                    comboScales.Add(cardScale);
                    comboCoin.Add(coinValue);
                    comboCoin.Add(coinValue);
                }
            }
            else
            {
                var number1 =
                    comboPoints.CreateNew(earnedPoints / 2, card1.transform.position + offsetCombo * cardScale);
                var number2 =
                    comboPoints.CreateNew(earnedPoints / 2, card2.transform.position + offsetCombo * cardScale);

                ScaleCombo(number1, cardScale);
                ScaleCombo(number2, cardScale);

                _comboPointsValue += earnedPoints;
            }
        }

        public bool keepComboScoreUntilRoundEnd;

        IEnumerator SetColor(DamageNumber dn, Color clr)
        {
            yield return null;
            var txtA = dn ? dn.GetTextA() : null;
            var txtB = dn ? dn.GetTextB() : null;
            if (txtA) txtA.color = clr;
            if (txtB) txtB.color = clr;
        }

        void CreateMoveCoin(int score,DamageNumber dn,  float scale, float moveTime, float coinDelay)
        {
            StartCoroutine(CreateMovingCoin(dn, scale, moveTime, coinDelay));
            StartCoroutine(AddPoints(score, moveTime + coinDelay));
        }

        public void OnComboEnded(int successRow)
        {
            if (_isDisable) return;

            if (!comboController.UseComboLabel)
                MoveComboCoins();

            if (comboController.UseComboLabel)
            {
                totalScore += _comboPointsValue;
                EventAdd(_comboPointsValue, true);
                EventChange(totalScore);
                RefreshRuntimeData();
                _comboPointsValue = 0;
            }
        }

        void MoveComboCoins()
        {
            for (var i = 0; i < comboNumbers.Count; i++)
            {
                var number = comboNumbers[i];
                var coin = comboCoin[i];
                var scale = comboScales[i];
                CreateMoveCoin(coin, number, scale, RandomMoveTime, 0);
                number.lifetime = 0.01f;
            }

            comboNumbers = new List<DamageNumber>();
            comboCoin = new List<int>();
            comboScales = new List<float>();
        }


        IEnumerator CreateMovingCoin(DamageNumber dn, float scale, float moveTime, float delay)
        {
            yield return new WaitForSeconds(delay);

            Sound(CoinTrailSound);
            var vfx = Instantiate(pointsVfx, dn.transform.position, Quaternion.identity);
            vfx.transform.localScale = new Vector3(scale, scale, scale);
            yield return
                vfx.transform.DOMove(MovePosition, moveTime).WaitForCompletion();

            yield return new WaitForSeconds(1f);
            vfx.SetActive(false);
        }

        IEnumerator AddPoints(int value, float delay)
        {
            yield return new WaitForSeconds(delay);

            totalScore += value;

            GameManager.Instance.UserResourceSave.AddGold(value);
            EventAdd(value, true);
            EventChange(totalScore);
            RefreshRuntimeData();
        }

        void EventAdd(int earned, bool isCombo = false) => Events.Instance.AddScore(earned, isCombo);
        void EventChange(int total) => Events.Instance.ScoreChange(total);

        void RefreshRuntimeData()
        {
            if (!RuntimeData) return;
            RuntimeData.Score.total = totalScore;
            RuntimeData.Score.scorePerPair = scorePerPair;
            RuntimeData.Score.comboMultiplier = comboMultiplier;
        }

        void ScaleBasic(DamageNumber dn, float cardScale)
        {
            var scale = MaxPointsScale(cardScale);
            dn.transform.localScale = new Vector3(scale, scale, scale);
            dn.lerpSettings.speed = _basicSpeed * cardScale;
            dn.lerpSettings.minX = _basicMinX * cardScale;
            dn.lerpSettings.maxX = _basicMaxX * cardScale;
            dn.lerpSettings.minY = _basicMinY * cardScale;
            dn.lerpSettings.maxY = _basicMaxY * cardScale;
        }

        void ScaleCombo(DamageNumber dn, float cardScale)
        {
            var scale = MaxPointsScale(cardScale);
            dn.transform.localScale = new Vector3(scale, scale, scale);
            dn.lerpSettings.speed = _comboSpeed * cardScale;
            dn.lerpSettings.minX = _comboMinX * cardScale;
            dn.lerpSettings.maxX = _comboMaxX * cardScale;
            dn.lerpSettings.minY = _comboMinY * cardScale;
            dn.lerpSettings.maxY = _comboMaxY * cardScale;
        }

        float MaxPointsScale(float cardScale) =>
            maxPointsSize == 0
                ? cardScale
                : cardScale > maxPointsSize
                    ? maxPointsSize
                    : cardScale;

        void GetStartValues()
        {
            _basicSpeed = basicPoints.lerpSettings.speed;
            _basicMinX = basicPoints.lerpSettings.minX;
            _basicMaxX = basicPoints.lerpSettings.maxX;
            _basicMinY = basicPoints.lerpSettings.minY;
            _basicMaxY = basicPoints.lerpSettings.maxY;

            if (comboController.UseComboLabel)
            {
                _comboSpeed = basicPoints.lerpSettings.speed;
                _comboMinX = basicPoints.lerpSettings.minX;
                _comboMaxX = basicPoints.lerpSettings.maxX;
                _comboMinY = basicPoints.lerpSettings.minY;
                _comboMaxY = basicPoints.lerpSettings.maxY;
            }
            else
            {
                _comboSpeed = comboPointsNoLabel.lerpSettings.speed;
                _comboMinX = comboPointsNoLabel.lerpSettings.minX;
                _comboMaxX = comboPointsNoLabel.lerpSettings.maxX;
                _comboMinY = comboPointsNoLabel.lerpSettings.minY;
                _comboMaxY = comboPointsNoLabel.lerpSettings.maxY;
            }
        }

        void Sound(SoundData sound) => AudioManager.Instance.PlaySound(sound);
    }
}
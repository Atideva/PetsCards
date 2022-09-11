using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using game.managers;
using Misc;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable Unity.InefficientPropertyAccess


namespace game.cards.layout
{
    public class Layout : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static Layout Instance;

        void Awake()
        {
            _camera = Camera.main;

            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);
        }

        //-------------------------------------------------------------

        #endregion

        #region Variables

        [Header("Layout type")]
        [SerializeField] LayoutType layoutType;

        [Header("Settings")]
        [SerializeField] bool turnCardsBackAtStart;
        [SerializeField] float cardRealSizeX = 1.0f;
        [SerializeField] float cardRealSizeY = 1.6f;

        // [Header("Setup")]
        // [SerializeField] Image deckIcon;
        // [SerializeField] Sprite deckActive;
        // [SerializeField] Sprite deckPassive;

        [Header("Autosize")]
        [SerializeField] bool autoSizeCards;
        [SerializeField] float maxCardSizeY;
        [SerializeField] [Range(0f, 0.8f)] float autoSizePaddingTop;
        //    [SerializeField] float autoSizePaddingTopFixedPx;
        [SerializeField] [Range(0f, 0.8f)] float autoSizePaddingBottom;
        [SerializeField] [Range(0f, 0.8f)] float autoSizePaddingLeft;
        [SerializeField] [Range(0f, 0.8f)] float autoSizePaddingRight;
        [Range(0f, 1)] [SerializeField] float autoSizeCardsPaddingX;
        [Range(0f, 1)] [SerializeField] float autoSizeCardsPaddingY;

        [Header("Table center position")]
        [SerializeField] Vector2 tableCenter;

        [Header("Cards padding")]
        [SerializeField] float paddingX = 2;
        [SerializeField] float paddingY = 2;

        [Header("Animation")]
        [SerializeField] AnimationType animType;
        [SerializeField] float spawnInterval = 0.03f;
        [SerializeField] float moveTime = 0.3f;
        [SerializeField] Transform spawnPosition;
        [SerializeField] float cardsStartSize = 1f;
        public Transform topCornerObject;
        [Header("Setup")]
        [SerializeField] TableVertical tableVertical;
        [SerializeField] TableHorizontal tableHorizontal;
        public Vector3 SpawnPosit => _camera.ScreenToWorldPoint(spawnPosition.position);

        public float CardsStartSize => cardsStartSize;

        public float AutoSizePaddingTop
        {
            get => autoSizePaddingTop;
            set => autoSizePaddingTop = value;
        }

        public float AutoSizePaddingBottom
        {
            get => autoSizePaddingBottom;
            set => autoSizePaddingBottom = value;
        }

        [HideInInspector] public bool waitLastCardsDisappear;
        Camera _camera;

        public enum AnimationType
        {
            Instant,
            MoveFromSpawn
        }

        #endregion

        #region DebugVariables

        [Header("DEBUG")]
        public bool debugMode;
        public Transform debugPrefab;
        [Header("Sizes")]
        [SerializeField] float worldWidth;
        [SerializeField] float worldHeight;
        [SerializeField] float layoutWidth;
        [SerializeField] float layoutHeight;
        [SerializeField] float cardSizeX;
        [SerializeField] float cardSizeY;
        [Header("Corners")]
        [SerializeField] float leftCorner;
        [SerializeField] float rightCorner;
        [SerializeField] float topCorner;
        [SerializeField] float bottomCorner;
        [SerializeField] float centerY;
        [SerializeField] float centerX;
        [Header("Padding count")]
        [SerializeField] int rowSpaces;
        [SerializeField] int columnSpaces;

        #endregion

        void Start()
        {
            topCornerObject.gameObject.SetActive(false);
        }

        public void LayoutCards(List<Card> playingCards)
            => TableLayout(playingCards);

        Transform _debugObject;

        void TableLayout(List<Card> playingCards)
        {
            var shuffleList = Extensions.Shuffle(playingCards);
            var table = layoutType switch
            {
                LayoutType.TableVertical => tableVertical.GetLayout(shuffleList.Count),
                LayoutType.TableHorizontal => tableHorizontal.GetLayout(shuffleList.Count),
                _ => null
            };

            if (table == null)
            {
                Debug.LogError("Exception: table layout wasn't setuped for this cards count");
                return;
            }

            if (table.columns == 0)
            {
                Debug.LogError("Table columns = 0, did you set pair count > 0?");
                return;
            }

            var rows = table.rows;
            var columns = table.columns;
            if (autoSizeCards)
            {
                worldWidth = _camera.orthographicSize * _camera.aspect * 2;
                worldHeight = worldWidth / _camera.aspect;

                leftCorner = -worldWidth / 2 + autoSizePaddingLeft * worldWidth / 2;
                rightCorner = worldWidth / 2 - autoSizePaddingRight * worldWidth / 2;
                //   var y = autoSizePaddingTopFixedPx / Screen.height;
                topCorner = topCornerObject.transform.position.y;
                //  topCorner = worldHeight / 2 - (AutoSizePaddingTop + y*(1+_camera.aspect)) * worldHeight;
                // topCorner = 3 -   y  * worldHeight;
                bottomCorner = -worldHeight / 2 + AutoSizePaddingBottom * worldHeight;

                centerX = (leftCorner + rightCorner) / 2;
                centerY = (topCorner + bottomCorner) / 2;
                tableCenter = new Vector2(centerX, centerY);

                layoutWidth = Mathf.Abs(leftCorner - rightCorner);
                layoutHeight = Mathf.Abs(topCorner - bottomCorner);

                rowSpaces = columns - 1;
                columnSpaces = rows - 1;

                cardSizeY = layoutHeight / (rows + columnSpaces * autoSizeCardsPaddingY);
                cardSizeX = cardSizeY / (cardRealSizeY / cardRealSizeX);
                // cardSizeX = tableSizeX / (columns + paddingsInRowCount * autoSizeCardsPaddingX);

                //TODO: its layout for 2x4 cards
                if (table.columns == 2 && table.rows == 4)
                {
                    cardSizeX *= 0.9f;
                    cardSizeY *= 0.9f;
                }

                paddingX = autoSizeCardsPaddingX * cardSizeX;
                paddingY = autoSizeCardsPaddingY * cardSizeY;

                //TODO: its layout for 2x4 cards
                if (table.columns == 2 && table.rows == 4)
                {
                    paddingX *= 0.8f;
                    paddingY *= 0.8f;
                }
                //TODO: !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                if (maxCardSizeY > 0 && cardSizeY > maxCardSizeY)
                {
                    cardSizeY = maxCardSizeY;
                    cardSizeX = cardSizeY / (cardRealSizeY / cardRealSizeX);
                }

#if UNITY_EDITOR
                if (debugMode)
                {
                    if (!_debugObject)
                        _debugObject = Instantiate(debugPrefab);
                    _debugObject.transform.position = tableCenter;
                    _debugObject.transform.localScale = new Vector3(layoutWidth, layoutHeight);
                }
#endif
            }

            if (autoSizeCards)
            {
                var columnsEven = columns % 2 == 0;
                var rowEven = rows % 2 == 0;

                var columnsSteps = columnsEven ? columns / 2 - 1 : columns / 2;
                var rowSteps = rowEven ? rows / 2 - 1 : rows / 2;


                var px0 = columnsEven ? paddingX / 2 + cardSizeX / 2 : 0;
                var py0 = rowEven ? paddingY / 2 + cardSizeY / 2 : 0;
                var x0 = -px0 - columnsSteps * (paddingX + cardSizeX);
                var y0 = py0 + rowSteps * (paddingY + cardSizeY);

                var x = x0;
                var y = y0;

                for (var i = 0; i < rows; i++)
                {
                    for (var j = 0; j < columns; j++)
                    {
                        var cardID = i * columns + j;
                        var newPos = new Vector2(x, y) + tableCenter;
                        var card = shuffleList[cardID];

                        card.transform.position = newPos;
                        Events.Instance.CardPositionChange(card, newPos);

                        x += paddingX + cardSizeX;
                    }

                    y -= (paddingY + cardSizeY);
                    x = x0;
                }

                var mult = cardSizeY / cardRealSizeY;

                // //TODO: its layout for 2x4 cards
                // if (table.columns == 2 && table.rows == 4) mult *= 0.9f;

                foreach (var card in shuffleList)
                {
                    card.transform.localScale = new Vector3(mult, mult, mult);
                }

                //TODO: its layout for 2x4 cards
                if (table.columns == 2 && table.rows == 4)
                {
                    var c = 0;
                    var r = 0;
                    var cardID = c * rows + r;
                    var card = shuffleList[cardID];
                    x = x0 - (paddingX + cardSizeX) * 1;
                    y = y0 - (paddingY + cardSizeY) * 2;
                    var newPos = new Vector2(x, y) + tableCenter;
                    card.transform.position = newPos;

                    c = 0;
                    r = 1;
                    cardID = c * rows + r;
                    card = shuffleList[cardID];
                    x = x0 + (paddingX + cardSizeX) * 2;
                    y = y0 - (paddingY + cardSizeY) * 2;
                    newPos = new Vector2(x, y) + tableCenter;
                    card.transform.position = newPos;

                    foreach (var car in shuffleList)
                    {
                        car.transform.position += new Vector3(0, cardSizeY);
                    }
                }
            }
            else
            {
                var columnsEven = columns % 2 == 0;
                var lineEven = rows % 2 == 0;

                var columnsSteps = columns / 2 - 1;
                var rowSteps = rows / 2 - 1;

                var x0 = columnsEven ? paddingX / 2 : paddingX;
                var y0 = lineEven ? paddingY / 2 : paddingY;
                var x = -x0 - columnsSteps * paddingX;
                var y = y0 + rowSteps * paddingY;

                for (var i = 0; i < columns; i++)
                {
                    for (var j = 0; j < rows; j++)
                    {
                        var cardID = i * rows + j;
                        var newPos = new Vector2(x, y) + tableCenter;
                        var card = shuffleList[cardID];

                        card.transform.position = newPos;
                        Events.Instance.CardPositionChange(card, newPos);

                        y -= paddingY;
                    }

                    x += paddingX;
                    y = y0 + rowSteps * paddingY;
                }
            }


            if (turnCardsBackAtStart) TurnCardsBack(shuffleList);
            if (animType == AnimationType.Instant) StartCoroutine(InstantSpawn(shuffleList));
            if (animType == AnimationType.MoveFromSpawn) StartCoroutine(MoveFromSpawn(shuffleList));
        }


        [SerializeField] float spawnDelay = 0.5f;

        IEnumerator InstantSpawn(List<Card> playingCards)
        {
            var size = playingCards[0].transform.localScale.x;
            foreach (var t in playingCards)
            {
                t.gameObject.SetActive(false);
                t.transform.localScale = Vector3.zero;
            }

            while (waitLastCardsDisappear) yield return null;
            yield return new WaitForSeconds(spawnDelay);

            foreach (var c in playingCards)
            {
                yield return new WaitForSeconds(spawnInterval);
                c.gameObject.SetActive(true);
                c.transform.DOScale(size, 0.3f);
                Events.Instance.Spawn(c, size);
            }
        }

        IEnumerator MoveFromSpawn(List<Card> playingCards)
        {
            foreach (var t in playingCards) t.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.1f);
            while (waitLastCardsDisappear) yield return null;

            var spawnPos = SpawnPosit;
            foreach (var card in playingCards)
            {
                yield return new WaitForSeconds(spawnInterval);

                card.gameObject.SetActive(true);
                var pos = card.transform.position;
                card.transform.position = spawnPos;

                var rand = Random.Range(0.8f, 1.2f);
                card.transform
                    .DOMove(pos, moveTime * rand)
                    .SetEase(Ease.InOutQuad);

                var endSize = card.transform.localScale.x;
                card.transform.localScale = new Vector3(CardsStartSize, CardsStartSize, CardsStartSize);
                card.transform
                    .DOScale(endSize, moveTime * rand).SetEase(Ease.OutQuad)
                    .OnComplete(() => CardMoveEnd(card, endSize));
            }
        }

        void CardMoveEnd(Card card, float size)
        {
            card.Active();
            Events.Instance.Spawn(card, size);
        }

        static void TurnCardsBack(List<Card> playingCards)
        {
            const float yRot = 180;
            foreach (var item in playingCards)
            {
                item.MainArt.flipX = !item.MainArt.flipX;
                item.transform.localEulerAngles = new Vector3
                    (item.transform.localEulerAngles.x, yRot, item.transform.localEulerAngles.z);
            }
        }
    }
}
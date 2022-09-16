using System.Collections;
using System.Collections.Generic;
using System.Linq;
using __PUBLISH_v1.Scripts;
using DG.Tweening;
using game.cards;
using game.cards.ability;
using game.cards.data;
using game.cards.managers;
using game.managers;
using Misc;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UI;
using UnityEngine;
using Sequence = game.sessions.Sequence;

// ReSharper disable InconsistentNaming


public class LevelSlider : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] SliderPoint pointPrefab;
    [SerializeField] CustomSlider slider;
    [SerializeField] Sprite completeSprite;
    [SerializeField] Sprite uncompleteSprite;
    [SerializeField] Sprite activeSprite;

    [Header("Setup")]
    [SerializeField] CanvasGroup sliderGroup;
    [SerializeField] CanvasGroup cardGroup;
    [SerializeField] SliderCardPoint cardPointPrefab;
    [SerializeField] Transform cadPointContainer;
    [SerializeField] CardPointPool cadPointPool;
    public int poolPrewarm = 10;

    [Header("Animation")]
    [SerializeField] Transform animationContainer;
    [SerializeField] bool useMoveAnim;
    [SerializeField] float moveTime;
    [SerializeField] [Range(0, 0.5f)] float moveTimeSpread;
    float RandomMoveTime => Random.Range(1 - moveTimeSpread, 1 + moveTimeSpread) * moveTime;
    [SerializeField] float moveDelay;
    [SerializeField] float animMoveStep;
    [SerializeField] Ease moveEase;

    [Header("DEBUG")]
    [SerializeField] List<SliderPoint> roundList = new();
    [SerializeField] List<SliderCardPoint> cardList = new();

    SoundData CollectSound => GameManager.Instance.Config.Sound.SliderCardCollect;
    SoundData RoundCompleteSound => GameManager.Instance.Config.Sound.SliderRoundComplete;


    [SerializeField]  float cardsShowStep = 0.1f;
    [SerializeField]  int minPairToShow = 1;
    public bool IsCards { get; private set; }
    public bool IsSlider { get; private set; }
    public bool waitSliderCompleteBeforeNextRound;
    int _currentRound;
    int success;
    List<CardData> _cards = new();
    List<int> _pairs = new();
    public SliderCardPoint GetCardPoint(CardData data) => cardList.FirstOrDefault(c => c.Data == data && !c.IsComplete);
    public SliderPoint GetRoundPoint() => roundList[_currentRound];

    void Awake()
    {
        sliderGroup.alpha = 0;
        cardGroup.alpha = 0;
    }


    void Start()
    {
        roundComplete = true;
        cadPointPool.Init(cardPointPrefab, poolPrewarm);
        Events.Instance.OnSessionPairStart += OnNewRound;
        Invoke(nameof(StartSlider), 0.05f);
    }


    void StartSlider()
    {
        var pairs = Sequence.Instance.GetPairSessions().Select(findPair => findPair.TotalPairs).ToList();
        if (pairs.Count <= 1)
        {
            IsSlider = false;
            sliderGroup.gameObject.SetActive(false);
        }
        else
        {
            IsSlider = true;
            sliderGroup.DOFade(1, 0.1f);
            CreateRounds(pairs);
            if (waitSliderCompleteBeforeNextRound) Sequence.Instance.waitSlider = true;
        }
    }

    void Sound(SoundData sound) => AudioManager.Instance.PlaySound(sound);

    public void CardActive(Card c)
    {
        if (IsCards)
        {
            RefreshCard(c.Data);
            Sound(CollectSound);
        }

        if (IsSlider)
        {
            FillSlider(_pairs[_currentRound]);
            Success(_pairs[_currentRound]);
        }
    }


    void Success(int pairs)
    {
        success++;
        if (success < pairs) return;
        success = 0;
        if (useMoveAnim)
        {
            var pos = roundList[_currentRound].transform.position;
            var delay = 0f;
            foreach (var card in cardList)
            {
                delay += animMoveStep;
                card.transform.SetParent(animationContainer);
                card.transform.localScale = Vector3.one;
                var moveTime = RandomMoveTime;
                card.transform.DOMove(pos, moveTime).SetDelay(moveDelay + delay).SetEase(moveEase)
                    .OnComplete(() => Sound(moveCardComplete));
                card.transform.DOScale(Vector3.zero, 0.2f).SetDelay(moveDelay + delay + moveTime);
            }

            Invoke(nameof(RoundComplete), moveDelay + delay + moveTime * (1 + moveTimeSpread));
        }
        else
            RoundComplete();
    }

    public SoundData moveCardComplete;

    void RefreshCard(CardData cd)
    {
        var cardPoint = GetCardPoint(cd);
        if (cardPoint) cardPoint.Complete();
    }

    void FillSlider(int pairs)
    {
        if (_currentRound <= 0) return;
        var pairWidth = (float) 1 / pairs;
        var pointWidth = (float) 1 / (_pairs.Count - 1);
        var sliderAdd = pairWidth * pointWidth;
        slider.AddValue(sliderAdd);
    }

    bool roundComplete;

    void RoundComplete()
    {
        if (_currentRound >= roundList.Count) return;
        Sound(RoundCompleteSound);
        roundList[_currentRound].Complete();
        roundComplete = true;
        if (_currentRound + 1 < roundList.Count) roundList[_currentRound + 1].Active();
        _currentRound++;
        Sequence.Instance.sliderDone = true;
    }

    void OnNewRound(int totalPairs)
        => StartCoroutine(WaitLastRoundComplete());

    IEnumerator WaitLastRoundComplete()
    {
        yield return new WaitUntil(() => roundComplete);
        yield return new WaitForSeconds(cardPointPrefab.PumpTime + 0.2f);
        roundComplete = false;
        RefreshCardList();
        ClearCards();
        if (_cards.Count >= minPairToShow)
        {
            IsCards = true;
            cardGroup.alpha = 1;
        }
        else
        {
            IsCards = false;
            cardGroup.alpha = 0;
        }

        CreateCards(_cards);
    }


    void RefreshCardList()
    {
        _cards = new List<CardData>();
        foreach (var pair in PairsManager.Instance.All) _cards.Add(pair.cardType);
    }

    void ClearCards()
    {
        foreach (var c in cardList) c.gameObject.SetActive(false);
        cardList = new List<SliderCardPoint>();
    }

    void CreateCards(List<CardData> cards)
    {
        float showDelay = 0;
        foreach (var cd in cards)
        {
          //  var isDanger = cd.Abilities.Any(a => a.config.Prefab.AbilityPrefab is AbilityClose);
            var isEvil = cd.Abilities.Any(a => a.config.Type == AbilityType.Evil);
            var isGood = cd.Abilities.Any(a => a.config.Type == AbilityType.Good);
            var delay = showDelay;
            cadPointPool.Get()
                .With(c => c.transform.SetParent(cadPointContainer))
                .With(c => c.Init(cd, isEvil, isGood))
                .With(c => c.Show(delay))
                .With(c => cardList.Add(c))
                .With(c => c.transform.localScale = Vector3.one)
                .With(c => c.transform.localPosition = Vector3.zero);
            showDelay += cardsShowStep;
        }
    }


    void CreateRounds(List<int> pairCounts)
    {
        _pairs = pairCounts;

        slider.SetValue(0);
        var sliderRect = (RectTransform) slider.transform;
        var width = sliderRect.rect.width;
        var stepWidth = width / (_pairs.Count - 1);

        for (var i = 0; i < _pairs.Count; i++)
        {
            var point = Instantiate(pointPrefab, slider.transform);
            point.Init(pairCounts[i], completeSprite, uncompleteSprite, activeSprite);
            var rect = (RectTransform) point.transform;
            rect.anchoredPosition = new Vector3(i * stepWidth, 0);
            point.gameObject.name = "Point: " + pairCounts[i];
            roundList.Add(point);
        }

        _currentRound = 0;
        roundList[_currentRound].Active();
    }
}
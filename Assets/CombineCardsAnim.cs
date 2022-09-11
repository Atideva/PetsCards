using System.Collections;
using System.Linq;
using __PUBLISH_v1.Scripts;
using DG.Tweening;
using game.cards;
using game.cards.ability;
using game.cards.managers;
using game.managers;
using UnityEngine;

// ReSharper disable Unity.InefficientPropertyAccess

public class CombineCardsAnim : MonoBehaviour
{
    public bool useCombineAnim;
    public bool disableGlowAfterCombine;
    public CombineType combineType;
    [Header("Combine")]
    public Ease moveType;
    public float moevTime;
    public float moevDelay;
    public float combineAnimDelay;
    [Header("MoveToSlider")]
    public Ease moveToSliderType;
    public float moveToSLiderDelay = 1;
    public float moveToSliderTime = 1;
    [Header("Setup")]
    public LevelSlider levelSlider;

    public enum CombineType
    {
        ToFirst,
        ToSecond,
        InMiddle
    }

    bool IsCombineAllow(Card c)
        => IsCloseAbility(c) || !PairsManager.Instance.AnyWolf;

    bool IsCloseAbility(Card c)
        => c.Data.Abilities.Any(data => data.config.Prefab.AbilityPrefab is AbilityClose);

    void Start()
    {
        Events.Instance.OnPairSuccess += OnPariSuccess;
    }


    void OnPariSuccess(Card c1, Card c2)
    {
        if (!useCombineAnim) return;

        if (IsCombineAllow(c1))
        {
            var pos = Vector3.zero;
            Card moveCard = null;
            var offset = Vector3.back;
            if (combineType == CombineType.InMiddle)
            {
                var dir = c2.transform.position - c1.transform.position;
                var lenght = dir.magnitude;
                pos = c1.transform.position + dir.normalized * lenght / 2 + offset;
                moveCard = c1;
                c2.transform.DOMove(pos, moevTime).SetDelay(moevDelay).SetEase(moveType);
            }
            else
            {
                switch (combineType)
                {
                    case CombineType.ToFirst:
                        moveCard = c2;
                        pos = c1.transform.position + offset;
                        break;
                    case CombineType.ToSecond:
                        moveCard = c1;
                        pos = c2.transform.position + offset;
                        break;
                }
            }

            if (moveCard)
                moveCard.transform
                    .DOMove(pos, moevTime)
                    .SetDelay(moevDelay)
                    .SetEase(moveType)
                    .OnComplete(() => CombineAnimation(c1, c2, c1.transform.position, c2.transform.position));
        }
        else
        {
            c1.WolfBlock();
            c2.WolfBlock();
        }
    }

    void CombineAnimation(Card c1, Card c2, Vector3 pos1, Vector3 pos2)
    {
        if (disableGlowAfterCombine)
        {
            c1.DisableGlow();
            c2.DisableGlow();
        }

        StartCoroutine(Animate(c1, pos2, combineAnimDelay));
        ResetCard(c2, pos2);
        Events.Instance.Combine(c1, c2);
    }


    IEnumerator Animate(Card c, Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);
        c.CombineAnimation();
        if (GameManager.Instance.IsCraft) c.SetMainArt(c.Data.ResultCraft);

        yield return new WaitForSeconds(moveToSLiderDelay);

        if (levelSlider.IsCards)
        {
            var movePos = levelSlider.IsCards
                ? levelSlider.GetCardPoint(c.Data).transform.position
                : levelSlider.IsSlider
                    ? levelSlider.transform.position
                    : levelSlider.GetRoundPoint().transform.position;
            c.transform.DOMove(movePos, moveToSliderTime).SetEase(moveToSliderType);
            c.transform.DOScale(0.2f, moveToSliderTime / 2).SetDelay(moveToSliderTime / 2);
        }

        yield return new WaitForSeconds(moveToSliderTime);
        levelSlider.CardActive(c);
        ResetCard(c, pos);
    }

    void ResetCard(Card c, Vector3 pos)
    {
        c.transform.position = pos;
        c.ReturnToPool();
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using game.cards;
using game.cards.ability;
using game.cards.managers;
using game.managers;
using UnityEngine;

public class LineDraw : MonoBehaviour
{
    //   LineObject line = null;
    public float width;

    public LinePool linePool;
    public LineObject linePrefab;
    public int poolPrewarm;
    List<LineObject> lines = new();
    [SerializeField] float minDistance;
    [SerializeField] Vector3 offset;
    public bool drawAtSuccess;
    public bool drawAtFail;
    public bool animateAtSuccess;


    void Start()
    {
        linePool.Init(linePrefab, poolPrewarm);
        //    Events.Instance.OnFlipEnd += OnFlipEnd;
        Events.Instance.OnPairSuccess += OnPairSuccess;
        Events.Instance.OnPairMiss += OnPairMiss;
    }

    public float moveTime = 0.5f;
    public float animateLifeTime = 1;

    void OnPairMiss(Card c1, Card c2)
    {
        if (!drawAtFail) return;
        Animate(c1, c2);
    }

    void Animate(Card c1, Card c2)
    {
        var line1 = linePool.Get();
        var line2 = linePool.Get();
        var dir = c2.transform.position - c1.transform.position;
        var lenght = dir.magnitude;
        var middlePos = c1.transform.position + dir.normalized * lenght / 2;
        line1.SetPosition(c1.transform.position + offset, middlePos + offset, moveTime);
        line2.SetPosition(c2.transform.position + offset, middlePos + offset, moveTime);
        line1.SetWidth(width * c1.transform.localScale.x);
        line2.SetWidth(width * c2.transform.localScale.x);
        line1.SetLifeTime(animateLifeTime);
        line2.SetLifeTime(animateLifeTime);
        line1.SetColor(c1.Data.GlowColor);
        line2.SetColor(c2.Data.GlowColor);
    }

    void OnPairSuccess(Card c1, Card c2)
    {
        if (PairsManager.Instance.AnyWolf) return;

        if (animateAtSuccess)
        {
            Animate(c1, c2);
        }

        if (drawAtSuccess)
        {
            StartCoroutine(FollowDelayed(c1, c2, cobineLineDelay));
        }
    }

    public float cobineLineDelay;
    public Vector3 offset2;

    IEnumerator FollowDelayed(Card c1, Card c2, float delay)
    {
        yield return new WaitForSeconds(delay);
        var line = linePool.Get();
        line.StartFollow(c1, c2, minDistance, width * c1.transform.localScale.x, offset2 * c1.transform.localScale.x);
        var line2 = linePool.Get();
        line2.StartFollow(c1, c2, minDistance, width * c1.transform.localScale.x, -offset2 * c1.transform.localScale.x);
    }
}
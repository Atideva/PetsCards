using System;
using System.Collections;
using game.cards;
using UnityEngine;

public class LineObject : PoolObject
{
    public LineRenderer line;
    public Card card1;
    public Card card2;
    float _minDistance;
    bool _follow;

    public void StartFollow(Card c1, Card c2, float minDistance, float width, Vector3 offset)
    {
        _follow = true;
        _offset = offset;
        _minDistance = minDistance;
        card1 = c1;
        card2 = c2;

        line.widthMultiplier = width;
        line.startColor = c1.Data.GlowColor;
        line.endColor = c1.Data.GlowColor;
        line.positionCount = 2;
    }

    public void SetColor(Color color)
    {
        line.startColor = color;
        line.endColor = color;
    }

    public void SetPosition(Vector3 pos1, Vector3 pos2, float moveTime)
    {
        line.positionCount = 2;
        line.SetPosition(0, pos1);
        line.SetPosition(1, pos1);
        StartCoroutine(MoveTo(1, pos1, pos2, moveTime));
    }

    IEnumerator MoveTo(int index, Vector3 from, Vector3 to, float moveTime)
    {
        var dir = (to - from).normalized;
        var speed = 1 / moveTime;
        var pos = from;
        while ((pos - to).magnitude >= 0.1f)
        {
            pos += dir * (speed * Time.deltaTime);
            line.SetPosition(index, pos);
            yield return null;
        }

        line.SetPosition(index, to);
    }

    public void SetWidth(float width) => line.widthMultiplier = width;

    public void SetLifeTime(float lifeTime) => Invoke(nameof(ReturnToPool), lifeTime);
    Vector3 _offset;

    void Update()
    {
        if (!_follow) return;
        line.SetPosition(0, card1.transform.position + _offset);
        line.SetPosition(1, card2.transform.position + _offset);
        var lenght = (card1.transform.position - card2.transform.position).magnitude;
        if (lenght > _minDistance) return;
        _follow = false;
        ReturnToPool();
    }
}
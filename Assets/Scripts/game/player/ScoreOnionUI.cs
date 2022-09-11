 using System;
 using DG.Tweening;
using game.managers;
using TMPro;
using UnityEngine;

public class ScoreOnionUI : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] CanvasGroup group;
    [SerializeField] TextMeshProUGUI txt;
    [Header("Animation")]
    [SerializeField] float fadeTime;
    [SerializeField] float pumpSize;
    [SerializeField] float pumpTime;
    [Header("DEBUG")]
    [SerializeField] float onionMultiplier;

    void Awake() => group.alpha = 0;

    void Start()
    {
        Events.Instance.OnAddScoreMultiplier += AddMultiplier;
        Events.Instance.OnRemoveScoreMultiplier += RemoveMultiplier;
    }

      void AddMultiplier(float value)
    {
        onionMultiplier += value;
        RefreshText();
        if (onionMultiplier > 0) Show();
    }

      void RemoveMultiplier(float value)
    {
        onionMultiplier -= value;
        RefreshText();
        if (onionMultiplier <= 0) Hide();
    }

    void Show()
    {
        group.DOFade(1, fadeTime);
        group.transform
            .DOScale(pumpSize, pumpTime / 2)
            .OnComplete(()
                => group.transform
                    .DOScale(1, pumpTime / 2));
    }

    void RefreshText() => txt.text = "x" + onionMultiplier;

    void Hide()
    {
        group.DOFade(0, fadeTime);
    }
}
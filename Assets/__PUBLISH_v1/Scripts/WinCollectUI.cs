using System;
using DG.Tweening;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;

public class WinCollectUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainGroup;
    [SerializeField] private CanvasGroup buttonGroup;
    [SerializeField] private RewardItemUI coinReward;
    [SerializeField] private RewardItemUI petCoinReward;
    [SerializeField] private RewardItemUI gemReward;

    float _delay;
    float _fadeTime;
    SoundData _sound;
    int _coinValue;
    int _gemValue;

    void Awake()
    {
        Disable();
    }

    public void Init(int coinValue, int petCoins,int gemValue, float fadeTime, float delay, SoundData sound)
    {
        _gemValue = gemValue;
        _coinValue = coinValue;
        _sound = sound;
        _fadeTime = fadeTime;
        _delay = delay;

        if (coinValue > 0) coinReward.Init(coinValue, _fadeTime);
        else coinReward.Disable();

        if (petCoins > 0) petCoinReward.Init(petCoins, _fadeTime);
        else petCoinReward.Disable();
        
        if (gemValue > 0) gemReward.Init(gemValue, _fadeTime);
        else gemReward.Disable();

        buttonGroup.alpha = 0;
        mainGroup.alpha = 0;
    }

    public void Hide()
        => mainGroup.DOFade(0, _fadeTime).OnComplete(Disable);

    void Disable()
    {
        mainGroup.interactable = false;
        mainGroup.blocksRaycasts = false;
    }

    void Enable()
    {
        mainGroup.alpha = 1;
        mainGroup.interactable = true;
        mainGroup.blocksRaycasts = true;
    }

    public void Show(float mainDelay)
    {
        Enable();
        var i = 0;
        if (_coinValue > 0)
        {
            i++;
            coinReward.Animate(mainDelay + _delay * i);
            AudioManager.Instance.PlaySound(_sound, mainDelay + _delay * i);
        }

        if (_gemValue > 0)
        {
            i++;
            gemReward.Animate(mainDelay + _delay * i);
            AudioManager.Instance.PlaySound(_sound, mainDelay + _delay * i);
        }

        if (buttonGroup)
        {
            i++;
            buttonGroup
                .DOFade(1, _fadeTime)
                .SetDelay(mainDelay + _delay * i);
            buttonGroup.transform
                .DOScale(1.5f, _fadeTime / 2)
                .SetDelay(mainDelay + _delay * i)
                .OnComplete(() => 
                    buttonGroup.transform
                        .DOScale(1, _fadeTime / 2));
            AudioManager.Instance.PlaySound(_sound, mainDelay + _delay * i);
        }
    }
}
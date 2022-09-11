using DG.Tweening;
using System.Collections.Generic;
using __PUBLISH_v1.Scripts;
using TMPro;
using UnityEngine;

public class ShowLevelName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelID;
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private float showTime;
    [SerializeField] private float fadeTime = 0.2f;
    [SerializeField] List<TextMeshProUGUI> pvpModeText = new();
    [SerializeField] List<TextMeshProUGUI> pvpLevelText = new();
    [SerializeField] CanvasGroup pvpGroup;
    private void Start()
    {
        if (GameManager.Instance.IsPvP)
        {
            levelID.enabled = false;
            levelName.enabled = false;
            pvpGroup.alpha = 1;
            FadeList(pvpLevelText);
            FadeList(pvpModeText);
        }
        else
        {
            pvpGroup.alpha = 0;
            levelID.text = GameManager.Instance.CurrentLevel.SceneToLoad;
            levelName.text = GameManager.Instance.CurrentLevel.LvlName;
            Fade(levelID);
            Fade(levelName);
        }

        void Fade(TextMeshProUGUI txt) => txt
            .DOFade(0, fadeTime)
            .SetDelay(showTime)
            .OnComplete(() => txt.enabled = false);

        void FadeList(List<TextMeshProUGUI> txts)
        {
            foreach (var txt in txts) Fade(txt);
        }
    }
}
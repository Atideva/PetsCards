using game.managers;
using RengeGames.HealthBars;
using TMPro;
using UI;
using UnityEngine;

namespace game.sessions.hitpoints
{
    public class LivesUI : MonoBehaviour
    {
        [Header("Setup")]
        public GameObject hpCanvas;
        public TextMeshProUGUI txt;
        [Header("Slider")]
        public bool useSlider;
        public CustomSlider slider;
        public bool useBar;
        public UltimateCircularHealthBar circularBar;
        [Header("Knife mode")]
        bool _knifeMode;

        void Awake() => DisableUI();

        void Start()
        {
            Events.Instance.OnHitpointsEnable += EnableUI;
            Events.Instance.OnHitpointsDisable += DisableUI;
            Events.Instance.OnHitpointsChanged += Refresh;
        }

        public void Refresh(int hpCurrent, int hpMax)
        {
            if (useSlider)
            {
                slider.SetValue((float) hpCurrent / hpMax);
            }

            if (useBar)
            {
                circularBar.SetSegmentCount(hpMax);
                circularBar.SetRemovedSegments(hpMax - hpCurrent);
            }

            RefreshText(hpCurrent);
        }


        void EnableUI() => hpCanvas.SetActive(true);
        void DisableUI() => hpCanvas.SetActive(false);
        void RefreshText(int hpCurrent) => txt.text = hpCurrent.ToString();
    }
}
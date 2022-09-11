using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.tabs
{
    [ExecuteInEditMode]
    public class Tab : MonoBehaviour
    {
        [SerializeField] private Button button;
        public event Action OnTabEnabled = delegate { };
        public event Action OnTabDisabled = delegate { };
        public event Action<Tab> OnTabClicked = delegate { };

        void Awake() => button.onClick.AddListener(Clicked);

        public void Clicked() => OnTabClicked(this);

        public void Enable() => OnTabEnabled();

        public void Disable() => OnTabDisabled();
    }
}
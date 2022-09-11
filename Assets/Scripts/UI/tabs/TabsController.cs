using System.Collections.Generic;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;

namespace UI.tabs
{
    public class TabsController : MonoBehaviour
    {
        [Header("Enable")]
        [SerializeField] bool enableOnStart;
        [Header("Sound")]
        [SerializeField] SoundData clickSound;
        [Header("Setup")]
        [SerializeField] Tab parentTab;
        [SerializeField] Tab firstOpenedTab;
        [SerializeField] List<Tab> childTabs = new ();
        Tab _lastOpenedTab;

        void Start()
        {
            SubscribeParent();
            SubscribeChild();
            OpenStartTab();
        }
 
        void SubscribeParent()
        {
            if (!parentTab) return;
            parentTab.OnTabEnabled += Enable;
            parentTab.OnTabDisabled += Disable;
        }

        void SubscribeChild()
        {
            foreach (var item in childTabs)
            {
                item.OnTabClicked += TabClicked;
            }
        }

        void Enable()
        {
            OpenStartTab();
        }

        void Disable()
        {
            CloseLastTab();
        }


        void TabClicked(Tab tab)
        {
            PlaySound(clickSound);
            RefreshTabs(tab);
        }

        void RefreshTabs(Tab tab)
        {
            foreach (var item in childTabs)
            {
                if (tab == item)
                {
                    _lastOpenedTab = item;
                    item.Enable();
                }
                else
                {
                    item.Disable();
                }
            }
        }

        void OpenStartTab()
        {
            if (firstOpenedTab)
            {
                RefreshTabs(firstOpenedTab);
            }
            else
            {
                if (childTabs[0])
                {
                    RefreshTabs(childTabs[0]);
                }
                else
                {
                    Debug.LogError("myTabs.Count = 0");
                }
            }
        }

        void CloseLastTab()
        {
            if (_lastOpenedTab)
            {
                _lastOpenedTab.Disable();
            }
        }
        
        void PlaySound(SoundData sound) => AudioManager.Instance.PlaySound(sound);
    }
}
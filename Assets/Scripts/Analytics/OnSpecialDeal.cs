using UnityEngine;

namespace Analytics
{
    public class OnSpecialDeal : MonoBehaviour
    {
        // public Shop shop;
        // public MenuLevelsUI levels;
        // bool _afterSpecial;
        //
        // void Start()
        // {
        //     shop.OnSpecialOpen += Open;
        //     shop.OnSpecialCancel += Cancel;
        // }
        //
        // void Open()
        // {
        //     FirebaseAnalytics.LogEvent(Events.SPECIAL_OPEN);
        //
        //     _afterSpecial = true;
        //     if (levels) levels.OnClick += ClickAfterSpecial;
        // }
        //
        // void Cancel()
        // {
        //     FirebaseAnalytics.LogEvent(Events.SPECIAL_CANCEL);
        // }
        //
        // void ClickAfterSpecial(WordFinderLevel lvl)
        // {
        //     var lvlItem = new Parameter(Params.LEVEL_ITEM, lvl.lvlName);
        //     FirebaseAnalytics.LogEvent(Events.LEVEL_CLICK_AFTER_SPECIAL, lvlItem);
        // }
        //
        // void OnApplicationQuit()
        // {
        //     if (_afterSpecial)
        //         FirebaseAnalytics.LogEvent(Events.QUIT_AFTER_SPECIAL);
        // }
    }
}
using UnityEngine;

namespace Analytics
{
    public class OnShopItem : MonoBehaviour
    {
        // public Shop shop;
        // Parameter _level;
        //
        // void Start()
        // {
        //     var activeScene = SceneManager.GetActiveScene();
        //     var sceneName = activeScene.name;
        //     _level = new Parameter(Params.LEVEL_NAME, sceneName);
        //
        //     shop.OnClick += OnClick;
        //     shop.OnFail += OnFail;
        //     shop.OnPurchase += OnPurchase;
        // }
        //
        // void OnPurchase(ShopItemConfig shopItem)
        // {
        //     var item = new Parameter(Params.SHOP_ITEM, shopItem.ItemName);
        //     Parameter[] parameters = {_level, item}; 
        //     FirebaseAnalytics.LogEvent(Events.SHOP_ITEM_PURCHASE, parameters);
        // }
        //
        // void OnFail(ShopItemConfig shopItem)
        // {
        //     var item = new Parameter(Params.SHOP_ITEM, shopItem.ItemName);
        //     Parameter[] parameters = {_level, item};
        //     FirebaseAnalytics.LogEvent(Events.SHOP_ITEM_FAIL, parameters);
        // }
        //
        // void OnClick(ShopItemConfig shopItem)
        // {
        //     var item = new Parameter(Params.SHOP_ITEM, shopItem.ItemName);
        //     Parameter[] parameters = {_level, item};
        //     FirebaseAnalytics.LogEvent(Events.SHOP_ITEM_CLICK, parameters);
        // }
    }
}
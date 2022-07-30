using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameShopManager : MonoBehaviour
{
    GameObject currentEnterShop;
    bool shopEnter;

    public void ShowShop<T>(string path) where T : Multi_UI_Popup
    {
        currentEnterShop = Multi_Managers.UI.ShowPopupUI<T>(path).gameObject;
        shopEnter = true;
    }

    public void CloseShop()
    {
        currentEnterShop = null;
        shopEnter = false;
    }
}

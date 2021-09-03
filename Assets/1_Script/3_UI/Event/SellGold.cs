using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellGold : MonoBehaviour, ISellEventShopItem
{
    public int addGold;

    public void Sell_Item()
    {
        GameManager.instance.Gold += addGold;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }
}

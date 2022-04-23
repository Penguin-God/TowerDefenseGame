using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellFood : MonoBehaviour, ISellEventShopItem
{
    public int addFood;

    public void Sell_Item()
    {
        GameManager.instance.Food += addFood;
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuyEventType
{
    CurrentUnitDie,
}

public class SellEvent : MonoBehaviour, ISellEventShopItem
{
    public BuyEventType buyEventType;

    delegate void BuyEvent();
    BuyEvent buyEvent = null;

    private void Awake()
    {
        SetBuyEvent();
    }

    void SetBuyEvent()
    {
        switch (buyEventType)
        {
            case BuyEventType.CurrentUnitDie:
                buyEvent = EventManager.instance.CurrentEnemyDie;
                break;
        }
    }

    public void Sell_Item()
    {
        if (buyEvent != null) buyEvent();
    }
}

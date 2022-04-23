using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyMaxUnitExpend : MonoBehaviour, ISellEventShopItem
{
    public int expendUnitCount;

    public void Sell_Item()
    {
        UnitManager.instance.ExpendMaxUnit(expendUnitCount);
    }
}

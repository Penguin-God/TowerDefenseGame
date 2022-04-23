using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellUnit : MonoBehaviour, ISellEventShopItem
{
    [SerializeField] UnitColor sellUnitColor;
    [SerializeField] UnitClass sellUnitClass;

    public void Sell_Item()
    {
        string _tag = UnitDataBase.GetUnitTag(sellUnitColor, sellUnitClass);
        CombineSoldierPooling.GetObject(_tag, (int)sellUnitColor, (int)sellUnitClass);
    }
}

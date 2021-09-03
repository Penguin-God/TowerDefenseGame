using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellUnit : MonoBehaviour, ISellEventShopItem
{
    [SerializeField] int unitClassNumber;
    [SerializeField] int unitColorNumber;

    public void Sell_Item()
    {
        FindObjectOfType<CreateDefenser>().CreateSoldier(unitColorNumber, unitClassNumber);
    }

}

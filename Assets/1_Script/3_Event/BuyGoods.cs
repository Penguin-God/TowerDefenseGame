using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyGoods : MonoBehaviour
{
    public int unitColorNumber;
    public int unitClassNumber;

    public int buyFoodCount;
    public int price;

    public int buyGoldAmount;

    private Shop shop;
    CreateDefenser createDefenser;
    private void Awake()
    {
        //shop = transform.parent.transform.parent.gameObject.GetComponentInParent<Shop>();
        shop = GetComponentInParent<Shop>();
        createDefenser = shop.createDefenser;
    }

    public void BuyUnitGoods(int price)
    {
        if (GameManager.instance.Gold < price) return;
        
        MinusGold(price);
        if (createDefenser != null) createDefenser.CreateSoldier(unitColorNumber, unitClassNumber);
        shop.ExitShop();
    }

    void MinusGold(int subtractGold)
    {
        GameManager.instance.Gold -= subtractGold;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }
}

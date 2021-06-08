using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyGoods : MonoBehaviour
{
    public int unitColorNumber;
    public int unitClassNumber;

    private Shop shop;
    CreateDefenser createDefenser;
    private void Awake()
    {
        //shop = transform.parent.transform.parent.gameObject.GetComponentInParent<Shop>();
        shop = GetComponentInParent<Shop>();
        createDefenser = shop.createDefenser;
    }

    private void Start()
    {
        createDefenser = FindObjectOfType<CreateDefenser>();
        Debug.Log(createDefenser);
    }

    public void BuyUnitGoods(int price)
    {
        if (GameManager.instance.Gold < price) return;
        MinusGold(price);

        //createDefenser.CreateSoldier(unitColorNumber, unitClassNumber);
        if (createDefenser != null) createDefenser.CreateSoldier(unitColorNumber, unitClassNumber);
    }

    void MinusGold(int subtractGold)
    {
        GameManager.instance.Gold -= subtractGold;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }
}

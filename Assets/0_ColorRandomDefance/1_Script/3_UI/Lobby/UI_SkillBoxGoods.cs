using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillBoxGoods : UI_Base
{
    enum Texts
    {

    }

    protected override void Init()
    {
        GetComponent<Button>().onClick.AddListener(Buy);
    }

    PurchaseManager _purchaseManager;
    MoneyData _price;
    public void DependencyInject(PurchaseManager purchaseManager, MoneyData price)
    {
        _purchaseManager = purchaseManager;
        _price = price;
    }

    void Buy()
    {
        _purchaseManager.Purchase(_price);
    }
}

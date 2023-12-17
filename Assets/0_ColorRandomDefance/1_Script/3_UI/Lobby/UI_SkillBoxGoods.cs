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
    public void DependencyInject(PurchaseManager purchaseManager)
    {
        _purchaseManager = purchaseManager;
    }

    void Buy()
    {
        _purchaseManager.Purchase(PlayerMoneyType.Gem, 1000);
    }
}

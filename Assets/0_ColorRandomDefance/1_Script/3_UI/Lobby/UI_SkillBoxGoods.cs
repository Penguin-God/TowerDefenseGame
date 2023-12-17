using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillBoxGoods : UI_Base
{
    enum Texts
    {
        ProductText,
        PriceText,
    }

    protected override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        GetComponent<Button>().onClick.AddListener(Buy);
    }

    PurchaseManager _purchaseManager;
    MoneyData _price;
    public void DependencyInject(PurchaseManager purchaseManager, MoneyData price, string productName)
    {
        CheckInit();
        _purchaseManager = purchaseManager;
        _price = price;

        GetTextMeshPro((int)Texts.ProductText).text = $"��ǰ : {productName}";
        GetTextMeshPro((int)Texts.PriceText).text = $"���� : {new MoneyPersenter().GetMoneyText(_price)}";
    }

    void Buy()
    {
        _purchaseManager.Purchase(_price);
    }
}

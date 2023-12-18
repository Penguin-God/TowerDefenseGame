using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopProduct : UI_Base
{
    enum Texts
    {
        ProductText,
        PriceText,
    }

    protected override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        GetComponent<Button>().onClick.AddListener(ComfirmBuy);
    }

    PurchaseManager _purchaseManager;
    MoneyData _price;
    string _productName;
    readonly MoneyPersenter _moneyPersenter = new();
    public void DependencyInject(PurchaseManager purchaseManager, MoneyData price, string productName)
    {
        CheckInit();
        _purchaseManager = purchaseManager;
        _price = price;
        _productName = productName;

        GetTextMeshPro((int)Texts.ProductText).text = $"상품 : {productName}";
        GetTextMeshPro((int)Texts.PriceText).text = $"가격 : {_moneyPersenter.GetMoneyText(_price)}";
    }

    void ComfirmBuy() => Managers.UI.ShowPopupUI<UI_ComfirmPopup>().SetInfo($"{_moneyPersenter.GetMoneyTextWithSuffix(_price)} 지불하여 {_productName}을 구매하시겠습니까?", TryBuy);

    void TryBuy()
    {
        Managers.UI.ClosePopupUI();
        if (_purchaseManager.Purchase(_price) == false)
            Managers.UI.ShowPopupUI<UI_NotifyWindow>().SetMessage($"{_moneyPersenter.TypeTextWithSuffix(_price.MoneyType)} 부족합니다");
    }
}

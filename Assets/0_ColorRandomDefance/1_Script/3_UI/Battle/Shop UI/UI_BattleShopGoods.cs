using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public struct ShopGoodsData
{
    public CurrencyData GoodsPriceData;
    public string GoodsName;
    
}

public class UI_BattleShopGoods : UI_Base
{
    enum Texts
    {
        ProductNameText,
        PriceText,
    }

    enum Images
    {
        CurrencyImage,
    }

    enum Buttons
    {
        PanelButton,
    }

    protected override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
    }

    internal void DisplayGoods(BattleShopGoodsData shopGoodsData)
    {
        GetText((int)Texts.ProductNameText).text = shopGoodsData.Name;
        GetText((int)Texts.PriceText).text = shopGoodsData.PriceData.Amount.ToString();
        // GetImage((int)Images.CurrencyImage).sprite = CurrencyToSprite(priceData.CurrencyType);

        GetButton((int)Buttons.PanelButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.PanelButton).onClick.AddListener(() => ShowBuyWindow(new BuyActionFactory().CreateBuyAction()));
    }

    void ShowBuyWindow(UnityAction action)
    {
        string qustionText = "";
        Managers.UI.ShowPopupUI<UI_ComfirmPopup>("UI_ComfirmPopup2").SetInfo(qustionText, action);
    }
}

public class BuyActionFactory
{
    public UnityAction CreateBuyAction()
    {
        return null;
    }
}
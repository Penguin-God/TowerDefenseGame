using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class Goods_UI : Multi_UI_Base
{
    enum Texts
    {
        ProductNameText,
        PriceText,
    }

    enum Images
    {
        GradePanel,
        CurrencyImage,
    }

    [SerializeField] ShopDataTransfer dataTransfer;
    public event Action<Goods_UI> OnSell;
    public void _Init()
    {
        dataTransfer = GetComponentInParent<ShopDataTransfer>();
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

    public void Setup(UI_RandomShopGoodsData data, RandomShopPanel_UI panel)
    {
        GetText((int)Texts.ProductNameText).text = data.Name;
        GetText((int)Texts.PriceText).text = data.Price.ToString();
        GetText((int)Texts.PriceText).color = dataTransfer.CurrencyToColor(data.CurrencyType);

        GetImage((int)Images.GradePanel).color = dataTransfer.GradeToColor(data.Grade);
        GetImage((int)Images.CurrencyImage).sprite = dataTransfer.CurrencyToSprite(data.CurrencyType);

        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => panel.Setup(data, gameObject));
        button.onClick.AddListener(() => Multi_Managers.Sound.PlayEffect(EffectSoundType.ShopGoodsClick));
        gameObject.SetActive(true);
    }

    public void Setup(UI_RandomShopGoodsData data, UnityAction<Action> clickAct)
    {
        GetText((int)Texts.ProductNameText).text = data.Name;
        GetText((int)Texts.PriceText).text = data.Price.ToString();
        GetText((int)Texts.PriceText).color = dataTransfer.CurrencyToColor(data.CurrencyType);

        GetImage((int)Images.GradePanel).color = dataTransfer.GradeToColor(data.Grade);
        GetImage((int)Images.CurrencyImage).sprite = dataTransfer.CurrencyToSprite(data.CurrencyType);

        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => clickAct?.Invoke(Sell));
        button.onClick.AddListener(() => Multi_Managers.Sound.PlayEffect(EffectSoundType.ShopGoodsClick));
        gameObject.SetActive(true);
    }

    void Sell()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        gameObject.SetActive(false);

        OnSell?.Invoke(this);
    }

    void OnDisable()
    {
        OnSell = null;
    }
}

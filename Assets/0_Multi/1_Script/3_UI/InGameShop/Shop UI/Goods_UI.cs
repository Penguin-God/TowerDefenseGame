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

    [SerializeField] GoodsLocation location;
    public GoodsLocation Loaction => location;
    [SerializeField] ShopDataTransfer dataTransfer;

    public void _Init()
    {
        dataTransfer = GetComponentInParent<ShopDataTransfer>();
        button = GetComponent<Button>();
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

    Button button;
    public void Setup(UI_RandomShopGoodsData data, UnityAction<UI_RandomShopGoodsData> clickAct)
    {
        GetText((int)Texts.ProductNameText).text = data.Name;
        GetText((int)Texts.PriceText).text = data.Price.ToString();
        GetText((int)Texts.PriceText).color = dataTransfer.CurrencyToColor(data.CurrencyType);

        GetImage((int)Images.GradePanel).color = dataTransfer.GradeToColor(data.Grade);
        GetImage((int)Images.CurrencyImage).sprite = dataTransfer.CurrencyToSprite(data.CurrencyType);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => clickAct?.Invoke(data));
        button.onClick.AddListener(() => Multi_Managers.Sound.PlayEffect(EffectSoundType.ShopGoodsClick));
        gameObject.SetActive(true);
    }
}

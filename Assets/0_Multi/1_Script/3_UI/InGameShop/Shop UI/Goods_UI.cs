using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] UI_RandomShopGoodsData _data; // 디버그용
    [SerializeField] ShopDataTransfer dataTransfer;
    protected override void Init()
    {
        dataTransfer = GetComponentInParent<ShopDataTransfer>();
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        gameObject.SetActive(false);
    }

    public void Setup(UI_RandomShopGoodsData data)
    {
        _data = data;
        GetText((int)Texts.ProductNameText).text = data.Name;
        GetText((int)Texts.PriceText).text = data.Price.ToString();
        GetText((int)Texts.PriceText).color = dataTransfer.CurrencyToColor(data.CurrencyType);

        GetImage((int)Images.GradePanel).color = dataTransfer.GradeToColor(data.Grade);
        GetImage((int)Images.CurrencyImage).sprite = dataTransfer.CurrencyToSprite(data.CurrencyType);

        gameObject.SetActive(true);
    }
}

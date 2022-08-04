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

    [SerializeField] ShopDataTransfer dataTransfer;
    public void _Init()
    {
        dataTransfer = GetComponentInParent<ShopDataTransfer>();
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        gameObject.SetActive(false);
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

        gameObject.SetActive(true);
    }

    void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }
}

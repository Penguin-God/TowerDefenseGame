using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Goods : UI_Base
{
    enum Texts
    {
        ProductNameText,
        PriceText,
    }

    enum Images
    {
        ColorPanel,
        CurrencyImage,
    }

    [SerializeField] GoodsLocation location;
    public GoodsLocation Loaction => location;
    [SerializeField] ShopDataTransfer dataTransfer;

    public void _Init()
    {
        dataTransfer = GetComponentInParent<ShopDataTransfer>();
        showPanelButton = GetComponent<Button>();
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

    [SerializeField] Sprite _goldImage;
    [SerializeField] Sprite _foodImage;
    Sprite CurrencyToSprite(GameCurrencyType type) => type == GameCurrencyType.Gold ? _goldImage : _foodImage;

    public void Setup(UnitUpgradeGoods upgradeGoods, BuyController buyController)
    {
        var goodsPresenter = new UnitUpgradeGoodsPresenter();
        var goodsData = new UnitUpgradeGoodsData(upgradeGoods);

        GetText((int)Texts.ProductNameText).text = goodsPresenter.BuildGoodsText(upgradeGoods);
        GetImage((int)Images.ColorPanel).color = goodsPresenter.GetUnitColor(upgradeGoods.TargetColor);
        GetText((int)Texts.PriceText).color = goodsPresenter.CurrencyToColor(goodsData.Currency);
        GetText((int)Texts.PriceText).text = goodsData.Price.ToString();
        GetImage((int)Images.CurrencyImage).sprite = CurrencyToSprite(goodsData.Currency);

        showPanelButton.onClick.RemoveAllListeners();
        showPanelButton.onClick.AddListener(() => Managers.UI.ShowPopupUI<UI_RandomShopPanel>("InGameShop/UnitUpgradeGoodsPanel").Setup(goodsData, buyController));
    }

    Button showPanelButton;
    public void Setup(UI_RandomShopGoodsData data, UnityAction<UI_RandomShopGoodsData> clickAct)
    {
        GetText((int)Texts.ProductNameText).text = data.Name;
        GetText((int)Texts.PriceText).text = data.Price.ToString();
        GetText((int)Texts.PriceText).color = dataTransfer.CurrencyToColor(data.CurrencyType);

        GetImage((int)Images.ColorPanel).color = dataTransfer.GradeToColor(data.Grade);
        GetImage((int)Images.CurrencyImage).sprite = dataTransfer.CurrencyToSprite(data.CurrencyType);

        showPanelButton.onClick.RemoveAllListeners();
        showPanelButton.onClick.AddListener(() => clickAct?.Invoke(data));
        showPanelButton.onClick.AddListener(() => Managers.Sound.PlayEffect(EffectSoundType.ShopGoodsClick));
        gameObject.SetActive(true);
    }
}

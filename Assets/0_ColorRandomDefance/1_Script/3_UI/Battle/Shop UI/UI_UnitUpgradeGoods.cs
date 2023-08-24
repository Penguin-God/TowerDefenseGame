using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitUpgradeGoods : UI_Base
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

    Button showPanelButton;
    public void _Init(UnitUpgradeShopController unitUpgradeShopController)
    {
        showPanelButton = GetComponent<Button>();
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        _buyController = unitUpgradeShopController;
    }

    [SerializeField] GoodsLocation _goodsLocation;
    public event Action<GoodsLocation> OnBuyGoods;
    [SerializeField] Sprite _goldImage;
    [SerializeField] Sprite _foodImage;
    Sprite CurrencyToSprite(GameCurrencyType type) => type == GameCurrencyType.Gold ? _goldImage : _foodImage;

    readonly UnitUpgradeGoodsPresenter _goodsPresenter = new UnitUpgradeGoodsPresenter();
    UnitUpgradeShopController _buyController;
    public void Setup(UnitUpgradeGoodsData upgradeData, UnitUpgradeShopData data)
    {
        var priceData = Multi_GameManager.Instance.BattleData.ShopPriceDataByUnitUpgradeData[upgradeData];

        GetText((int)Texts.ProductNameText).text = _goodsPresenter.BuildGoodsText(upgradeData, data);
        GetImage((int)Images.ColorPanel).color = _goodsPresenter.GetUnitColor(upgradeData.TargetColor);
        GetText((int)Texts.PriceText).color = _goodsPresenter.CurrencyToColor(priceData.CurrencyType);
        GetText((int)Texts.PriceText).text = priceData.Amount.ToString();
        GetImage((int)Images.CurrencyImage).sprite = CurrencyToSprite(priceData.CurrencyType);

        showPanelButton.onClick.RemoveAllListeners();
        showPanelButton.onClick.AddListener(() => ShowBuyWindow(upgradeData, priceData, data));
    }

    void ShowBuyWindow(UnitUpgradeGoodsData upgradeData, CurrencyData priceData, UnitUpgradeShopData data)
    {
        if(priceData.Amount <= 0) // 진짜 0원일 때도 있음
        {
            _buyController.Buy(upgradeData);
            return;
        }
        string qustionText = $"{_goodsPresenter.BuildGoodsText(upgradeData, data)}를 {new GameCurrencyPresenter().BuildCurrencyText(priceData)}에 구매하시겠습니까?";
        Managers.UI.ShowPopupUI<UI_ComfirmPopup>("UI_ComfirmPopup2").SetInfo(qustionText, () => _buyController.Buy(upgradeData));
    }

    public void Setup(UnitUpgradeData upgradeData)
    {
        var priceData = upgradeData.PriceData;

        GetText((int)Texts.ProductNameText).text = _goodsPresenter.BuildGoodsText(upgradeData);
        GetImage((int)Images.ColorPanel).color = _goodsPresenter.GetUnitColor(upgradeData.TargetColor);
        GetText((int)Texts.PriceText).color = _goodsPresenter.CurrencyToColor(priceData.CurrencyType);
        GetText((int)Texts.PriceText).text = priceData.Amount.ToString();
        GetImage((int)Images.CurrencyImage).sprite = CurrencyToSprite(priceData.CurrencyType);

        showPanelButton.onClick.RemoveAllListeners();
        showPanelButton.onClick.AddListener(() => ShowBuyWindow(upgradeData));
    }

    void ShowBuyWindow(UnitUpgradeData upgradeData)
    {
        if (upgradeData.PriceData.Amount <= 0) // 진짜 0원일 때도 있음
        {
            BuyGoods(upgradeData);
            return;
        }
        string qustionText = $"{_goodsPresenter.BuildGoodsText(upgradeData)}를 {new GameCurrencyPresenter().BuildCurrencyText(upgradeData.PriceData)}에 구매하시겠습니까?";
        Managers.UI.ShowPopupUI<UI_ComfirmPopup>("UI_ComfirmPopup2").SetInfo(qustionText, () => BuyGoods(upgradeData));
    }

    void BuyGoods(UnitUpgradeData upgradeData)
    {
        _buyController.Buy(upgradeData);
        OnBuyGoods?.Invoke(_goodsLocation);
    }
}

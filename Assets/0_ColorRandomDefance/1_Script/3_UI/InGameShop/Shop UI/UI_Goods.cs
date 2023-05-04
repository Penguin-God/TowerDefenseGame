using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    Button showPanelButton;
    public void _Init()
    {
        showPanelButton = GetComponent<Button>();
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

    [SerializeField] Sprite _goldImage;
    [SerializeField] Sprite _foodImage;
    Sprite CurrencyToSprite(GameCurrencyType type) => type == GameCurrencyType.Gold ? _goldImage : _foodImage;

    readonly UnitUpgradeGoodsPresenter GoodsPresenter = new UnitUpgradeGoodsPresenter();
    public void Setup(UnitUpgradeGoodsData upgradeData, UnitUpgradeShopController buyController, UnitUpgradeShopData data)
    {
        var priceData = Multi_GameManager.Instance.BattleData.ShopPriceDataByUnitUpgradeData[upgradeData];

        GetText((int)Texts.ProductNameText).text = GoodsPresenter.BuildGoodsText(upgradeData, data);
        GetImage((int)Images.ColorPanel).color = GoodsPresenter.GetUnitColor(upgradeData.TargetColor);
        GetText((int)Texts.PriceText).color = GoodsPresenter.CurrencyToColor(priceData.CurrencyType);
        GetText((int)Texts.PriceText).text = priceData.Amount.ToString();
        GetImage((int)Images.CurrencyImage).sprite = CurrencyToSprite(priceData.CurrencyType);

        showPanelButton.onClick.RemoveAllListeners();
        showPanelButton.onClick.AddListener(() => ShowBuyWindow(upgradeData, priceData, buyController, data));
    }

    void ShowBuyWindow(UnitUpgradeGoodsData upgradeData, CurrencyData priceData, UnitUpgradeShopController buyController, UnitUpgradeShopData data)
    {
        if(priceData.Amount <= 0)
        {
            buyController.Buy(upgradeData);
            return;
        }
        string qustionText = $"{GoodsPresenter.BuildGoodsText(upgradeData, data)}를 {new GameCurrencyPresenter().BuildCurrencyText(priceData)}에 구매하시겠습니까?";
        Managers.UI.ShowPopupUI<UI_ComfirmPopup>("UI_ComfirmPopup2").SetInfo(qustionText, () => buyController.Buy(upgradeData));
    }
}

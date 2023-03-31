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
    public void Setup(UnitUpgradeGoods upgradeGoods, BuyController buyController)
    {
        var goodsData = new UnitUpgradeGoodsData(upgradeGoods);

        GetText((int)Texts.ProductNameText).text = GoodsPresenter.BuildGoodsText(upgradeGoods);
        GetImage((int)Images.ColorPanel).color = GoodsPresenter.GetUnitColor(upgradeGoods.TargetColor);
        GetText((int)Texts.PriceText).color = GoodsPresenter.CurrencyToColor(goodsData.Currency);
        GetText((int)Texts.PriceText).text = goodsData.Price.ToString();
        GetImage((int)Images.CurrencyImage).sprite = CurrencyToSprite(goodsData.Currency);

        showPanelButton.onClick.RemoveAllListeners();
        showPanelButton.onClick.AddListener(() => ShowBuyWindow(goodsData, buyController));
    }

    void ShowBuyWindow(UnitUpgradeGoodsData goodsData, BuyController buyController)
    {
        string qustionText 
            = $"{GoodsPresenter.BuildGoodsText(goodsData.UpgradeGoods)}를 {new GameCurrencyPresenter().BuildCurrencyText(goodsData.Currency, goodsData.Price)}에 구매하시겠습니까?";
        Managers.UI.ShowPopupUI<UI_ComfirmPopup>("UI_ComfirmPopup2").SetInfo(qustionText, () => buyController.Buy(goodsData));
    }
}

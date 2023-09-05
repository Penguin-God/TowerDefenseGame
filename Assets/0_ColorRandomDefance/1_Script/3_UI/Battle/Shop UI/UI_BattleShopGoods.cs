using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct GoodsData
{
    [SerializeField] BattleShopGoodsType _goodsType;
    [SerializeField] float[] _datas;

    public BattleShopGoodsType GoodsType => _goodsType;
    public float[] Datas => _datas;
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
        ColorPanel,
    }

    enum Buttons
    {
        PanelButton,
        ChangeGoodsButton,
    }

    protected override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.ChangeGoodsButton).onClick.AddListener(ChangeGoods);
    }

    GoodsBuyController _goodsBuyController;
    [SerializeField] GoodsLocation _goodsLocation;
    public GoodsLocation GoodsLocation => _goodsLocation;
    public event Action<GoodsLocation> OnBuyGoods;
    Func<GoodsLocation, BattleShopGoodsData, BattleShopGoodsData> _getChangeGoods;
    BattleShopGoodsData _currentDisplayGoodsData;
    public void Inject(GoodsBuyController goodsBuyController, Func<GoodsLocation, BattleShopGoodsData, BattleShopGoodsData> changeGoods)
    {
        _goodsBuyController = goodsBuyController;
        _getChangeGoods = changeGoods;
    }

    public void InitGoods(BattleShopGoodsData goodsData)
    {
        CheckInit();
        DisplayGoods(goodsData);
        GetButton((int)Buttons.ChangeGoodsButton).gameObject.SetActive(true);
    }

    void ChangeGoods()
    {
        DisplayGoods(_getChangeGoods.Invoke(_goodsLocation, _currentDisplayGoodsData));
        GetButton((int)Buttons.ChangeGoodsButton).gameObject.SetActive(false);
    }

    void DisplayGoods(BattleShopGoodsData goodsData)
    {
        _currentDisplayGoodsData = goodsData;

        GetTextMeshPro((int)Texts.ProductNameText).text = goodsData.Name;
        GetTextMeshPro((int)Texts.PriceText).text = goodsData.PriceData.Amount.ToString();
        
        GetImage((int)Images.CurrencyImage).sprite = new SpriteUtility().GetBattleCurrencyImage(goodsData.PriceData.CurrencyType);
        SetPanelColor(goodsData.SellData);

        GetButton((int)Buttons.PanelButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.PanelButton).onClick.AddListener(() => ShowBuyWindow(goodsData));
    }

    void SetPanelColor(GoodsData goodsData)
    {
        UnitColor unitColor = UnitColor.Black;
        var convertor = new DataConvertUtili();
        switch (goodsData.GoodsType)
        {
            case BattleShopGoodsType.Unit:
                unitColor = convertor.ToUnitFlag(goodsData.Datas).UnitColor; break;
            case BattleShopGoodsType.UnitUpgrade:
                unitColor = convertor.ToUnitUpgradeData(goodsData.Datas).TargetColor; break;
        }
        GetImage((int)Images.ColorPanel).color = new SpriteUtility().GetUnitColor(unitColor);
    }

    void ShowBuyWindow(BattleShopGoodsData goodsData)
    {
        string qustionText = $"{goodsData.Name}{goodsData.InfoText} 구매하시겠습니까?";
        Managers.UI.ShowPopupUI<UI_ComfirmPopup>("UI_ComfirmPopup2").SetInfo(qustionText, () => Buy(goodsData));
    }

    void Buy(BattleShopGoodsData goodsData)
    {
        if (_goodsBuyController.TryBuy(goodsData))
        {
            OnBuyGoods?.Invoke(_goodsLocation);
        }
    }
}

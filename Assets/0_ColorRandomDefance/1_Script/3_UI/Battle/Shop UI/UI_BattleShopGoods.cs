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

    public GoodsData Clone(BattleShopGoodsType battleShopGoodsType, float[] datas)
    {
        _goodsType = battleShopGoodsType;
        _datas = datas;
        return this;
    }

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
    }

    protected override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
    }

    GoodsBuyController _goodsBuyController;
    [SerializeField] GoodsLocation _goodsLocation;
    public GoodsLocation GoodsLocation => _goodsLocation;
    public event Action<GoodsLocation> OnBuyGoods;
    public BattleShopGoodsData CurrentDisplayGoodsData { get; private set; }
    public void Inject(GoodsBuyController goodsBuyController)
    {
        _goodsBuyController = goodsBuyController;
    }

    public void DisplayGoods(BattleShopGoodsData goodsData)
    {
        CheckInit();
        CurrentDisplayGoodsData = goodsData;

        GetTextMeshPro((int)Texts.ProductNameText).text = goodsData.Name;
        GetTextMeshPro((int)Texts.PriceText).text = goodsData.PriceData.Amount.ToString();
        
        GetImage((int)Images.CurrencyImage).sprite = new SpriteUtility().GetBattleCurrencyImage(goodsData.PriceData.CurrencyType);
        SetPanelColor(goodsData.SellData);

        GetButton((int)Buttons.PanelButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.PanelButton).onClick.AddListener(() => _goodsBuyController.ShowBuyWindow(goodsData, () => OnSuccessBuy(goodsData)));
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

    void OnSuccessBuy(BattleShopGoodsData goodsData) => OnBuyGoods?.Invoke(_goodsLocation);
}

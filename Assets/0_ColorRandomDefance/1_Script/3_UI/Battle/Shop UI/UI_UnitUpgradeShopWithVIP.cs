using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum BattleShopGoodsType
{
    Unit,
    UnitUpgrade,
}

public class UI_UnitUpgradeShopWithVIP : UI_Base
{
    enum GameObjects
    {
        Goods,
        SpecialGoodsParent,
        ResetBackgound,
    }

    enum Texts
    {
        SpecialShopStackText
    }

    enum Buttons
    {
        ResetButton,
    }

    int _goodsBuyStack;
    int NeedStackForEnterSpecialShop;

    Dictionary<GoodsLocation, UI_BattleShopGoods> _goodsByLocation = new Dictionary<GoodsLocation, UI_BattleShopGoods>();
    Dictionary<GoodsLocation, GoodsManager<BattleShopGoodsData>> _goodsManagerByLocation = new Dictionary<GoodsLocation, GoodsManager<BattleShopGoodsData>>();
    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        UpdateVipStatkText();

        foreach (var goods in GetObject((int)GameObjects.Goods).GetComponentsInChildren<UI_UnitUpgradeGoods>())
            goods.OnBuyGoods += _ => IncreaseGoodsBuyStack();
        InitSpecailShop();
        ConfigureNormalShop();
        InitGoodsManger();
    }

    void InitGoodsManger()
    {
        foreach (GoodsLocation location in Enum.GetValues(typeof(GoodsLocation)))
        {
            string csv = Managers.Resources.Load<TextAsset>($"Data/SkillData/{Enum.GetName(typeof(GoodsLocation), location)}").text;
            var goodsManager = new GoodsManager<BattleShopGoodsData>(CsvUtility.CsvToArray<BattleShopGoodsData>(csv));
            _goodsManagerByLocation.Add(location, goodsManager);
        }
    }

    void InitSpecailShop()
    {
        foreach (var goods in GetObject((int)GameObjects.SpecialGoodsParent).GetComponentsInChildren<UI_BattleShopGoods>())
        {
            goods.Inject(_buyController, ChangeGoods);
            goods.OnBuyGoods += _ => ConfigureNormalShop();
            goods.OnBuyGoods += _ =>  _goodsManager.AddBackAllGoods();
            _goodsByLocation.Add(goods.GoodsLocation, goods);
        }
    }

    GoodsBuyController _buyController;
    GoodsManager<BattleShopGoodsData> _goodsManager;
    public void ReceiveInject(GoodsBuyController buyController, GoodsManager<BattleShopGoodsData> goodsManager, int needStack)
    {
        // NeedStackForEnterSpecialShop = needStack;
        NeedStackForEnterSpecialShop = 3;
        _buyController = buyController;
        _goodsManager = goodsManager;
    }

    void IncreaseGoodsBuyStack()
    {
        _goodsBuyStack++;
        if (_goodsBuyStack >= NeedStackForEnterSpecialShop)
        {
            ConfigureSpecialShop();
            _goodsBuyStack = 0;
        }
        UpdateVipStatkText();
    }

    void UpdateVipStatkText() => GetTextMeshPro((int)Texts.SpecialShopStackText).text = $"다음 특별 상점까지 구매해야하는 상품 개수 : {NeedStackForEnterSpecialShop - _goodsBuyStack}";

    BattleShopGoodsData ChangeGoods(GoodsLocation goodsLocation, BattleShopGoodsData prveiousGoodsData) => _goodsManager.ChangeGoods(prveiousGoodsData);

    void ConfigureSpecialShop()
    {
        ConfigureShop(isSpecialShop: true);

        foreach (var goods in GetObject((int)GameObjects.SpecialGoodsParent).GetComponentsInChildren<UI_BattleShopGoods>())
            goods.InitGoods(_goodsManager.GetRandomGoods());
    }

    void ConfigureNormalShop() => ConfigureShop(isSpecialShop: false);
    void ConfigureShop(bool isSpecialShop)
    {
        GetTextMeshPro((int)Texts.SpecialShopStackText).gameObject.SetActive(!isSpecialShop);
        GetObject((int)GameObjects.Goods).SetActive(!isSpecialShop);
        GetObject((int)GameObjects.ResetBackgound).SetActive(!isSpecialShop);

        GetObject((int)GameObjects.SpecialGoodsParent).SetActive(isSpecialShop);
    }
}

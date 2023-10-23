using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleShopGoodsType
{
    Unit,
    UnitUpgrade,
}

public class UI_BattleShopWithVIP : UI_Base
{
    enum GameObjects
    {
        GoodsParent,
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
    SpecialShopBuyController _buyController;
    BuyAction _buyActionFactory;
    Dictionary<GoodsLocation, GoodsManager<BattleShopGoodsData>> _goodsManagerByLocation;
    GoodsManager<BattleShopGoodsData> GetGoodsManager(GoodsLocation location) => _goodsManagerByLocation[location];

    public void ReceiveInject(SpecialShopBuyController buyController, BuyAction buyActionFactory, Dictionary<GoodsLocation, GoodsManager<BattleShopGoodsData>> goodsManagerByLocation, int needStack)
    {
        _buyController = buyController;
        _buyActionFactory = buyActionFactory;
        _goodsManagerByLocation = goodsManagerByLocation;
        NeedStackForEnterSpecialShop = needStack;
    }

    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        UpdateVipStatkText();

        foreach (var goods in GetObject((int)GameObjects.GoodsParent).GetComponentsInChildren<UI_BattleShopGoods>())
            goods.OnBuyGoods += _ => IncreaseGoodsBuyStack();
        InitSpecailShop();
        ConfigureNormalShop();
    }

    void InitSpecailShop()
    {
        foreach (var goods in GetObject((int)GameObjects.SpecialGoodsParent).GetComponentsInChildren<UI_BattleShopGoods>())
        {
            goods.Inject(_buyController, _buyActionFactory);
            goods.OnBuyGoods += _ => ConfigureNormalShop();
            goods.OnBuyGoods += _ => GetGoodsManager(goods.GoodsLocation).AddBackAllGoods();
        }

        foreach (var goods in GetObject((int)GameObjects.SpecialGoodsParent).GetComponentsInChildren<UI_GoodsChangeController>())
            goods.DependencyInject(ChangeGoods);
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

    BattleShopGoodsData ChangeGoods(GoodsLocation goodsLocation, BattleShopGoodsData prveiousGoodsData) => GetGoodsManager(goodsLocation).ChangeGoods(prveiousGoodsData);

    void ConfigureSpecialShop()
    {
        ConfigureShop(isSpecialShop: true);

        foreach (var goods in GetObject((int)GameObjects.SpecialGoodsParent).GetComponentsInChildren<UI_BattleShopGoods>())
            goods.DisplayGoods(GetGoodsManager(goods.GoodsLocation).GetRandomGoods());

        foreach (var goods in GetObject((int)GameObjects.SpecialGoodsParent).GetComponentsInChildren<UI_GoodsChangeController>())
            goods.ActiveButton();
    }

    void ConfigureNormalShop() => ConfigureShop(isSpecialShop: false);
    void ConfigureShop(bool isSpecialShop)
    {
        GetTextMeshPro((int)Texts.SpecialShopStackText).gameObject.SetActive(!isSpecialShop);
        GetObject((int)GameObjects.GoodsParent).SetActive(!isSpecialShop);
        GetObject((int)GameObjects.ResetBackgound).SetActive(!isSpecialShop);

        GetObject((int)GameObjects.SpecialGoodsParent).SetActive(isSpecialShop);
    }
}

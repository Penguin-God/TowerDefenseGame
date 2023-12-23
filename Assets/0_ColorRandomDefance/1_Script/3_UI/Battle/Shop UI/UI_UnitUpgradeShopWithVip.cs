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

public class UI_UnitUpgradeShopWithVip : UI_Base
{
    enum GameObjects
    {
        GoldShop,
        RunShop,
        VIPShop,
    }

    enum Texts
    {
        StackText,
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

        UpdateVipStatkText();

        GetComponent<UI_UnitUpgradeShop>().OnUpgradeUnit += IncreaseGoodsBuyStack;
        InitSpecailShop();
        ConfigureNormalShop();
    }

    void InitSpecailShop()
    {
        foreach (var goods in GetAllGoods())
        {
            goods.Inject(_buyController, _buyActionFactory);
            goods.OnBuyGoods += _ => ConfigureNormalShop();
            goods.OnBuyGoods += _ => GetGoodsManager(goods.GoodsLocation).AddBackAllGoods();
        }

        foreach (var goods in GetAllGoodsControllers())
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

    void UpdateVipStatkText() => GetTextMeshPro((int)Texts.StackText).text = $"다음 특별 상점까지 구매해야하는 상품 개수 : {NeedStackForEnterSpecialShop - _goodsBuyStack}";

    BattleShopGoodsData ChangeGoods(GoodsLocation goodsLocation, BattleShopGoodsData prveiousGoodsData) => GetGoodsManager(goodsLocation).ChangeGoods(prveiousGoodsData);

    IEnumerable<UI_BattleShopGoods> GetAllGoods() => GetObject((int)GameObjects.VIPShop).GetComponentsInChildren<UI_BattleShopGoods>();
    IEnumerable<UI_GoodsChangeController> GetAllGoodsControllers() => GetObject((int)GameObjects.VIPShop).GetComponentsInChildren<UI_GoodsChangeController>();

    void ConfigureSpecialShop()
    {
        ConfigureShop(isSpecialShop: true);

        foreach (var goods in GetAllGoods())
            goods.DisplayGoods(GetGoodsManager(goods.GoodsLocation).GetRandomGoods());

        foreach (var goods in GetAllGoodsControllers())
            goods.ActiveButton();
    }

    void ConfigureNormalShop() => ConfigureShop(isSpecialShop: false);
    void ConfigureShop(bool isSpecialShop)
    {
        GetTextMeshPro((int)Texts.StackText).gameObject.SetActive(!isSpecialShop);
        GetObject((int)GameObjects.GoldShop).SetActive(!isSpecialShop);
        GetObject((int)GameObjects.RunShop).SetActive(!isSpecialShop);

        GetObject((int)GameObjects.VIPShop).SetActive(isSpecialShop);
    }
}

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
    }

    void InitSpecailShop()
    {
        foreach (var goods in GetObject((int)GameObjects.SpecialGoodsParent).GetComponentsInChildren<UI_BattleShopGoods>())
        {
            goods.Inject(_buyController);
            goods.OnBuyGoods += _ => ConfigureNormalShop();
            goods.OnBuyGoods += _ =>  _goodsManager.AddBackAllGoods();
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

    void ConfigureSpecialShop()
    {
        ConfigureShop(isSpecialShop: true);

        foreach (var goods in GetObject((int)GameObjects.SpecialGoodsParent).GetComponentsInChildren<UI_BattleShopGoods>())
            goods.DecorateGoods(_goodsManager.GetRandomGoods());   
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

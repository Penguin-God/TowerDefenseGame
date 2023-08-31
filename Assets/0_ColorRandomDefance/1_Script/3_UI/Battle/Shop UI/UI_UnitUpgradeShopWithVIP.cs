using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

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
    }

    enum Texts
    {
        SpecialShopStackText
    }

    int _goodsBuyStack;
    int NeedStackForEnterSpecialShop;
    
    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));

        GetTextMeshPro((int)Texts.SpecialShopStackText).text = $"���� Ư�� �������� �����ؾ��ϴ� ��ǰ ���� : {NeedStackForEnterSpecialShop}";

        foreach (var goods in GetObject((int)GameObjects.Goods).GetComponentsInChildren<UI_UnitUpgradeGoods>())
            goods.OnBuyGoods += _ => AddStack();
        InitSpecailShop();
    }

    void InitSpecailShop()
    {
        foreach (var goods in GetObject((int)GameObjects.SpecialGoodsParent).GetComponentsInChildren<UI_BattleShopGoods>())
            goods.Inject(_buyController);
        foreach (var goods in GetObject((int)GameObjects.SpecialGoodsParent).GetComponentsInChildren<UI_BattleShopGoods>())
            goods.OnBuyGoods += _ => ConfigureNormalShop();
        foreach (var goods in GetObject((int)GameObjects.SpecialGoodsParent).GetComponentsInChildren<UI_BattleShopGoods>())
            goods._OnBuyGoods += _goodsManager.AddBackGoods;
    }

    GoodsBuyController _buyController;
    GoodsManager<BattleShopGoodsData> _goodsManager;
    public void ReceiveInject(GoodsBuyController buyController, GoodsManager<BattleShopGoodsData> goodsManager, int needStack)
    {
        NeedStackForEnterSpecialShop = needStack;
        _buyController = buyController;
        _goodsManager = goodsManager;
    }

    void AddStack()
    {
        _goodsBuyStack++;
        if (_goodsBuyStack >= NeedStackForEnterSpecialShop)
        {
            ConfigureSpecialShop();
            _goodsBuyStack = 0;
        }
        GetTextMeshPro((int)Texts.SpecialShopStackText).text = $"���� ���ڱ��� �����ؾ��ϴ� ��ǰ ���� : {NeedStackForEnterSpecialShop - _goodsBuyStack}";
    }

    void ConfigureSpecialShop()
    {
        GetTextMeshPro((int)Texts.SpecialShopStackText).gameObject.SetActive(false);
        GetObject((int)GameObjects.Goods).SetActive(false);

        foreach (var goods in GetObject((int)GameObjects.SpecialGoodsParent).GetComponentsInChildren<UI_BattleShopGoods>())
            goods.DisplayGoods(_goodsManager.GetRandomGoods());
        GetObject((int)GameObjects.SpecialGoodsParent).SetActive(true);
    }

    void ConfigureNormalShop()
    {
        GetTextMeshPro((int)Texts.SpecialShopStackText).gameObject.SetActive(true);
        GetObject((int)GameObjects.Goods).SetActive(true);
        GetObject((int)GameObjects.SpecialGoodsParent).SetActive(false);
    }
}

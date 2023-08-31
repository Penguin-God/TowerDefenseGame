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
    }

    GoodsBuyController _buyController;
    public void Inject(GoodsBuyController buyController, int needStack)
    {
        NeedStackForEnterSpecialShop = needStack;
        _buyController = buyController;
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
            goods.DisplayGoods(CreateGoods());
        GetObject((int)GameObjects.SpecialGoodsParent).SetActive(true);
    }

    BattleShopGoodsData CreateGoods()
    {
        string csv = Managers.Resources.Load<TextAsset>("Data/SkillData/VIPGoodsData").text;
        BattleShopGoodsData data = CsvUtility.CsvToArray<BattleShopGoodsData>(csv).ToList().GetRandom();
        return data;
    }

    void ConfigureNormalShop()
    {
        GetTextMeshPro((int)Texts.SpecialShopStackText).gameObject.SetActive(true);
        GetObject((int)GameObjects.Goods).SetActive(true);
        GetObject((int)GameObjects.SpecialGoodsParent).SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_UnitUpgradeShopWithVIP : UI_UnitUpgradeShop
{
    enum GameObjects
    {
        GamblePanel,
        Goods,
    }

    enum Texts
    {
        SpecialShopStackText
    }

    int _goodsBuyStack;
    int NeedStackForGamble;
    UI_GamblePanel _gamblePanel;

    protected override void Init()
    {
        base.Init();
        _buyController.OnBuyGoods += _ => AddStack();
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));

        GetTextMeshPro((int)Texts.SpecialShopStackText).text = $"���� Ư�� �������� �����ؾ��ϴ� ��ǰ ���� : {NeedStackForGamble}";
    }

    public void SetNeedStackForGamble(int needStack) => NeedStackForGamble = needStack;

    void AddStack()
    {
        _goodsBuyStack++;
        if (_goodsBuyStack >= NeedStackForGamble)
        {
            ConfigureSpecialShop();
            _goodsBuyStack = 0;
        }
        GetTextMeshPro((int)Texts.SpecialShopStackText).text = $"���� ���ڱ��� �����ؾ��ϴ� ��ǰ ���� : {NeedStackForGamble - _goodsBuyStack}";
    }

    void ConfigureSpecialShop()
    {
        GetTextMeshPro((int)Texts.SpecialShopStackText).gameObject.SetActive(false);
        GetObject((int)GameObjects.Goods).SetActive(false);
        _gamblePanel.gameObject.SetActive(true);
        _gamblePanel.SetupGamblePanel();
    }
}

public class SpecialShopInitializer
{
    public void ConfigureShop(Transform goodsParnet)
    {
        foreach(var goods in goodsParnet.GetComponentsInChildren<UI_BattleShopGoods>())
        {
            goods.DisplayGoods();
        }
    }
}

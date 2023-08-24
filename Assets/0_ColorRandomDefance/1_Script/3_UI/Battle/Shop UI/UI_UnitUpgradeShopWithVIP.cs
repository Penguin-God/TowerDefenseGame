using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

        foreach (var goods in GetObject((int)GameObjects.SpecialGoodsParent).GetComponentsInChildren<UI_UnitUpgradeGoods>())
            goods._Init(new UnitUpgradeShopController(_textController));
        foreach (var goods in GetObject((int)GameObjects.SpecialGoodsParent).GetComponentsInChildren<UI_UnitUpgradeGoods>())
            goods.OnBuyGoods += _ => ConfigureNormalShop();
    }

    TextShowAndHideController _textController;
    public void Inject(TextShowAndHideController textController, int needStack)
    {
        NeedStackForEnterSpecialShop = needStack;
        _textController = textController;
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
        
        foreach (var goods in GetObject((int)GameObjects.SpecialGoodsParent).GetComponentsInChildren<UI_UnitUpgradeGoods>())
            goods.Setup(new UnitUpgradeData(UnitUpgradeType.Scale, UnitColor.Orange, 50, new CurrencyData(GameCurrencyType.Food, 1)));
        GetObject((int)GameObjects.SpecialGoodsParent).SetActive(true);
    }

    void ConfigureNormalShop()
    {
        GetTextMeshPro((int)Texts.SpecialShopStackText).gameObject.SetActive(true);
        GetObject((int)GameObjects.Goods).SetActive(true);
        GetObject((int)GameObjects.SpecialGoodsParent).SetActive(false);
    }
}

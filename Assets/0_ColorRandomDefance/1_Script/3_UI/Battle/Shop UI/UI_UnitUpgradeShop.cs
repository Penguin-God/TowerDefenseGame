using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UnitUpgradeShop : UI_Popup
{
    enum GameObjects
    {
        GoldGoodsParent,
        RunGoodsParent,
    }

    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));

        CreateGoods(GetObject((int)GameObjects.GoldGoodsParent).transform, UnitUpgradeType.Value, 250, new CurrencyData(GameCurrencyType.Gold, 5));
        CreateGoods(GetObject((int)GameObjects.RunGoodsParent).transform, UnitUpgradeType.Scale, 25, new CurrencyData(GameCurrencyType.Rune, 2));
    }

    MultiUnitStatController _statController;
    public void DependencyInject(MultiUnitStatController statController)
    {
        _statController = statController;
    }

    void CreateGoods(Transform goodsParent, UnitUpgradeType unitUpgradeType, int value, CurrencyData currencyData)
    {
        foreach (Transform child in goodsParent)
            Destroy(child.gameObject);

        foreach (var color in UnitFlags.NormalColors)
            Managers.UI.MakeSubItem<UI_UnitUpgradeIcon>(goodsParent).FillGoods(color, new UnitUpgradeGoodsData(new UnitUpgradeData(unitUpgradeType, color, value), currencyData), _statController, 5);
    }
}

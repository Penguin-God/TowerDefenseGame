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

        CreateGoods(GetObject((int)GameObjects.GoldGoodsParent).transform);
        CreateGoods(GetObject((int)GameObjects.RunGoodsParent).transform);
    }

    MultiUnitStatController _statController;
    public void DependencyInject(MultiUnitStatController statController)
    {
        _statController = statController;
    }

    void CreateGoods(Transform goodsParent)
    {
        foreach (Transform child in goodsParent)
            Destroy(child.gameObject);
        
        foreach (var color in UnitFlags.NormalColors)
            Managers.UI.MakeSubItem<UI_UnitUpgradeIcon>(goodsParent).FillGoods(color, new UnitUpgradeData(UnitUpgradeType.Value, color, 250), _statController, 5);
    }
}

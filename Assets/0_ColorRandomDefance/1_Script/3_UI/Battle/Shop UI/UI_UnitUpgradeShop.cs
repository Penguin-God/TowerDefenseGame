using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UnitUpgradeShop : UI_Popup
{
    [SerializeField] Transform _goodsParent;

    protected override void Init()
    {
        CreateGoods();
    }

    MultiUnitStatController _statController;
    public void DependencyInject(MultiUnitStatController statController)
    {
        _statController = statController;
    }

    void CreateGoods()
    {
        foreach (Transform child in _goodsParent)
            Destroy(child.gameObject);
        
        foreach (var color in UnitFlags.NormalColors)
            Managers.UI.MakeSubItem<UI_UnitUpgradeIcon>(_goodsParent).FillGoods(color, new UnitUpgradeData(UnitUpgradeType.Value, color, 250), _statController, 5);
    }
}

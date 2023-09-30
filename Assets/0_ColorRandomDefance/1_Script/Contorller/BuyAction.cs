using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyAction
{
    readonly Multi_NormalUnitSpawner _unitSpawner;
    // readonly UnitUpgradeController _unitUpgradeController;
    readonly MultiUnitStatController _unitStatController;
    //public BuyAction(Multi_NormalUnitSpawner unitSpawner, UnitUpgradeController unitUpgradeController)
    //{
    //    _unitSpawner = unitSpawner;
    //    _unitUpgradeController = unitUpgradeController;
    //}

    public BuyAction(Multi_NormalUnitSpawner unitSpawner, MultiUnitStatController unitStatController)
    {
        _unitSpawner = unitSpawner;
        _unitStatController = unitStatController;
    }

    public void Do(GoodsData sellData)
    {
        var convertor = new DataConvertUtili();
        switch (sellData.GoodsType)
        {
            case BattleShopGoodsType.Unit: SpawnUnit(convertor.ToUnitFlag(sellData.Datas)); break;
            case BattleShopGoodsType.UnitUpgrade: UpgradeUnit(convertor.ToUnitUpgradeData(sellData.Datas)); break;
            default: Debug.LogError($"정의되지 않은 굿즈 타입 {sellData.GoodsType}"); break;
        }
    }

    void SpawnUnit(UnitFlags flag) => _unitSpawner.Spawn(flag);

    void UpgradeUnit(UnitUpgradeData goods)
    {
        const float Percentage = 100f;
        switch (goods.UpgradeType)
        {
            case UnitUpgradeType.Value:
                _unitStatController.AddUnitDamageValue(goods.TargetColor, goods.Value, UnitStatType.All); break;
            case UnitUpgradeType.Scale:
                _unitStatController.ScaleUnitDamageValue(goods.TargetColor, goods.Value / Percentage, UnitStatType.All); break;
        }
        Multi_GameManager.Instance.IncrementUnitUpgradeValue(goods);
    }
}

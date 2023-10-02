using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuyAction
{
    readonly Multi_NormalUnitSpawner _unitSpawner;
    readonly MultiUnitStatController _unitStatController;

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
        switch (goods.UpgradeType)
        {
            case UnitUpgradeType.Value: AddValue(goods); break;
            case UnitUpgradeType.Scale: UpScale(goods); break;
        }
        // Multi_GameManager.Instance.IncrementUnitUpgradeValue(goods);
    }

    void AddValue(UnitUpgradeData goods)
    {
        _unitStatController.AddUnitDamageValue(goods.TargetColor, goods.Value, UnitStatType.All);
        foreach (var flag in UnitFlags.AllClass.Select(x => new UnitFlags(goods.TargetColor, x)))
            _unitStatController.UnitStatController.AddUnitUpgradeValue(flag, goods.Value);
    }

    void UpScale(UnitUpgradeData goods)
    {
        const float Percentage = 100f;
        _unitStatController.ScaleUnitDamageValue(goods.TargetColor, goods.Value / Percentage, UnitStatType.All);
        foreach (var flag in UnitFlags.AllClass.Select(x => new UnitFlags(goods.TargetColor, x)))
            _unitStatController.UnitStatController.AddUnitUpgradeScale(flag, goods.Value);
    }
}

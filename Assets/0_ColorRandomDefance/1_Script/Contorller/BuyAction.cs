using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyAction
{
    readonly Multi_NormalUnitSpawner _unitSpawner;
    readonly UnitUpgradeController _unitUpgradeController;
    public BuyAction(Multi_NormalUnitSpawner unitSpawner, UnitUpgradeController unitUpgradeController)
    {
        _unitSpawner = unitSpawner;
        _unitUpgradeController = unitUpgradeController;
    }

    public void Do(GoodsData sellData)
    {
        var convertor = new DataConvertUtili();
        switch (sellData.GoodsType)
        {
            case BattleShopGoodsType.Unit: SpawnUnit(convertor.ToUnitFlag(sellData.Datas)); break;
            case BattleShopGoodsType.UnitUpgrade: UpgradeUnit(convertor.ToUnitUpgradeData(sellData.Datas)); break;
            default: Debug.LogError($"���ǵ��� ���� ���� Ÿ�� {sellData.GoodsType}"); break;
        }
    }

    void SpawnUnit(UnitFlags flag) => _unitSpawner.Spawn(flag);

    void UpgradeUnit(UnitUpgradeData goods)
    {
        const float Percentage = 100f;
        switch (goods.UpgradeType)
        {
            case UnitUpgradeType.Value:
                _unitUpgradeController.AddUnitDamageValue(goods.TargetColor, goods.Value, UnitStatType.All); break;
            case UnitUpgradeType.Scale:
                _unitUpgradeController.ScaleUnitDamageValue(goods.TargetColor, goods.Value / Percentage, UnitStatType.All); break;
        }
        Multi_GameManager.Instance.IncrementUnitUpgradeValue(goods);
    }
}
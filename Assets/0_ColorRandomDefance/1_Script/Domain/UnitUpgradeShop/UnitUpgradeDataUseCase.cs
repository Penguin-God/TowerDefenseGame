using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUpgradeDataUseCase
{
    public readonly UnitUpgradeGoodsData AddData;
    public readonly UnitUpgradeGoodsData ScaleData;
    public readonly int UpgradeMaxLevel;
    public UnitUpgradeDataUseCase(UnitUpgradeGoodsData addData, UnitUpgradeGoodsData scaleData, int upgradeMaxLevel)
    {
        AddData = addData;
        ScaleData = scaleData;
        UpgradeMaxLevel = upgradeMaxLevel;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum UnitUpgradeType
{
    Value,
    Scale,
}

public struct UnitUpgradeGoodsData
{
    public UnitUpgradeType UpgradeType { get; private set; }
    public UnitColor TargetColor { get; private set; }

    public UnitUpgradeGoodsData(UnitUpgradeType upgradeType, UnitColor color)
    {
        UpgradeType = upgradeType;
        TargetColor = color;
    }
}

public class UnitUpgradeGoodsSelector
{
    public IEnumerable<UnitUpgradeGoodsData> GetAllGoods()
        => Enum.GetValues(typeof(UnitUpgradeType))
        .Cast<UnitUpgradeType>()
        .SelectMany(upgradeType => Enum.GetValues(typeof(UnitColor))
        .Cast<UnitColor>()
        .Where(x => UnitFlags.SpecialColors.Contains(x) == false)
        .Select(color => new UnitUpgradeGoodsData(upgradeType, color)));
}

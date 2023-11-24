using System.Collections;
using System.Collections.Generic;


public enum UnitUpgradeType
{
    Value,
    Scale,
}

public struct UnitUpgradeData
{
    public UnitUpgradeType UpgradeType { get; private set; }
    public UnitColor TargetColor { get; private set; }
    public int Value { get; private set; }
    public UnitUpgradeData(UnitUpgradeType upgradeType, UnitColor color, int value)
    {
        UpgradeType = upgradeType;
        TargetColor = color;
        Value = value;
    }
}

public struct UnitUpgradeGoodsData
{
    public readonly UnitDamageInfo UpgradeInfo;
    public readonly UnitUpgradeType UpgradeType;
    public readonly CurrencyData Price;
    
    public UnitUpgradeGoodsData(UnitDamageInfo upgradeInfo, UnitUpgradeType upgradeType, CurrencyData price)
    {
        UpgradeInfo = upgradeInfo;
        UpgradeType = upgradeType;
        Price = price;
    }
}
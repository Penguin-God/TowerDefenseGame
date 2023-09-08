using System.Collections;
using System.Collections.Generic;

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
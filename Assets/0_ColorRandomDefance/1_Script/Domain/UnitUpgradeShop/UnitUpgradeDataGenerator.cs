using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UnitUpgradeDataGenerator
{
    public IEnumerable<UnitUpgradeData> GenerateAllUnitUpgradeDatas(int upValue, int scaleValue)
        => Enum.GetValues(typeof(UnitUpgradeType))
        .Cast<UnitUpgradeType>()
        .SelectMany(upgradeType => Enum.GetValues(typeof(UnitColor))
        .Cast<UnitColor>()
        .Where(x => UnitFlags.SpecialColors.Contains(x) == false)
        .Select(color => new UnitUpgradeData(upgradeType, color, upgradeType == UnitUpgradeType.Value ? upValue : scaleValue)));
}

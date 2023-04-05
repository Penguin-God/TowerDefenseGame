using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum UnitUpgradeType
{
    Value,
    Scale,
}

public struct UnitUpgradeData
{
    public UnitUpgradeType UpgradeType { get; private set; }
    public UnitColor TargetColor { get; private set; }

    public UnitUpgradeData(UnitUpgradeType upgradeType, UnitColor color)
    {
        UpgradeType = upgradeType;
        TargetColor = color;
    }
}

public class UnitUpgradeDataSelector
{
    readonly int GOODS_COUNT = 3;
    public IEnumerable<UnitUpgradeData> SelectGoodsSet() => SelectGoodsSet(GetAllGoods());

    public UnitUpgradeData SelectGoodsExcluding(IEnumerable<UnitUpgradeData> excludeGoods)
        => SelectGoodsSetExcluding(excludeGoods).First();

    public IEnumerable<UnitUpgradeData> SelectGoodsSetExcluding(IEnumerable<UnitUpgradeData> excludeGoods)
        => SelectGoodsSet(GetAllGoods().Except(excludeGoods));

    IEnumerable<UnitUpgradeData> SelectGoodsSet(IEnumerable<UnitUpgradeData> targetGoods)
    {
        var allGoods = targetGoods.ToList();
        var result = new List<UnitUpgradeData>();
        for (int i = 0; i < GOODS_COUNT; i++)
        {
            int randNum = UnityEngine.Random.Range(0, allGoods.Count);
            result.Add(allGoods[randNum]);
            allGoods.RemoveAt(randNum);
        }
        return result;
    }

    IEnumerable<UnitUpgradeData> GetAllGoods()
        => Enum.GetValues(typeof(UnitUpgradeType))
        .Cast<UnitUpgradeType>()
        .SelectMany(upgradeType => Enum.GetValues(typeof(UnitColor))
        .Cast<UnitColor>()
        .Where(x => UnitFlags.SpecialColors.Contains(x) == false)
        .Select(color => new UnitUpgradeData(upgradeType, color)));
}

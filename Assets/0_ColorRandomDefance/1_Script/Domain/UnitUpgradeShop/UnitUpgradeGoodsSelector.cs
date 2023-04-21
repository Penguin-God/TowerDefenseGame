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
    readonly int GOODS_COUNT = 3;
    public IEnumerable<UnitUpgradeGoodsData> SelectGoodsSet() => SelectGoodsSet(GetAllGoods());

    public UnitUpgradeGoodsData SelectGoodsExcluding(IEnumerable<UnitUpgradeGoodsData> excludeGoods)
        => SelectGoodsSetExcluding(excludeGoods).First();

    public IEnumerable<UnitUpgradeGoodsData> SelectGoodsSetExcluding(IEnumerable<UnitUpgradeGoodsData> excludeGoods)
        => SelectGoodsSet(GetAllGoods().Except(excludeGoods));

    IEnumerable<UnitUpgradeGoodsData> SelectGoodsSet(IEnumerable<UnitUpgradeGoodsData> targetGoods)
    {
        var allGoods = targetGoods.ToList();
        var result = new List<UnitUpgradeGoodsData>();
        for (int i = 0; i < GOODS_COUNT; i++)
        {
            int randNum = UnityEngine.Random.Range(0, allGoods.Count);
            result.Add(allGoods[randNum]);
            allGoods.RemoveAt(randNum);
        }
        return result;
    }

    public IEnumerable<UnitUpgradeGoodsData> GetAllGoods()
        => Enum.GetValues(typeof(UnitUpgradeType))
        .Cast<UnitUpgradeType>()
        .SelectMany(upgradeType => Enum.GetValues(typeof(UnitColor))
        .Cast<UnitColor>()
        .Where(x => UnitFlags.SpecialColors.Contains(x) == false)
        .Select(color => new UnitUpgradeGoodsData(upgradeType, color)));
}

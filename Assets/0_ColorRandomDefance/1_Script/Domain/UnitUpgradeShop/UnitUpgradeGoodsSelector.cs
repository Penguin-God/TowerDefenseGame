using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum UnitUpgradeType
{
    Value,
    Scale,
}

public struct UnitUpgradeGoods
{
    public static int ADD_DAMAGE => 50;
    public static float SCALE_DAMAGE_RATE => 10f;

    public UnitUpgradeType UpgradeType { get; private set; }
    public UnitColor TargetColor { get; private set; }

    public UnitUpgradeGoods(UnitUpgradeType upgradeType, UnitColor color)
    {
        UpgradeType = upgradeType;
        TargetColor = color;
    }
}

public class UnitUpgradeGoodsSelector
{
    readonly int GOODS_COUNT = 3;
    public IEnumerable<UnitUpgradeGoods> SelectGoodsSet() => SelectGoodsSet(GetAllGoods());

    public UnitUpgradeGoods SelectGoodsExcluding(IEnumerable<UnitUpgradeGoods> excludeGoods)
        => SelectGoodsSetExcluding(excludeGoods).First();

    public IEnumerable<UnitUpgradeGoods> SelectGoodsSetExcluding(IEnumerable<UnitUpgradeGoods> excludeGoods)
        => SelectGoodsSet(GetAllGoods().Except(excludeGoods));

    IEnumerable<UnitUpgradeGoods> SelectGoodsSet(IEnumerable<UnitUpgradeGoods> targetGoods)
    {
        var allGoods = targetGoods.ToList();
        var result = new List<UnitUpgradeGoods>();
        for (int i = 0; i < GOODS_COUNT; i++)
        {
            int randNum = UnityEngine.Random.Range(0, allGoods.Count);
            result.Add(allGoods[randNum]);
            allGoods.RemoveAt(randNum);
        }
        return result;
    }

    IEnumerable<UnitUpgradeGoods> GetAllGoods()
        => Enum.GetValues(typeof(UnitUpgradeType))
        .Cast<UnitUpgradeType>()
        .SelectMany(upgradeType => Enum.GetValues(typeof(UnitColor))
        .Cast<UnitColor>()
        .Where(x => UnitFlags.SpecialColors.Contains(x) == false)
        .Select(color => new UnitUpgradeGoods(upgradeType, color)));
}

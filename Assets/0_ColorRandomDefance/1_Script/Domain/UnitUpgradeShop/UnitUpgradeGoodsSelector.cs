using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UnitUpgradeType
{
    Value,
    Scale,
}

public struct UnitUpgradeGoods
{
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
    public IEnumerable<UnitUpgradeGoods> SelectGoods()
    {
        var allGoods = GetAllGoods().ToList();
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
        .Where(x => x != UnitColor.White && x != UnitColor.Black)
        .Select(color => new UnitUpgradeGoods(upgradeType, color)));
}

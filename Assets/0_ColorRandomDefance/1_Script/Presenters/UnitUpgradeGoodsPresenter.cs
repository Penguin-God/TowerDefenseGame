using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUpgradeGoodsPresenter
{
    static readonly IReadOnlyDictionary<UnitColor, Color32> Colors = new Dictionary<UnitColor, Color32>()
    {
        {UnitColor.Red, new Color32(221, 34, 22, 100) },
        {UnitColor.Blue, new Color32(90, 214, 255, 100) },
        {UnitColor.Yellow, new Color32(255, 255, 40, 100) },
        {UnitColor.Green, new Color32(98, 221, 189, 100) },
        {UnitColor.Orange, new Color32(249, 160, 58, 100) },
        {UnitColor.Violet, new Color32(108, 36, 188, 100) },
    };
    public Color GetUnitColor(UnitColor unitColor) => Colors[unitColor];

    public string BuildGoodsText(UnitUpgradeGoodsData upgradeGoods, UnitUpgradeShopData data)
        => $"{UnitPresenter.GetColorText(upgradeGoods.TargetColor)} 유닛 {GetUpgradeText(upgradeGoods.UpgradeType, data)} 증가";

    string GetUpgradeText(UnitUpgradeType upgradeType, UnitUpgradeShopData data) => upgradeType == UnitUpgradeType.Value ? $" 공격력 {data.AddValue}" : $" 공격력 {data.UpScale}%";

    static readonly IReadOnlyDictionary<GameCurrencyType, Color32> CurrencyColors = new Dictionary<GameCurrencyType, Color32>()
    {
        {GameCurrencyType.Gold, new Color32(255, 188, 0 , 255) },
        {GameCurrencyType.Food, new Color32(255, 104, 13 , 255) },
    };

    public Color CurrencyToColor(GameCurrencyType currency) => CurrencyColors[currency];
}
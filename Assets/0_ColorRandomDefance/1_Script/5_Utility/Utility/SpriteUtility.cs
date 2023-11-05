using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class SpriteUtility
{
    static readonly IReadOnlyDictionary<UnitColor, Color32> UnitColors = new Dictionary<UnitColor, Color32>()
    {
        {UnitColor.Red, new Color32(221, 34, 22, 255) },
        {UnitColor.Blue, new Color32(30, 30, 244, 255) },
        {UnitColor.Yellow, new Color32(230, 230, 40, 255) },
        {UnitColor.Green, new Color32(12, 221, 12, 255) },
        {UnitColor.Orange, new Color32(249, 100, 22, 255) },
        {UnitColor.Violet, new Color32(188, 36, 233, 255) },
        {UnitColor.White, new Color32(255, 255, 255, 255) },
        {UnitColor.Black, new Color32(0, 0, 0, 255) },
    };
    public static Color32 GetUnitColor(UnitColor unitColor) => UnitColors[unitColor];

    public static Sprite GetUnitClassIcon(UnitClass unitClass) => LoadImage($"UnitIcon/{Enum.GetName(typeof(UnitClass), unitClass)}");

    static readonly IReadOnlyDictionary<GameCurrencyType, Color32> CurrencyColors = new Dictionary<GameCurrencyType, Color32>()
    {
        {GameCurrencyType.Gold, new Color32(255, 188, 0 , 255) },
        {GameCurrencyType.Rune, new Color32(86, 29, 92 , 255) },
    };
    public static Color CurrencyToColor(GameCurrencyType currency) => CurrencyColors[currency];

    public static Sprite GetBattleCurrencyImage(GameCurrencyType gameCurrencyType) => LoadImage(gameCurrencyType == GameCurrencyType.Gold ? "Gold" : "Rune");
    static Sprite LoadImage(string imagePath) => Resources.Load<Sprite>($"Sprites/{imagePath}");
}

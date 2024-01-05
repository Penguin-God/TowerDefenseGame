using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class SpriteUtility
{
    static readonly IReadOnlyDictionary<UnitColor, Color32> UnitColors = new Dictionary<UnitColor, Color32>()
    {
        {UnitColor.Red, new Color32(240, 34, 22, 255) },
        {UnitColor.Blue, new Color32(20, 20, 244, 255) },
        {UnitColor.Yellow, new Color32(245, 210, 0, 255) },
        {UnitColor.Green, new Color32(12, 241, 12, 255) },
        {UnitColor.Orange, new Color32(220, 78, 15, 255) },
        {UnitColor.Violet, new Color32(180, 20, 230, 255) },
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
    public static Sprite GetSkillImage(SkillType skillType) => LoadImage(Managers.Data.UserSkill.GetSkillGoodsData(skillType).ImageName);
    public static Sprite LoadImage(string imagePath) => Resources.Load<Sprite>($"Sprites/{imagePath}");
}

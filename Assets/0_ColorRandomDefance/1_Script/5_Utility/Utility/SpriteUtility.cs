using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteUtility
{
    static readonly IReadOnlyDictionary<UnitColor, Color32> UnitColors = new Dictionary<UnitColor, Color32>()
    {
        {UnitColor.Red, new Color32(221, 34, 22, 100) },
        {UnitColor.Blue, new Color32(90, 214, 255, 100) },
        {UnitColor.Yellow, new Color32(255, 255, 40, 100) },
        {UnitColor.Green, new Color32(98, 221, 189, 100) },
        {UnitColor.Orange, new Color32(249, 160, 58, 100) },
        {UnitColor.Violet, new Color32(108, 36, 188, 100) },
        {UnitColor.White, new Color32(255, 255, 255, 100) },
    };
    public Color GetUnitColor(UnitColor unitColor) => UnitColors[unitColor];

    static readonly IReadOnlyDictionary<GameCurrencyType, Color32> CurrencyColors = new Dictionary<GameCurrencyType, Color32>()
    {
        {GameCurrencyType.Gold, new Color32(255, 188, 0 , 255) },
        {GameCurrencyType.Rune, new Color32(86, 29, 92 , 255) },
    };
    public Color CurrencyToColor(GameCurrencyType currency) => CurrencyColors[currency];

    public Sprite GetBattleCurrencyImage(GameCurrencyType gameCurrencyType) => LoadImage(gameCurrencyType == GameCurrencyType.Gold ? "Gold" : "Rune");
    Sprite LoadImage(string imagePath) => Resources.Load<Sprite>($"Sprites/{imagePath}");
}

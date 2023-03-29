using Codice.CM.Common.Merge;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDataTransfer : MonoBehaviour
{
    [SerializeField] Color[] gradeColors;
    public Color GradeToColor(int grade) => gradeColors[grade];

    [SerializeField] Color[] currecyTextColors;
    public Color CurrencyToColor(GameCurrencyType type) => type == GameCurrencyType.Gold ? currecyTextColors[0] : currecyTextColors[1];

    [SerializeField] Sprite goldImage;
    [SerializeField] Sprite foodImage;
    public Sprite CurrencyToSprite(GameCurrencyType type) => type == GameCurrencyType.Gold ? goldImage : foodImage;
}

public class UnitUpgradeGoodsPresenter
{
    readonly IReadOnlyDictionary<UnitColor, Color32> Colors = new Dictionary<UnitColor, Color32>()
    {
        {UnitColor.Red, new Color32(221, 34, 22, 100) },
        {UnitColor.Blue, new Color32(90, 214, 255, 100) },
        {UnitColor.Yellow, new Color32(255, 255, 40, 100) },
        {UnitColor.Green, new Color32(98, 221, 189, 100) },
        {UnitColor.Orange, new Color32(249, 160, 58, 100) },
        {UnitColor.Violet, new Color32(108, 36, 188, 100) },
    };

    public Color GetUnitColor(UnitColor unitColor) => Colors[unitColor];

    readonly IReadOnlyDictionary<UnitColor, string> ColorTexts = new Dictionary<UnitColor, string>()
    {
        {UnitColor.Red, "빨간" },
        {UnitColor.Blue, "파란" },
        {UnitColor.Yellow, "노란" },
        {UnitColor.Green, "초록" },
        {UnitColor.Orange, "주황" },
        {UnitColor.Violet, "보라" },
    };
    public string GetUnitColorText(UnitColor unitColor) => ColorTexts[unitColor];

    public GameCurrencyType GetCurrency(UnitUpgradeType upgradeType) => upgradeType == UnitUpgradeType.Value ? GameCurrencyType.Gold : GameCurrencyType.Food;
    public int GetPrice(UnitUpgradeType upgradeType) => upgradeType == UnitUpgradeType.Value ? 10 : 1;
}
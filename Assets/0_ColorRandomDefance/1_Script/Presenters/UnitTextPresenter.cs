﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitTextPresenter
{
    public static string GetUnitName(UnitFlags flag) => $"{GetColorText(flag.UnitColor)} {GetClassText(flag.UnitClass)}";
    public static string GetUnitNameWithColor(UnitFlags flag) => $"<color={ColorCodeByUnitColor[flag.UnitColor]}>{GetUnitName(flag)}</color>";

    static readonly IReadOnlyDictionary<UnitColor, string> ColorCodeByUnitColor = new Dictionary<UnitColor, string>()
    {
        {UnitColor.Red, "#FF0000" },
        {UnitColor.Blue, "#0000FF" },
        {UnitColor.Yellow, "#FFFF00" },
        {UnitColor.Green, "#00FF00" },
        {UnitColor.Orange, "#FFA500" },
        {UnitColor.Violet, "#800080" },
        {UnitColor.White, "#FFFFFF" },
        {UnitColor.Black, "#000000" },
    };

    static readonly IReadOnlyDictionary<UnitColor, string> ColorTexts = new Dictionary<UnitColor, string>()
    {
        {UnitColor.Red, "빨간" },
        {UnitColor.Blue, "파란" },
        {UnitColor.Yellow, "노란" },
        {UnitColor.Green, "초록" },
        {UnitColor.Orange, "주황" },
        {UnitColor.Violet, "보라" },
        {UnitColor.White, "하얀" },
        {UnitColor.Black, "검은" },
    };
    public static string GetColorText(UnitColor color) => ColorTexts[color];

    static readonly IReadOnlyDictionary<UnitClass, string> ClassTexts = new Dictionary<UnitClass, string>()
    {
        {UnitClass.Swordman, "기사" },
        {UnitClass.Archer, "궁수" },
        {UnitClass.Spearman, "창병" },
        {UnitClass.Mage, "마법사" },
    };
    public static string GetClassText(UnitClass unitClass) => ClassTexts[unitClass];
}
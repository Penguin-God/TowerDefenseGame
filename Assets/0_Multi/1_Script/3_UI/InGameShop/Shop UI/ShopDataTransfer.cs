﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDataTransfer : MonoBehaviour
{
    [SerializeField] Color[] gradeColors;
    public Color GradeToColor(int grade) => gradeColors[grade];

    [SerializeField] Color[] currecyTextColors;
    public Color CurrencyToColor(string currencyType) => currencyType == "Gold" ? currecyTextColors[0] : currecyTextColors[1];

    [SerializeField] Sprite goldImage;
    [SerializeField] Sprite foodImage;
    public Sprite CurrencyToSprite(string currencyType) => currencyType == "Gold" ? goldImage : foodImage;
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCurrencyPresenter
{
    public string BuildCurrencyText(GameCurrencyType currencyType, int amount) 
        => currencyType == GameCurrencyType.Gold ? BuildGoldText(amount) : BuildFoodText(amount);
    public string BuildGoldText(int amount) => $"골드 {amount}원";
    public string BuildFoodText(int amount) => $"고기 {amount}개";
}

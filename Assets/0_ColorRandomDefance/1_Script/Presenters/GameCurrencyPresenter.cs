using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCurrencyPresenter
{
    public string BuildCurrencyText(CurrencyData data)
        => BuildCurrencyText(data.CurrencyType, data.Amount);

    public string BuildCurrencyText(GameCurrencyType currencyType, int amount) 
        => currencyType == GameCurrencyType.Gold ? BuildGoldText(amount) : BuildFoodText(amount);
    public string BuildGoldText(int amount) => $"골드 {amount}원";
    public string BuildFoodText(int amount) => $"룬 {amount}개";
    public string BuildCurrencyTypeText(GameCurrencyType currencyType) => GameCurrencyType.Gold == currencyType ? "골드" : "룬";
}

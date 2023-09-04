using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCurrencyPresenter
{
    public string BuildCurrencyText(CurrencyData data) => BuildCurrencyText(data.CurrencyType, data.Amount);

    string BuildCurrencyText(GameCurrencyType currencyType, int amount)  => IsGold(currencyType) ? BuildGoldText(amount) : BuildFoodText(amount);
    string BuildGoldText(int amount) => $"{BuildCurrencyTypeText(GameCurrencyType.Gold)} {amount}원";
    string BuildFoodText(int amount) => $"{BuildCurrencyTypeText(GameCurrencyType.Rune)} {amount}개";
    public string BuildCurrencyTypeText(GameCurrencyType currencyType) => IsGold(currencyType) ? "골드" : "룬";

    bool IsGold(GameCurrencyType currencyType) => currencyType == GameCurrencyType.Gold;
}

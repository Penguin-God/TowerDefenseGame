using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    [SerializeField] BattleDataManager _battleData;
    public BattleDataManager BattleData => _battleData;
    CurrencyManager CurrencyManager => _battleData.CurrencyManager;

    UnitDamageInfoManager _unitDamageManager;
    public UnitDamageInfoManager UnitDamageManager => _unitDamageManager;

    public void Init(BattleDataManager battleData)
    {
        _battleData = battleData;
    }

    public void AddGold(int _addGold) => CurrencyManager.Gold += _addGold;
    public bool TryUseGold(int gold) => CurrencyManager.TryUseGold(gold);

    public void AddFood(int _addFood) => CurrencyManager.Food += _addFood;
    public bool TryUseFood(int food) => CurrencyManager.TryUseFood(food);

    public bool TryUseCurrency(GameCurrencyType currencyType, int quantity) => currencyType == GameCurrencyType.Gold ? TryUseGold(quantity) : TryUseFood(quantity);
}

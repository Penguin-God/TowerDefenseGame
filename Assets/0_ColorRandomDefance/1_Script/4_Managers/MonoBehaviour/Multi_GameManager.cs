using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Photon.Pun;
using Random = UnityEngine.Random;

public enum GameCurrencyType
{
    Gold,
    Rune,
}

[Serializable]
public class BattleDataController
{
    BattleEventDispatcher _dispatcher;
    public void Init(BattleDataContainer startData, BattleEventDispatcher dispatcher)
    {
        UnitSummonData = startData.UnitSummonData;
        _dispatcher = dispatcher;

        MaxUnit = startData.MaxUnit;
        MaxMonsterCount = startData.MaxMonsterCount;
        StageUpGold = startData.StageUpGold;
        YellowKnightRewardGold = startData.YellowKnightCombineGold;
        WhiteUnitTime = startData.WhiteUnitTime;
        UnitSellRewardDatas = startData.UnitSellRewardDatas.Select(x => x.Cloen()).ToArray();

        StageMonsetSpawnCount = startData.StageMonsetSpawnCount;
        MonsterSpawnDelayTime = startData.MonsterSpawnDelayTime;
        StageBreakTime = startData.StageBreakTime;
        MonsterResurrectionDelayTime = startData.MonsterResurrectionDelayTime;
    }

    public int MaxUnit { get; private set; }
    public void ChangeMaxUnit(int count)
    {
        MaxUnit = count;
        _dispatcher.NotifyMaxUnitCountChange(MaxUnit);
    }

    public UnitSummonData UnitSummonData;
    public int MaxMonsterCount { get; private set; }
    public int StageUpGold { get; private set; }
    public int YellowKnightRewardGold;
    public float WhiteUnitTime;

    public IReadOnlyList<CurrencyData> UnitSellRewardDatas { get; private set; }

    // 스테이지
    public int StageMonsetSpawnCount;
    public float MonsterSpawnDelayTime;
    public float StageBreakTime;
    public float MonsterResurrectionDelayTime;
    public float StageTime => (StageMonsetSpawnCount * MonsterSpawnDelayTime) + StageBreakTime;
}

[Serializable]
public struct UnitSummonData
{
    public int SummonPrice;
    public UnitColor SummonMaxColor;

    public UnitSummonData(int summonPrice, UnitColor maxColor)
    {
        SummonPrice = summonPrice;
        SummonMaxColor = maxColor;
    }

    public UnitColor SelectColor() => (UnitColor)Random.Range(0, (int)(SummonMaxColor + 1));
}

[Serializable]
public class CurrencyData
{
    [SerializeField] GameCurrencyType _currencyType;
    public GameCurrencyType CurrencyType => _currencyType;
    public void ChangedCurrencyType(GameCurrencyType newCurrency) => _currencyType = newCurrency;

    [SerializeField] int _amount;
    public int Amount => _amount;
    public void ChangeAmount(int newAmount) => _amount = newAmount;

    public CurrencyData(GameCurrencyType currencyType, int amount)
    {
        _currencyType = currencyType;
        _amount = amount;
    }
    public CurrencyData() { } // 리플랙션용

    public CurrencyData Cloen() => new CurrencyData(CurrencyType, _amount);
}

public interface IBattleCurrencyManager
{
    public int Gold { get; set; }
    public void AddGold(int amount);
    public void UseGold(int amount);

    public int Food { get; set; }
    public void AddFood(int amount);
    public void UseFood(int amount);
}

[Serializable]
public class CurrencyManager : IBattleCurrencyManager
{
    public int Gold { get; set; }
    public int Food { get; set; }

    public void AddGold(int amount) => Gold += amount;
    public void UseGold(int amount)
    {
        if(Gold >= amount)
            Gold -= amount;
    }

    public void AddFood(int amount) => Food += amount;

    public void UseFood(int amount)
    {
        if(Food >= amount)
            Food -= amount;
    }
}

public class Multi_GameManager : SingletonPun<Multi_GameManager>
{
    [SerializeField] BattleDataController _battleData = new BattleDataController();
    public BattleDataController BattleData => _battleData;
    
    public event Action<int> OnGoldChanged;
    public void UpdateGold(int amount)
    {
        _currencyManager.Gold = amount;
        OnGoldChanged?.Invoke(_currencyManager.Gold);
    }


    public event Action<int> OnFoodChanged;
    public void UpdateFood(int amount)
    {
        _currencyManager.Food = amount;
        OnFoodChanged?.Invoke(_currencyManager.Food);
    }

    IBattleCurrencyManager _currencyManager;
    public IBattleCurrencyManager CurrencyManager => _currencyManager;
    public bool TryUseGold(int gold)
    {
        if (_currencyManager.Gold >= gold)
        {
            _currencyManager.UseGold(gold);
            return true;
        }
        else
            return false;
    }

    public bool HasGold(int gold) => _currencyManager.Gold >= gold;
    public void AddFood(int amount) => _currencyManager.AddFood(amount);
    public bool TryUseFood(int amount)
    {
        if (_currencyManager.Food >= amount)
        {
            _currencyManager.UseFood(amount);
            return true;
        }
        else
            return false;
    }

    public bool TryUseCurrency(CurrencyData currencyData) => TryUseCurrency(currencyData.CurrencyType, currencyData.Amount);
    public bool TryUseCurrency(GameCurrencyType currencyType, int quantity) => currencyType == GameCurrencyType.Gold ? TryUseGold(quantity) : TryUseFood(quantity);

    public bool UnitOver => Managers.Unit.CurrentUnitCount >= _battleData.MaxUnit;

    public void Init(IBattleCurrencyManager currencyManager, MultiBattleDataController multiBattleDataController, BattleDataContainer battleDataContainer, BattleEventDispatcher dispatcher)
    {
        base.Init();

        _currencyManager = currencyManager;
        _battleData.Init(battleDataContainer, dispatcher);
        AddGold(battleDataContainer.Gold);
        AddFood(battleDataContainer.Food);
        dispatcher.OnGameStart += () => multiBattleDataController.IncreasedMaxUnitCount(battleDataContainer.MaxUnit);
    }

    [PunRPC]
    public void AddGold(int _addGold) => _currencyManager.AddGold(_addGold);
    public void AddGold_RPC(int _addGold, int id)
    {
        if (id == PlayerIdManager.Id)
            AddGold(_addGold);
        else
            photonView.RPC(nameof(AddGold), RpcTarget.Others, _addGold);
    }
}

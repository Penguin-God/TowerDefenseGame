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
public class BattleDataManager
{
    [SerializeField] BattleDataContainer _battleData;
    public BattleDataContainer BattleData => _battleData;
    [SerializeField] UnitUpgradeShopData _unitUpgradeShopData;
    public UnitUpgradeShopData UnitUpgradeShopData => _unitUpgradeShopData;

    public void Init(BattleDataContainer startData, UnitUpgradeShopData unitUpgradeShopData)
    {
        _battleData = startData.Clone();
        UnitSummonData = startData.UnitSummonData;

        _unitUpgradeShopData = unitUpgradeShopData.Clone();
        ShopPriceDataByUnitUpgradeData = new UnitUpgradeDataGenerator().GenerateAllUnitUpgradeDatas(_unitUpgradeShopData.AddValue, _unitUpgradeShopData.UpScale)
            .ToDictionary(x => x, x => x.UpgradeType == UnitUpgradeType.Value ? _unitUpgradeShopData.AddValuePriceData.Cloen() : _unitUpgradeShopData.UpScalePriceData.Cloen());
    }

    public event Action<int> OnMaxUnitChanged = null;
    public int MaxUnit { get => _battleData.MaxUnit; set { _battleData.MaxUnit = value; OnMaxUnitChanged?.Invoke(_battleData.MaxUnit); } }
    public int MaxEnemyCount => _battleData.MaxEnemy;
    public int StageUpGold { get => _battleData.StageUpGold; set => _battleData.StageUpGold = value; }
    public int YellowKnightRewardGold { get => _battleData.YellowKnightCombineGold; set => _battleData.YellowKnightCombineGold = value; }

    public UnitSummonData UnitSummonData;

    public IReadOnlyList<CurrencyData> UnitSellRewardDatas => _battleData.UnitSellRewardDatas;

    // 상점
    public IReadOnlyList<CurrencyData> WhiteUnitShopPriceDatas => _battleData.WhiteUnitPriceDatas;
    public CurrencyData MaxUnitIncreasePriceData => _battleData.MaxUnitIncreasePriceData;

    public IReadOnlyDictionary<UnitUpgradeData, CurrencyData> ShopPriceDataByUnitUpgradeData { get; private set; }
    public IEnumerable<CurrencyData> GetAllShopPriceDatas() 
        => ShopPriceDataByUnitUpgradeData.Values
            .Concat(WhiteUnitShopPriceDatas)
            .Concat(new CurrencyData[] { MaxUnitIncreasePriceData });
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
    [SerializeField] BattleDataManager _battleData = new BattleDataManager();
    public BattleDataManager BattleData => _battleData;
    
    public event Action<int> OnGoldChanged;
    public void UpdateGold(int amount)
    {
        _currencyManager.Gold = amount;
        OnGoldChanged?.Invoke(_currencyManager.Gold);
    }
    public int Gold => _currencyManager.Gold;


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

    UnitMaxCountController _unitMaxCountController;
    public void IncreasedMaxUnitCount(int amount) => _unitMaxCountController.IncreasedMaxUnitCount(amount);

    [SerializeField] UnitUpgradeShopData _unitUpgradeShopData;
    [SerializeField] BattleDataContainer _battleDataContainer;
    public void Init(IBattleCurrencyManager currencyManager, UnitMaxCountController unitMaxCountController, BattleDataContainer battleDataContainer, BattleEventDispatcher dispatcher)
    {
        base.Init();

        _currencyManager = currencyManager;
        _battleDataContainer = battleDataContainer;
        _battleData.Init(_battleDataContainer, _unitUpgradeShopData);
        AddGold(_battleDataContainer.Gold);
        AddFood(_battleDataContainer.Food);
        _unitMaxCountController = unitMaxCountController;
        dispatcher.OnGameStart += () => IncreasedMaxUnitCount(_battleDataContainer.MaxUnit);

        _addDamageValueByFlag = UnitFlags.NormalFlags.ToDictionary(x => x, x => 0);
        _upScaleValueByFlag = UnitFlags.NormalFlags.ToDictionary(x => x, x => 0);
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


    Dictionary<UnitFlags, int> _addDamageValueByFlag;
    public int GetUnitUpgradeShopAddDamageValue(UnitFlags flag) => _addDamageValueByFlag[flag];
    Dictionary<UnitFlags, int> _upScaleValueByFlag;
    public int GetUnitUpgradeShopUpScaleValue(UnitFlags flag) => _upScaleValueByFlag[flag];

    public void IncrementUnitUpgradeValue(UnitUpgradeData goodsData)
    {
        if (goodsData.UpgradeType == UnitUpgradeType.Value)
            IncrementUnitUpgradeValue(_addDamageValueByFlag, goodsData.Value, goodsData.TargetColor);
        else
            IncrementUnitUpgradeValue(_upScaleValueByFlag, goodsData.Value, goodsData.TargetColor);
    }

    void IncrementUnitUpgradeValue(Dictionary<UnitFlags, int> valueDict, int incrementValue, UnitColor color)
    {
        foreach (var flag in valueDict.Keys.Where(x => x.UnitColor == color).ToList())
            valueDict[flag] += incrementValue;
    }
}

public class RewradController : MonoBehaviourPun
{
    Multi_GameManager _gameManager;
    void Start()
    {
        _gameManager = Multi_GameManager.Instance;
        StageManager.Instance.OnUpdateStage += _stage => _gameManager.AddGold(_gameManager.BattleData.StageUpGold);

        if (PhotonNetwork.IsMasterClient)
        {
            _bossSpawner.OnDead += GetBossReward;
            Multi_SpawnManagers.TowerEnemy.OnDead += GetTowerReward;
        }
    }

    Multi_BossEnemySpawner _bossSpawner;
    public void Inject(Multi_BossEnemySpawner bossSpawner)
    {
        _bossSpawner = bossSpawner;
    }

    void GetBossReward(Multi_BossEnemy enemy)
    {
        if (enemy.UsingId == PlayerIdManager.Id)
            GetReward(enemy.BossData);
        else
            photonView.RPC(nameof(GetReward), RpcTarget.Others, enemy.BossData.Gold, enemy.BossData.Food);
    }

    void GetTowerReward(Multi_EnemyTower enemy)
    {
        if (enemy.UsingId == PlayerIdManager.Id)
            GetReward(enemy.TowerData);
        else
            photonView.RPC(nameof(GetReward), RpcTarget.Others, enemy.TowerData.Gold, enemy.TowerData.Food);
    }

    void GetReward(BossData data) => GetReward(data.Gold, data.Food);

    [PunRPC]
    void GetReward(int gold, int food)
    {
        _gameManager.AddGold(gold);
        _gameManager.AddFood(food);
    }
}
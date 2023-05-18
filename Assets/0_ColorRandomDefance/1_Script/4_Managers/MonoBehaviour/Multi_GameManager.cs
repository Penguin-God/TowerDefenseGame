using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using Photon.Pun;

public enum GameCurrencyType
{
    Gold,
    Food,
}

[Serializable]
public class BattleDataManager
{
    [SerializeField] BattleDataContainer _battleData;
    [SerializeField] UnitUpgradeShopData _unitUpgradeShopData;
    public UnitUpgradeShopData UnitUpgradeShopData => _unitUpgradeShopData;

    public BattleDataManager(BattleDataContainer startData, UnitUpgradeShopData unitUpgradeShopData)
    {
        _currencyManager = new CurrencyManager(startData.Gold, startData.Food);
        _battleData = startData.Clone();
        UnitSummonData = startData.UnitSummonData;

        _unitUpgradeShopData = unitUpgradeShopData.Clone();
        ShopPriceDataByUnitUpgradeData = new UnitUpgradeGoodsSelector().GetAllGoods()
            .ToDictionary(x => x, x => x.UpgradeType == UnitUpgradeType.Value ? _unitUpgradeShopData.AddValuePriceData.Cloen() : _unitUpgradeShopData.UpScalePriceData.Cloen());
    }

    [SerializeField] CurrencyManager _currencyManager;
    public CurrencyManager CurrencyManager => _currencyManager;

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

    public readonly IReadOnlyDictionary<UnitUpgradeGoodsData, CurrencyData> ShopPriceDataByUnitUpgradeData;
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

    public CurrencyData Cloen() => new CurrencyData(CurrencyType, _amount);
}

public interface IBattleCurrencyController
{
    public int CurrentGold { get; }
    public void AddGold(int amount);
    public void UseGold(int amount);

    public int CurrentFood { get; }
    public void AddFood(int amount);
    public void UseFood(int amount);
}

[Serializable]
public class CurrencyManager : IBattleCurrencyController
{
    public CurrencyManager(int startGold, int startFood) => (_gold, _food) = (startGold, startFood);

    [SerializeField] int _gold;
    public event Action<int> OnGoldChanged;
    public int Gold { get => _gold; set { _gold = value; OnGoldChanged?.Invoke(_gold); } }
    public bool TryUseGold(int gold)
    {
        if (_gold >= gold)
        {
            Gold -= gold;
            return true;
        }
        else
            return false;
    }

    [SerializeField] int _food;
    public event Action<int> OnFoodChanged;
    public int Food { get => _food; set { _food = value; OnFoodChanged?.Invoke(_food); } }

    public int CurrentGold => throw new NotImplementedException();

    public int CurrentFood => throw new NotImplementedException();

    public bool TryUseFood(int food)
    {
        if (_food >= food)
        {
            Food -= food;
            return true;
        }
        else
            return false;
    }

    public void AddGold(int amount)
    {
        throw new NotImplementedException();
    }

    public void UseGold(int amount)
    {
        throw new NotImplementedException();
    }

    public bool HasGold(int amount)
    {
        throw new NotImplementedException();
    }

    public void AddFood(int amount)
    {
        throw new NotImplementedException();
    }

    public void UseFood(int amount)
    {
        throw new NotImplementedException();
    }

    public bool HasFood(int amount)
    {
        throw new NotImplementedException();
    }
}

public class OtherPlayerData
{
    public OtherPlayerData(SkillType mainSkill, SkillType subSkill)
    {
        _mainSkill = mainSkill;
        _subSkill = subSkill;
    }

    SkillType _mainSkill;
    SkillType _subSkill;

    public SkillType MainSkill => _mainSkill;
    public SkillType SubSkill => _subSkill;
}

public class Multi_GameManager : SingletonPun<Multi_GameManager>
{
    [SerializeField] BattleDataManager _battleData;
    public BattleDataManager BattleData => _battleData;
    CurrencyManager CurrencyManager => _battleData.CurrencyManager;
    HashSet<UnitUpgradeGoodsData> _locationByGoods = new HashSet<UnitUpgradeGoodsData>();

    public event Action<int> OnGoldChanged;
    void Rasie_OnGoldChanged(int gold) => OnGoldChanged?.Invoke(gold);

    public event Action<int> OnFoodChanged;
    void Rasie_OnFoodChanged(int food) => OnFoodChanged?.Invoke(food);

    public bool UnitOver => Managers.Unit.CurrentUnitCount >= _battleData.MaxUnit;

    OtherPlayerData _otherPlayerData;
    public OtherPlayerData OtherPlayerData => _otherPlayerData;

    [PunRPC]
    public void CreateOtherPlayerData(SkillType mainSkill, SkillType subSkill) => _otherPlayerData = new OtherPlayerData(mainSkill, subSkill);

    // 임시
    [SerializeField] Button gameStartButton;
    [SerializeField] UnitUpgradeShopData _unitUpgradeShopData;
    [SerializeField] BattleDataContainer _battleDataContainer;
    public override void Init()
    {
        base.Init();

        _battleData = new BattleDataManager(_battleDataContainer, _unitUpgradeShopData);
        Managers.Sound.PlayBgm(BgmType.Default);
        if (PhotonNetwork.IsConnected)
            photonView.RPC(nameof(CreateOtherPlayerData), RpcTarget.Others, Managers.ClientData.EquipSkillManager.MainSkill, Managers.ClientData.EquipSkillManager.SubSkill);

        _addDamageValueByFlag = UnitFlags.NormalFlags.ToDictionary(x => x, x => 0);
        _upScaleValueByFlag = UnitFlags.NormalFlags.ToDictionary(x => x, x => 0);
    }

    void SetEvent()
    {
        CurrencyManager.OnGoldChanged += Rasie_OnGoldChanged;
        CurrencyManager.OnFoodChanged += Rasie_OnFoodChanged;
    }

    [HideInInspector]
    public bool isGameStart;
    public event Action OnGameStart;
    [PunRPC]
    void RPC_OnStart()
    {
        SetEvent();
        gameStartButton?.gameObject?.SetActive(false);
        OnGameStart?.Invoke();
        OnGameStart = null;
        isGameStart = true;
    }

    public void GameStart() => photonView.RPC(nameof(RPC_OnStart), RpcTarget.All);
    
    void Start() => gameObject.AddComponent<RewradController>();


    [PunRPC]
    public void AddGold(int _addGold) => CurrencyManager.Gold += _addGold;
    public void AddGold_RPC(int _addGold, int id)
    {
        if (id == PlayerIdManager.Id)
            AddGold(_addGold);
        else
            photonView.RPC(nameof(AddGold), RpcTarget.Others, _addGold);
    }

    public bool TryUseGold(int gold) => CurrencyManager.TryUseGold(gold);
    public bool HasGold(int gold) => CurrencyManager.Gold >= gold;
    public void AddFood(int _addFood) => CurrencyManager.Food += _addFood;
    public bool TryUseFood(int food) => CurrencyManager.TryUseFood(food);

    public bool TryUseCurrency(GameCurrencyType currencyType, int quantity) => currencyType == GameCurrencyType.Gold ? TryUseGold(quantity) : TryUseFood(quantity);



    Dictionary<UnitFlags, int> _addDamageValueByFlag;
    public int GetUnitUpgradeShopAddDamageValue(UnitFlags flag) => _addDamageValueByFlag[flag];
    Dictionary<UnitFlags, int> _upScaleValueByFlag;
    public int GetUnitUpgradeShopUpScaleValue(UnitFlags flag) => _upScaleValueByFlag[flag];

    public void IncrementUnitUpgradeValue(UnitUpgradeGoodsData goodsData)
    {
        if (goodsData.UpgradeType == UnitUpgradeType.Value)
            IncrementUnitUpgradeValue(_addDamageValueByFlag, BattleData.UnitUpgradeShopData.AddValue, goodsData.TargetColor);
        else
            IncrementUnitUpgradeValue(_upScaleValueByFlag, BattleData.UnitUpgradeShopData.UpScale, goodsData.TargetColor);
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
            Multi_SpawnManagers.BossEnemy.OnDead += GetBossReward;
            Multi_SpawnManagers.TowerEnemy.OnDead += GetTowerReward;
        }
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
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
public struct BattleStartData
{
    [SerializeField] int startGold;
    [SerializeField] int startFood;
    [SerializeField] int stageUpGold;
    [SerializeField] int startMaxUnitCount;
    [SerializeField] int enemyMaxCount;
    [SerializeField] int unitSummonPrice;
    [SerializeField] int unitSummonMaxColorNumber;
    [SerializeField] int startYellowKnightRewardGold;
    [SerializeField] PriceDataRecord unitSellPriceRecord;
    [SerializeField] PriceDataRecord whiteUnitPriceRecord;
    [SerializeField] PriceData maxUnitIncreaseRecord;

    public (int startGold, int startFood) StartCurrency => (startGold, startFood);
    public int StageUpGold => stageUpGold;
    public int StartMaxUnitCount => startMaxUnitCount;
    public int EnemyMaxCount => enemyMaxCount;
    public (int price, int maxColorNumber) UnitSummonData => (price: unitSummonPrice, maxColorNumber:unitSummonMaxColorNumber);
    public int YellowKnightRewardGold => startYellowKnightRewardGold;

    public PriceDataRecord UnitSellPriceRecord => unitSellPriceRecord;
    public PriceDataRecord WhiteUnitPriceRecord => whiteUnitPriceRecord;
    public PriceData MaxUnitIncreaseRecord => maxUnitIncreaseRecord;
}

[Serializable]
public class BattleDataManager
{
    public BattleDataManager(BattleStartData startData)
    {
        _currencyManager = new CurrencyManager(startData.StartCurrency);
        _maxUnit = startData.StartMaxUnitCount;
        _maxEnemyCount = startData.EnemyMaxCount;
        _stageUpGold = startData.StageUpGold;
        UnitSummonData = startData.UnitSummonData;
        _yellowKnightRewardGold = startData.YellowKnightRewardGold;
        _unitSellPriceRecord = startData.UnitSellPriceRecord;
        _whiteUnitPriceRecord = startData.WhiteUnitPriceRecord;
        _maxUnitIncreaseRecord = startData.MaxUnitIncreaseRecord;

        _unitSellRewardDatas = startData.UnitSellPriceRecord.PriceDatas.Select(x => new CurrencyData(x.CurrencyType, x.Price));
        _whiteUnitShopPriceDatas = startData.WhiteUnitPriceRecord.PriceDatas.Select(x => new CurrencyData(x.CurrencyType, x.Price));
        _maxUnitIncreasePriceData = new CurrencyData(startData.MaxUnitIncreaseRecord.CurrencyType, startData.MaxUnitIncreaseRecord.Price);
    }

    [SerializeField] CurrencyManager _currencyManager;
    public CurrencyManager CurrencyManager => _currencyManager;

    [SerializeField] int _maxUnit;
    public event Action<int> OnMaxUnitChanged = null;
    public int MaxUnit { get => _maxUnit; set { _maxUnit = value; OnMaxUnitChanged?.Invoke(_maxUnit); } }

    [SerializeField] int _maxEnemyCount;
    public int MaxEnemyCount => _maxEnemyCount;

    [SerializeField] int _stageUpGold;
    public int StageUpGold { get => _stageUpGold; set => _stageUpGold = value; }

    public (int price, int maxColorNumber) UnitSummonData;

    [SerializeField] int _yellowKnightRewardGold;
    public int YellowKnightRewardGold { get => _yellowKnightRewardGold; set => _yellowKnightRewardGold = value; }

    [SerializeField] PriceDataRecord _unitSellPriceRecord;
    public PriceDataRecord UnitSellPriceRecord => _unitSellPriceRecord;

    [SerializeField] PriceDataRecord _whiteUnitPriceRecord;
    public PriceDataRecord WhiteUnitPriceRecord => _whiteUnitPriceRecord;

    [SerializeField] PriceData _maxUnitIncreaseRecord;
    public PriceData MaxUnitIncreaseRecord => _maxUnitIncreaseRecord;


    [SerializeField] IEnumerable<CurrencyData> _unitSellRewardDatas;
    public IEnumerable<CurrencyData> UnitSellRewardDatas => _unitSellRewardDatas;

    // 상점
    [SerializeField] IEnumerable<CurrencyData> _whiteUnitShopPriceDatas;
    public IEnumerable<CurrencyData> WhiteUnitShopPriceDatas => _whiteUnitShopPriceDatas;

    [SerializeField] CurrencyData _maxUnitIncreasePriceData;
    public CurrencyData MaxUnitIncreasePriceData => _maxUnitIncreasePriceData;

    public static int VALUE_PRICE => 10;
    public static int SCALE_PRICE => 1;

    public readonly IReadOnlyDictionary<UnitUpgradeData, CurrencyData> UnitUpgradeShopPriceDatas
        = new UnitUpgradeDataSelector().GetAllGoods()
            .ToDictionary(x => x, x => new CurrencyData(
                x.UpgradeType == UnitUpgradeType.Value ? GameCurrencyType.Gold : GameCurrencyType.Food,
                x.UpgradeType == UnitUpgradeType.Value ? VALUE_PRICE : SCALE_PRICE
                ));

    public IEnumerable<PriceData> GetAllPriceDatas()
    {
        var result = new List<PriceData> { _maxUnitIncreaseRecord };
        return result.Concat(_whiteUnitPriceRecord.PriceDatas);
    }

    public IEnumerable<CurrencyData> _GetAllPriceDatas() 
        => UnitUpgradeShopPriceDatas.Values
            .Concat(_whiteUnitShopPriceDatas)
            .Concat(new CurrencyData[] { _maxUnitIncreasePriceData });
}

[Serializable]
public class PriceDataRecord
{
    [SerializeField] PriceData[] _priceDatas;
    public PriceData[] PriceDatas => _priceDatas;
    public PriceData GetData(int index) => _priceDatas[index];
}

[Serializable]
public class PriceData
{
    [SerializeField] GameCurrencyType _currencyType;
    public GameCurrencyType CurrencyType => _currencyType;
    public void ChangedCurrencyType(GameCurrencyType newCurrency) => _currencyType = newCurrency;

    [SerializeField] int _price;
    public int Price => _price;
    public void ChangePrice(int newPrice) => _price = newPrice;

    public string GetPriceDescription() => new CurrencyPresenter().GetPriceDescription(_price, _currencyType);
}

public class CurrencyData
{
    public GameCurrencyType CurrencyType { get; private set; }
    public void ChangedCurrencyType(GameCurrencyType newCurrency) => CurrencyType = newCurrency;
    public int Amount { get; private set; }
    public void ChangePrice(int newAmount) => Amount = newAmount;

    public CurrencyData(GameCurrencyType currencyType, int amount)
    {
        CurrencyType = currencyType;
        Amount = amount;
    }
}

class CurrencyPresenter
{
    string GetCurrencyKoreaText(GameCurrencyType type) => type == GameCurrencyType.Gold ? "골드" : "고기";
    string GetQuantityInfoText(GameCurrencyType type) => type == GameCurrencyType.Gold ? "원" : "개";
    public string GetPriceDescription(int price, GameCurrencyType type) => $"{GetCurrencyKoreaText(type)} {price}{GetQuantityInfoText(type)}";
}

[Serializable]
public class CurrencyManager
{
    public CurrencyManager((int startGold, int startFood) startCurrency)
    {
        Gold = startCurrency.startGold;
        Food = startCurrency.startFood;
    }

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
    HashSet<UnitUpgradeData> _locationByGoods = new HashSet<UnitUpgradeData>();

    public event Action<int> OnGoldChanged;
    void Rasie_OnGoldChanged(int gold) => OnGoldChanged?.Invoke(gold);

    public event Action<int> OnFoodChanged;
    void Rasie_OnFoodChanged(int food) => OnFoodChanged?.Invoke(food);

    public bool UnitOver => (Multi_UnitManager.Instance.CurrentUnitCount >= _battleData.MaxUnit);

    OtherPlayerData _otherPlayerData;
    public OtherPlayerData OtherPlayerData => _otherPlayerData;

    [PunRPC]
    public void CreateOtherPlayerData(SkillType mainSkill, SkillType subSkill) => _otherPlayerData = new OtherPlayerData(mainSkill, subSkill);

    // 임시
    [SerializeField] Button gameStartButton;
    protected override void Init()
    {
        base.Init();

        if (PhotonNetwork.IsMasterClient && gameStartButton != null)
            gameStartButton.onClick.AddListener(GameStart);
        else
            gameStartButton?.gameObject?.SetActive(false);

        _battleData = new BattleDataManager(Managers.Data.GetBattleStartData());
        Managers.Sound.PlayBgm(BgmType.Default);
        if (PhotonNetwork.IsConnected)
            photonView.RPC(nameof(CreateOtherPlayerData), RpcTarget.Others, Managers.ClientData.EquipSkillManager.MainSkill, Managers.ClientData.EquipSkillManager.SubSkill);
    }

    void SetEvent()
    {
        CurrencyManager.OnGoldChanged += Rasie_OnGoldChanged;
        CurrencyManager.OnFoodChanged += Rasie_OnFoodChanged;
        // UI 이벤트 업데이트
        CurrencyManager.Gold += 0;
        CurrencyManager.Food += 0;
        _battleData.MaxUnit += 0;
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

    public void AddFood(int _addFood) => CurrencyManager.Food += _addFood;
    public bool TryUseFood(int food) => CurrencyManager.TryUseFood(food);

    public bool TryUseCurrency(GameCurrencyType currencyType, int quantity) => currencyType == GameCurrencyType.Gold ? TryUseGold(quantity) : TryUseFood(quantity);
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
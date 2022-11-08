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

    public IEnumerable<PriceData> GetAllPriceDatas()
    {
        var result = new List<PriceData> { _maxUnitIncreaseRecord };
        return result.Concat(_whiteUnitPriceRecord.PriceDatas);
    }
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

public class Multi_GameManager : MonoBehaviourPunCallbacks
{
    public static Multi_GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<Multi_GameManager>();
            }
            return m_instance;
        }
    }

    private static Multi_GameManager m_instance;

    [ContextMenu("Logging Json")]
    void LoggingJson()
    {
        var data = JsonUtility.FromJson<BattleStartData>(Resources.Load<TextAsset>("Data/ClientData/BattleGameData").text);
        print(JsonUtility.ToJson(data, true));
    }
    
    [SerializeField] BattleDataManager _battleData;
    public BattleDataManager BattleData => _battleData;
    CurrencyManager CurrencyManager => _battleData.CurrencyManager;

    public event Action<int> OnGoldChanged;
    void Rasie_OnGoldChanged(int gold) => OnGoldChanged?.Invoke(gold);

    public event Action<int> OnFoodChanged;
    void Rasie_OnFoodChanged(int food) => OnFoodChanged?.Invoke(food);

    public bool UnitOver => Multi_UnitManager.Instance.CurrentUnitCount >= _battleData.MaxUnit;

    // 임시
    [SerializeField] Button gameStartButton;
    void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        if (PhotonNetwork.IsMasterClient)
            gameStartButton.onClick.AddListener(GameStart);
        else
            gameStartButton.gameObject.SetActive(false);

        _battleData = new BattleDataManager(Multi_Managers.Data.GetBattleStartData());
        Multi_Managers.Sound.PlayBgm(BgmType.Default);
    }

    void SetEvent()
    {
        CurrencyManager.OnGoldChanged += Rasie_OnGoldChanged;
        CurrencyManager.OnFoodChanged += Rasie_OnFoodChanged;
        // UI 업데이트
        CurrencyManager.Gold += 0;
        CurrencyManager.Food += 0;
    }

    [HideInInspector]
    public bool gameStart;
    public event Action OnStart;
    [PunRPC]
    void RPC_OnStart()
    {
        SetEvent();
        gameStartButton.gameObject.SetActive(false);
        gameStart = true;
        OnStart?.Invoke();
    }

    void GameStart() => photonView.RPC(nameof(RPC_OnStart), RpcTarget.All);

    void Start()
    {
        Multi_StageManager.Instance.OnUpdateStage += _stage => AddGold(_battleData.StageUpGold);
        Multi_EnemyManager.Instance.OnEnemyCountChanged += CheckGameOver;

        if (PhotonNetwork.IsMasterClient)
        {
            Multi_SpawnManagers.BossEnemy.OnDead += GetBossReward;
            Multi_SpawnManagers.TowerEnemy.OnDead += GetTowerReward;
        }
    }

    void GetBossReward(Multi_BossEnemy enemy)
    {
        if(enemy.UsingId == Multi_Data.instance.Id)
            GetReward(enemy.BossData);
        else
            photonView.RPC(nameof(GetBossReward), RpcTarget.Others, enemy.BossData.Gold, enemy.BossData.Food);
    }

    void GetTowerReward(Multi_EnemyTower enemy)
    {
        if (enemy.UsingId == Multi_Data.instance.Id)
            GetReward(enemy.TowerData);
        else
            photonView.RPC(nameof(GetBossReward), RpcTarget.Others, enemy.TowerData.Gold, enemy.TowerData.Food);
    }

    void GetReward(BossData data)
    {
        AddGold(data.Gold);
        AddFood(data.Food);
    }

    [PunRPC]
    void GetBossReward(int gold, int food)
    {
        AddGold(gold);
        AddFood(food);
    }

    [PunRPC]
    public void AddGold(int _addGold) => CurrencyManager.Gold += _addGold;
    public void AddGold_RPC(int _addGold, int id)
    {
        if (id == Multi_Data.instance.Id)
            AddGold(_addGold);
        else
            photonView.RPC(nameof(AddGold), RpcTarget.Others, _addGold);
    }
    public bool TryUseGold(int gold) => CurrencyManager.TryUseGold(gold);

    public void AddFood(int _addFood) => CurrencyManager.Food += _addFood;
    public bool TryUseFood(int food) => CurrencyManager.TryUseFood(food);

    public bool TryUseCurrency(GameCurrencyType currencyType, int quantity) => currencyType == GameCurrencyType.Gold ? TryUseGold(quantity) : TryUseFood(quantity);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) // 게임 테스트 용
        {
            if(Time.timeScale == 15f)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 15f;
            }
        }
    }

    void CheckGameOver(int enemyCount)
    {
        if(enemyCount >= _battleData.MaxEnemyCount)
        {
            Lose();
            photonView.RPC(nameof(Win), RpcTarget.Others);
        }
    }

    [PunRPC]
    void Win()
    {
        GameEnd("승리");
    }

    public void Lose()
    {
        GameEnd("패배");
    }

    void GameEnd(string message)
    {
        Multi_Managers.UI.ShowClickRockWaringText(message);
        Time.timeScale = 0;
        StartCoroutine(Co_AfterReturnLobby());
    }

    IEnumerator Co_AfterReturnLobby()
    {
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Multi_Managers.Scene.LoadScene(SceneTyep.클라이언트);
        Multi_Managers.Clear();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
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
    [SerializeField] int startYellowKnightRewardGold;
    [SerializeField] int startSwrodmanSellGold;
    [SerializeField] int startArcherSellGold;
    [SerializeField] int startSpearmanSellGold;
    [SerializeField] int startMageSellGold;

    public int StartGold => startGold;
    public int StartFood => startFood;
    public int StageUpGold => stageUpGold;
    public int StartMaxUnitCount => startMaxUnitCount;
    public int EnemyMaxCount => enemyMaxCount;
    public int YellowKnightRewardGold => startYellowKnightRewardGold;
    public int SwrodmanSellGold => startSwrodmanSellGold;
    public int ArcherSellGold => startArcherSellGold;
    public int SpearmanSellGold => startSpearmanSellGold;
    public int MageSellGold => startMageSellGold;
}

[Serializable]
public class BattleDataManager
{
    public BattleDataManager(BattleStartData startData)
    {
        _currencyManager = new CurrencyManager(startData);
        _maxUnit = startData.StartMaxUnitCount;
        _maxEnemyCount = startData.EnemyMaxCount;
        _stageUpGold = startData.StageUpGold;
        _yellowKnightRewardGold = startData.YellowKnightRewardGold;
        _swrodmanSellGold = startData.SwrodmanSellGold;
        _archerSellGold = startData.ArcherSellGold;
        _spearmanSellGold = startData.SpearmanSellGold;
        _mageSellGold = startData.MageSellGold;
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
    
    [SerializeField] int _yellowKnightRewardGold;
    public int YellowKnightRewardGold { get => _yellowKnightRewardGold; set => _yellowKnightRewardGold = value; }
    
    [SerializeField] int _swrodmanSellGold;
    public int SwordmanSellGold { get => _swrodmanSellGold; set => _swrodmanSellGold = value; }
    
    [SerializeField] int _archerSellGold;
    public int ArcherSellGold { get => _archerSellGold; set => _archerSellGold = value; }
    
    [SerializeField] int _spearmanSellGold;
    public int SpearmanSellGold { get => _spearmanSellGold; set => _spearmanSellGold = value; }

    [SerializeField] int _mageSellGold;
    public int MageSellGold { get => _mageSellGold; set => _mageSellGold = value; }
}

[Serializable]
public class CurrencyManager
{
    public CurrencyManager(BattleStartData startData)
    {
        Gold = startData.StartGold;
        Food = startData.StartFood;
    }

    public void SetStartData(BattleStartData startData)
    {
        Gold = startData.StartGold;
        Food = startData.StartFood;
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
    public bool TryUseCurrency(GameCurrencyType currencyType, int mount) => currencyType == GameCurrencyType.Gold ? TryUseGold(mount) : TryUseFood(mount);

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
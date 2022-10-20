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

    public int StartGold => startGold;
    public int StartFood => startFood;
    public int StageUpGold => stageUpGold;
    public int StartMaxUnitCount => startMaxUnitCount;
    public int EnemyMaxCount => enemyMaxCount;
}

[Serializable]
public class BattleSceneData
{
    public BattleSceneData(BattleStartData startData)
    {
        Gold = startData.StartGold;
        Food = startData.StartFood;
        _maxUnitCount = startData.StartMaxUnitCount;
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

    [SerializeField] int _maxUnitCount;
    public int MaxUnitCount { get => _maxUnitCount; set => _maxUnitCount = value; }
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

    [SerializeField] BattleSceneData _battleData;

    [SerializeField] BattleStartData _gameStartData;

    public event Action<int> OnGoldChanged;
    void Rasie_OnGoldChanged(int gold) => OnGoldChanged?.Invoke(gold);

    [SerializeField] int _gold;
    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            OnGoldChanged?.Invoke(_gold);
        }
    }

    public event Action<int> OnFoodChanged;
    void Rasie_OnFoodChanged(int food) => OnFoodChanged?.Invoke(food);

    [SerializeField] int _food;
    public int Food
    {
        get => _food;
        set
        {
            _food = value;
            OnFoodChanged?.Invoke(_food);
        }
    }

    int AddGold_WhenCombine_YellowKinght;
    int stageUpGold = 10;
    [SerializeField] int _maxEnemyCount;
    public int MaxEnemyCount => _maxEnemyCount;

    [SerializeField] int _maxUninCount;
    public int MaxUnitCount => _maxUninCount;
    public event Action<int> OnUnitMaxCountChanaged = null;
    public void IncreaseUnitMaxCount()
    {
        _maxUninCount++;
        OnUnitMaxCountChanaged?.Invoke(_maxEnemyCount);
    }
    public bool UnitOver => Multi_UnitManager.Instance.CurrentUnitCount >= _maxUninCount;

    // 임시
    [SerializeField] Button gameStartButton;
    [SerializeField] GameObject barrierUI;
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

        _battleData = new BattleSceneData(Multi_Managers.Data.GetBattleStartData());
        _battleData.OnGoldChanged += Rasie_OnGoldChanged;
        _battleData.OnFoodChanged += Rasie_OnFoodChanged;

        _gameStartData = Multi_Managers.Data.GetBattleStartData();
        var gameStartData = Multi_Managers.Data.GetBattleStartData();
        stageUpGold = gameStartData.StageUpGold;
        _maxEnemyCount = gameStartData.EnemyMaxCount;
        Multi_Managers.Sound.PlayBgm(BgmType.Default);
    }


    [HideInInspector]
    public bool gameStart;
    public event Action OnStart;
    [PunRPC]
    void RPC_OnStart()
    {
        gameStartButton.gameObject.SetActive(false);
        barrierUI.SetActive(false);
        gameStart = true;
        OnStart();
    }

    void GameStart() => photonView.RPC(nameof(RPC_OnStart), RpcTarget.All);


    void Start()
    {
        Gold = _gameStartData.StartGold;
        Food = _gameStartData.StartFood;
        

        Multi_StageManager.Instance.OnUpdateStage += _stage => AddGold(stageUpGold);
        Multi_EnemyManager.Instance.OnEnemyCountChang += CheckGameOver;
        SubSound();

        if (PhotonNetwork.IsMasterClient)
        {
            Multi_SpawnManagers.BossEnemy.OnDead += GetBossReward;
            Multi_SpawnManagers.TowerEnemy.OnDead += GetTowerReward;
        }
    }

    void SubSound()
    {
        var sound = Multi_Managers.Sound;
        // 빼기
        Multi_SpawnManagers.BossEnemy.rpcOnSpawn -= () => sound.PlayBgm(BgmType.Boss);
        Multi_SpawnManagers.BossEnemy.rpcOnDead -= () => sound.PlayBgm(BgmType.Default);

        Multi_SpawnManagers.BossEnemy.rpcOnDead -= () => sound.PlayEffect(EffectSoundType.BossDeadClip);
        Multi_SpawnManagers.TowerEnemy.OnDead -= (tower) => sound.PlayEffect(EffectSoundType.TowerDieClip);
        Multi_StageManager.Instance.OnUpdateStage -= (stage) => sound.PlayEffect(EffectSoundType.NewStageClip);

        // 더하기
        Multi_SpawnManagers.BossEnemy.rpcOnSpawn += () => sound.PlayBgm(BgmType.Boss);
        Multi_SpawnManagers.BossEnemy.rpcOnDead += () => sound.PlayBgm(BgmType.Default);

        Multi_SpawnManagers.BossEnemy.rpcOnDead += () => sound.PlayEffect(EffectSoundType.BossDeadClip);
        Multi_SpawnManagers.TowerEnemy.OnDead += (tower) => sound.PlayEffect(EffectSoundType.TowerDieClip);
        Multi_StageManager.Instance.OnUpdateStage += (stage) => sound.PlayEffect(EffectSoundType.NewStageClip);
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
    public void AddGold(int _addGold)
    {
        //Gold += _addGold;
        _battleData.Gold += _addGold;
        //OnGoldChanged?.Invoke(_battleData.Gold);
    }
    public void AddGold_RPC(int _addGold, int id)
    {
        if (id == Multi_Data.instance.Id)
            AddGold(_addGold);
        else
            photonView.RPC(nameof(AddGold), RpcTarget.Others, _addGold);
    }
    public bool TryUseGold(int gold) => _battleData.TryUseGold(gold);


    public void AddFood(int _addFood)
    {
        //Food += _addFood;
        _battleData.Gold += _addFood;
        //OnFoodChanged?.Invoke(_battleData.Food);
    }

    public bool TryUseFood(int food)
    {
        return _battleData.TryUseFood(food);

        if (_battleData.TryUseFood(food))
        {
            Food -= food;
            return true;
        }
        else
            return false;
    }

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
        if(enemyCount >= _maxEnemyCount)
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
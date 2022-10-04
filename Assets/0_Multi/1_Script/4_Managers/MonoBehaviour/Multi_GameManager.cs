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
public struct BattleGameData
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

    [SerializeField] BattleGameData _gameData;

    public event Action<int> OnGoldChanged;
    public event Action<int> OnFoodChanged;

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
    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        if (PhotonNetwork.IsMasterClient)
            gameStartButton.onClick.AddListener(GameStart);
        else
            gameStartButton.gameObject.SetActive(false);

        _gameData = Multi_Managers.Data.BattleGameData;
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

    void GameStart() => photonView.RPC("RPC_OnStart", RpcTarget.All);


    void Start()
    {
        Gold = _gameData.StartGold;
        Food = _gameData.StartFood;
        stageUpGold = _gameData.StageUpGold;
        _maxUninCount = _gameData.StartMaxUnitCount;
        _maxEnemyCount = _gameData.EnemyMaxCount;

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
        // TODO : event 마스터만 구독하는 문제 해결하기
        var sound = Multi_Managers.Sound;
        // 빼기
        Multi_SpawnManagers.BossEnemy.OnSpawn -= (boss) => sound.PlayBgm(BgmType.Boss);
        Multi_SpawnManagers.BossEnemy.OnDead -= (boss) => sound.PlayBgm(BgmType.Default);

        Multi_SpawnManagers.BossEnemy.OnDead -= (boss) => sound.PlayEffect(EffectSoundType.BossDeadClip);
        Multi_SpawnManagers.TowerEnemy.OnDead -= (tower) => sound.PlayEffect(EffectSoundType.TowerDieClip);
        Multi_StageManager.Instance.OnUpdateStage -= (stage) => sound.PlayEffect(EffectSoundType.NewStageClip);

        // 더하기
        Multi_SpawnManagers.BossEnemy.OnSpawn += (boss) => DoActionIfSameId(() => sound.PlayBgm(BgmType.Boss), boss);
        Multi_SpawnManagers.BossEnemy.OnDead += (boss) => DoActionIfSameId(() => sound.PlayBgm(BgmType.Default), boss);

        Multi_SpawnManagers.BossEnemy.OnDead += (boss) => DoActionIfSameId(() => sound.PlayEffect(EffectSoundType.BossDeadClip), boss);
        Multi_SpawnManagers.TowerEnemy.OnDead += (tower) => sound.PlayEffect(EffectSoundType.TowerDieClip);
        Multi_StageManager.Instance.OnUpdateStage += (stage) => sound.PlayEffect(EffectSoundType.NewStageClip);
    }

    void DoActionIfSameId(Action action, Component component)
    {
        var rpcable = component.GetComponent<RPCable>();
        if (rpcable != null)
            DoActionIfSameId(action, rpcable.UsingId);
    }

    void DoActionIfSameId(Action action, int id)
    {
        if (id == Multi_Data.instance.Id)
            action?.Invoke();
    }

    void GetBossReward(Multi_BossEnemy enemy)
    {
        if(enemy.UsingId == Multi_Data.instance.Id)
            GetReward(enemy.BossData);
        else
            photonView.RPC("GetBossReward", RpcTarget.Others, enemy.BossData.Gold, enemy.BossData.Food);
    }

    void GetTowerReward(Multi_EnemyTower enemy)
    {
        if (enemy.UsingId == Multi_Data.instance.Id)
            GetReward(enemy.TowerData);
        else
            photonView.RPC("GetBossReward", RpcTarget.Others, enemy.TowerData.Gold, enemy.TowerData.Food);
    }

    void GetReward(BossData data)
    {
        AddGold(data.Gold);
        AddFood(data.Food);
    }

    // TODO : 외부에 고기 혐오자 스킬 구현
    //public void FoodToGold(int rate)
    //{
    //    if (_food <= 0) return;
    //    AddGold(_food * rate);
    //    Food = 0;
    //}

    [PunRPC]
    void GetBossReward(int gold, int food)
    {
        AddGold(gold);
        AddFood(food);
    }

    [PunRPC]
    public void AddGold(int _addGold) => Gold += _addGold;
    public void AddGold_RPC(int _addGold, int id)
    {
        if (id == Multi_Data.instance.Id)
            AddGold(_addGold);
        else
            photonView.RPC("AddGold", RpcTarget.Others, _addGold);
    }
    public void AddFood(int _addFood) => Food += _addFood;

    public bool TryUseGold(int gold)
    {
        if (Gold >= gold)
        {
            Gold -= gold;
            return true;
        }
        else
            return false;
    }

    public bool TryUseFood(int food)
    {
        if (Food >= food)
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
            photonView.RPC("Win", RpcTarget.Others);
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
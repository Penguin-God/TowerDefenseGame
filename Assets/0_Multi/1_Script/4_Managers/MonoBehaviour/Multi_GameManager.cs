using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;

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

    [SerializeField] int gold;
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            OnGoldChanged?.Invoke(gold);
        }
    }

    [SerializeField] int food;
    public int Food
    {
        get => food;
        set
        {
            food = value;
            OnFoodChanged?.Invoke(food);
        }
    }

    int stageUpGold = 10;
    [SerializeField] int _maxEnemyCount;
    [SerializeField] int _maxUninCount;
    public int MaxUnitCount => _maxUninCount;
    public bool UnitOver => Multi_UnitManager.Count.CurrentUnitCount >= _maxUninCount;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
        gameManagerAudio = GetComponent<AudioSource>();
        _gameData = Multi_Managers.Data.BattleGameData;
    }


    [HideInInspector]
    public bool gameStart;
    public event Action OnStart;
    [PunRPC]
    void RPC_OnStart()
    {
        gameStart = true;
        OnStart();
    }

    public void GameStart(string difficult)
    {
        if (PhotonNetwork.IsMasterClient) photonView.RPC("RPC_OnStart", RpcTarget.All);
    }

  
    void Start()
    {
        Gold = _gameData.StartGold;
        Food = _gameData.StartFood;
        stageUpGold = _gameData.StageUpGold;
        _maxUninCount = _gameData.StartMaxUnitCount;
        _maxEnemyCount = _gameData.EnemyMaxCount;

        Multi_StageManager.Instance.OnUpdateStage += _stage => AddGold(stageUpGold);

        Multi_EnemyManager.Instance.OnEnemyCountChanged += CheckGameOver;

        Multi_SpawnManagers.BossEnemy.OnDead += GetReward;
        Multi_SpawnManagers.TowerEnemy.OnDead += GetReward;
    }

    void GetReward(Multi_BossEnemy boss)
    {
        if (Multi_Data.instance.CheckIdSame(boss.GetComponent<RPCable>().UsingId))
            GetReward(boss.BossData);
    }

    void GetReward(Multi_EnemyTower tower)
    {
        if (Multi_Data.instance.CheckIdSame(tower.GetComponent<RPCable>().UsingId))
            GetReward(tower.TowerData);
    }

    void GetReward(BossData data)
    {
        AddGold(data.Gold);
        AddFood(data.Food);
    }

    public void AddGold(int _addGold) => Gold += _addGold;
    public void AddFood(int _addFood) => Food += _addFood;

    public bool TryUseGold(int _gold)
    {
        if (Gold >= _gold)
        {
            Gold -= _gold;
            return true;
        }
        else
            return false;
    }

    public bool TryUseFood(int _food)
    {
        if (Food >= _food)
        {
            Food -= _food;
            return true;
        }
        else
            return false;
    }

    public bool TryUseCurrency(string currencyType, int mount) => currencyType == "Gold" ? TryUseGold(mount) : TryUseFood(mount);

    public AudioClip bossbgmClip;
    public AudioClip bgmClip;

    public AudioSource gameManagerAudio;

    public void ChangeBGM(AudioClip bgmClip)
    {
        gameManagerAudio.clip = bgmClip;
        gameManagerAudio.Play();
    }

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

    void GameEnd(string msg)
    {
        Multi_Managers.UI.ShowClickRockWaringText(msg);
        Time.timeScale = 0;
        StartCoroutine(Co_AfterReturn());
    }

    IEnumerator Co_AfterReturn()
    {
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1;
        PhotonNetwork.LeaveRoom();
        Multi_Managers.Scene.LoadScene(SceneTyep.클라이언트);
    }
}
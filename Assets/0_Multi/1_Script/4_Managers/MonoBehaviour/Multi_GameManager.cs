using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Multi_GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] int gold;
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            Multi_UIManager.instance.UpdateGoldText(gold);
        }
    }

    [SerializeField] int food;
    public int Food
    {
        get => food;
        set
        {
            food = value;
            Multi_UIManager.instance.UpdateFoodText(food);
        }
    }

    public int StartGold;
    public int StartFood;
    private bool isGameover;
    public bool isClear;

    [SerializeField] int _maxEnemyUnitCount;

    [SerializeField] int _maxUninCount;
    public int MaxUnitCount => _maxUninCount;
    public bool UnitOver => Multi_UnitManager.Instance.UnitCount >= _maxUninCount;

    public bool playerEnterStoryMode = false;

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

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        gameManagerAudio = GetComponent<AudioSource>();
    }


    [HideInInspector]
    public bool gameStart;
    public string Difficult { get; private set; }
    public event System.Action OnStart;
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


    [SerializeField] Text diffcultText;
    int stageUpGold = 10;

    void Start()
    {
        Multi_StageManager.Instance.OnUpdateStage += _stage => AddGold(stageUpGold);

        Multi_EnemyManager.Instance.OnEnemyCountChanged += CheckGameOver;

        Multi_SpawnManagers.BossEnemy.OnDead += GetReward;
        Multi_SpawnManagers.TowerEnemy.OnDead += GetReward;

        isGameover = false;
        Gold = 35 + StartGold;
        Food = 20  + StartFood;
        Multi_UIManager.instance.UpdateGoldText(Gold);
        Multi_UIManager.instance.UpdateFoodText(Food);
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

    public void AddGold(int _addGold)
    {
        Gold += _addGold;
        Multi_UIManager.instance.UpdateGoldText(Gold);
    }

    public void AddFood(int _addFood)
    {
        Food += _addFood;
        Multi_UIManager.instance.UpdateFoodText(Food);
    }

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

    int PlusTouchDamege;
    void Update()
    {
        // TODO : 멀티는 승리, 패배 조건을 바꾸어야 함

        if (Input.GetKeyDown(KeyCode.K)) // 빠른 게임 클리어 테스트 용
        {
            if(Time.timeScale == 20f)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 20f;
            }
        }
    }


    public GameObject GameoverUi;
    public Text EndText;

    void CheckGameOver(int enemyCount)
    {
        if(enemyCount >= _maxEnemyUnitCount)
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
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }


}
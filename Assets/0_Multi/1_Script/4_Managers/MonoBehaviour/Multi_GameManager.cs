using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Multi_GameManager : MonoBehaviourPun, IPunObservable
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

    public int Iron;
    public int Wood;
    public int Hammer;
    public int StartGold;
    public int StartFood;
    private bool isGameover;
    public bool isClear;
    public float timer;
    public int waitingTime;

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

        Multi_SpawnManagers.BossEnemy.OnDead += GetReward;
        Multi_SpawnManagers.TowerEnemy.OnDead += GetReward;

        Wood = PlayerPrefs.GetInt("Wood");
        Iron = PlayerPrefs.GetInt("Iron");
        Hammer = PlayerPrefs.GetInt("Hammer");
        StartGold = PlayerPrefs.GetInt("StartGold");
        StartFood = PlayerPrefs.GetInt("StartFood");
        isGameover = false;
        Gold = 35 + StartGold;
        Food = 20  + StartFood;
        PlusTouchDamege = PlayerPrefs.GetInt("PlusTouchDamege");
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
        Chilk();
    }

    // TODO : 멀티는 보상 방식을 바꾸어야함

    //int ClearRewardvalue;
    //void GetClearReward()
    //{
        
    //    switch (currentDifficult)
    //    {
    //        case "Baby": 
    //            Wood += 10; Iron += 10;
    //            ClearRewardvalue = 10;
    //            break;
    //        case "Easy":
    //            Wood += 30; Iron += 30;
    //            ClearRewardvalue = 30;
    //            break;
    //        case "Normal":
    //            Wood += 100; Iron += 100;
    //            ClearRewardvalue = 100;
    //            break;
    //        case "Hard":
    //            Wood += 300; Iron += 300;
    //            ClearRewardvalue = 300;
    //            break;
    //        case "Impossiable":
    //            Wood += 700; Iron += 700;
    //            ClearRewardvalue = 700;
    //            break;
    //        default:
    //            Debug.Log("난이도가 설정되지 않음"); break;
    //    }
        
    //}

    public Queue<GameObject> hitSoliderColor;
    public GameObject HitEnemy;
    public void Chilk()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.transform.gameObject);

                HitEnemy = hit.transform.gameObject;

                NomalEnemy nomalenemy = null;
                if (HitEnemy.transform.childCount > 0 && HitEnemy.transform.GetChild(0).GetComponent<NomalEnemy>() != null)
                    nomalenemy = HitEnemy.transform.GetChild(0).GetComponent<NomalEnemy>();


                if (nomalenemy != null)
                {
                    nomalenemy.currentHp -= (10 + PlusTouchDamege) * (Food + 1);
                    nomalenemy.hpSlider.value = nomalenemy.currentHp;
                    if (nomalenemy.currentHp <= 0)
                    {
                        nomalenemy.Dead();
                    }
                }
            }
        }
    }

    int FasterCount = 0;
    public Text FasterText;
    public float gameTimeSpeed;

    //int FasterAdCount = 0;
    public AdManager adManager;
    public void ClickFasterButton() // 클릭 할때 마다 게임 속도 증가
    {
        SoundManager.instance.PlayEffectSound_ByName("PopSound", 0.6f);

        if (FasterCount == 0)
        {
            FasterText.text = "X2";
            Time.timeScale = 2f;
            gameTimeSpeed = 2f;
            FasterCount += 1;
        }
        else if (FasterCount == 1)
        {
            FasterText.text = "X1";
            Time.timeScale = 1f;
            gameTimeSpeed = 1f;
            FasterCount = 0;
        }
    }

    public GameObject PlayAgainBackGround;
    public GameObject PauseBackGround;

    public void ClickPlayAgainButton()
    {
        SoundManager.instance.PlayEffectSound_ByName("PopSound", 0.6f);

        if (FasterCount == 1)
        {
            Time.timeScale = 2f;
            gameTimeSpeed = 2f;
        }
        else if (FasterCount == 2)
        {
            Time.timeScale = 3f;
            gameTimeSpeed = 3f;
        }
        else if (FasterCount == 0)
        {
            Time.timeScale = 1f;
            gameTimeSpeed = 1f;
        }

        PlayAgainBackGround.SetActive(false);
        PauseBackGround.SetActive(true);
    }

    public GameObject GameoverUi;
    public Text IronRewardText;
    public Text WoodRewardText;
    public Text HammerRewardText;
    public Text EndText;

    // TODO : 멀티 보상 방식 바꿔야함
    public void Lose()
    {
        EndText.text = "Lose!";
        //GetChallengeReward(); 
        adManager.ShowAD();
        isGameover = true;
        //UIManager.instance.SetActiveGameOverUI();
        Time.timeScale = 0;

        SoundManager.instance.PlayEffectSound_ByName("Lose");

        //if (enemySpawn.maxStage == 10000000)
        //{
        //    PlayerPrefs.SetInt("HighScore", HighScore);
        //    Iron += enemySpawn.stageNumber;
        //    Wood += enemySpawn.stageNumber;//보상 방식,,
        //    if (enemySpawn.stageNumber >= 50)
        //    {
        //        Hammer += 10;
        //        HammerRewardText.text = "+ 10";
        //    }
        //    else
        //    {
        //        HammerRewardText.text = "+ 0";
        //    }
        //    IronRewardText.text = "+ " + enemySpawn.stageNumber;
        //    WoodRewardText.text = "+ " + enemySpawn.stageNumber;
        //}
        //else
        //{
        //    IronRewardText.text = "+ 0";
        //    WoodRewardText.text = "+ 0";
        //    HammerRewardText.text = "+ 0";
        //}
        
        GameoverUi.SetActive(true);
        PlayerPrefs.SetInt("Iron", Iron);
        PlayerPrefs.SetInt("Wood", Wood);
        PlayerPrefs.Save();
    }

    // TODO : 멀티니까 Clear가 아니라 Win으로 바꿔야함
    //public void Clear()
    //{
    //    EndText.text = "Clear!";
    //    Hammer += 1;
    //    isClear = true;
    //    for (int i = 0; i < enemySpawn.currentEnemyList.Count; i++)
    //    {
    //        NomalEnemy enemy = enemySpawn.currentEnemyList[i].GetComponent<NomalEnemy>();
    //        enemy.Dead();
    //    }
    //    //UIManager.instance.SetActiveClearUI();
    //    Time.timeScale = 0;
    //    adManager.ShowAD();
    //    SoundManager.instance.PlayEffectSound_ByName("Clear");
    //    GetClearReward();
    //    IronRewardText.text = "+ " + ClearRewardvalue;
    //    WoodRewardText.text = "+ " + ClearRewardvalue;
    //    HammerRewardText.text = "+ 1";
    //    GameoverUi.SetActive(true);
    //    PlayerPrefs.SetInt("Iron", Iron);
    //    PlayerPrefs.SetInt("Wood", Wood);
    //    PlayerPrefs.SetInt("Hammer", Hammer);
    //    PlayerPrefs.Save();
    //}

    public void ReTurnClient()
    {
        Time.timeScale = 1;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Loding.LoadScene("클라이언트");
    }

    public void Reset()
    {
        SoundManager.instance.PlayEffectSound_ByName("PopSound", 0.6f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public GameObject ResetOkButton;
    public void OnResetButton()
    {
        SoundManager.instance.PlayEffectSound_ByName("PopSound", 0.6f);
        ResetOkButton.SetActive(true);
    }

    public void DownResetButton()
    {
        SoundManager.instance.PlayEffectSound_ByName("PopSound", 0.6f);
        ResetOkButton.SetActive(false);
    }

    public void LoadClient()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    [SerializeField] Shop shop;
    public void OnEventShop(int _level, TriggerType _type) => shop.OnShop(_level, _type);

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
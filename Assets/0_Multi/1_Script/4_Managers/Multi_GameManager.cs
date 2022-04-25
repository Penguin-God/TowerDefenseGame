using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Multi_GameManager : MonoBehaviourPun, IPunObservable
{
    //private int Stage;
    public int Gold;
    public int Food;
    public int Iron;
    public int Wood;
    public int Hammer;
    public int StartGold;
    public int StartFood;
    private bool isGameover;
    public bool isClear;
    public float timer;
    public int waitingTime;

    public bool playerEnterStoryMode = false;
    public int enemyCount; // EnemySpaw에 있던거 옮김

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
        OnStart += () => gameStart = true;
    }


    [HideInInspector]
    public bool gameStart;
    public string Difficult { get; private set; }
    public event System.Action OnStart;
    [PunRPC]
    void RPC_OnStart(string _difficult)
    {
        Difficult = _difficult;
        //SelectDifficult(_difficult);
        OnStart();
    }

    public void GameStart(string difficult)
    {
        if (PhotonNetwork.IsMasterClient) photonView.RPC("RPC_OnStart", RpcTarget.All, difficult);
    }


    [SerializeField] Text diffcultText;

    // TODO : 난이도 구현 멀티에 맞게 바꿔야함
    void SelectDifficult(string difficult)
    {
        diffcultText.text = "난이도 : " + difficult;
        //Multi_EnemySpawner.instance.arr_TowersHp = Dic_enemyTowerHp[difficult];
        switch (difficult)
        {
            case "Baby": SetDifficult(30, 10, 200); break;
            case "Easy": SetDifficult(30, 15, 250); break;
            case "Normal": SetDifficult(30, 35, 300); break;
            case "Hard": SetDifficult(35, 70, 350); break;
            case "Impossiable": SetDifficult(120, 250, 1000); break;
        }
    }
    void SetDifficult(int hpWeight, int plusHpWeigh, int minhp)
    {
        Multi_EnemySpawner.instance.enemyHpWeight = hpWeight;
        Multi_EnemySpawner.instance.plusEnemyHpWeight = plusHpWeigh;
        Multi_EnemySpawner.instance.minHp = minhp;
    }

    //public int HighScore;
    int stageUpGold;
    void Start()
    {
        Multi_EnemySpawner.instance.OnStartNewStage += _stage => AddGold(stageUpGold);

        Wood = PlayerPrefs.GetInt("Wood");
        Iron = PlayerPrefs.GetInt("Iron");
        Hammer = PlayerPrefs.GetInt("Hammer");
        StartGold = PlayerPrefs.GetInt("StartGold");
        StartFood = PlayerPrefs.GetInt("StartFood");
        isGameover = false;
        Gold = 15 + StartGold;
        Food = 1  + StartFood;
        PlusTouchDamege = PlayerPrefs.GetInt("PlusTouchDamege");
        Multi_UIManager.instance.UpdateGoldText(Gold);
        Multi_UIManager.instance.UpdateFoodText(Food);
    }

    public void AddGold(int _addGold)
    {
        Gold += _addGold;
        Multi_UIManager.instance.UpdateGoldText(Gold);
    }

    public void AddFood(int _addFood)
    {
        Food += _addFood;
        Multi_UIManager.instance.UpdateFoodText(_addFood);
    }

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

        // 멀티는 챌린지 없음
        //if(enemySpawn.stageNumber > HighScore && isChallenge == true)
        //{
        //    HighScore += 1;
        //    UIManager.instance.UpdateHighScoreText(HighScore);
        //}

        // TODO : 플레이어에 따라서 서로 다른 리스트를 카운트하도록 바꾸어야함
        //enemyCount = Multi_EnemySpawner.instance.currentEnemyList.Count; // 리스트 크기를 enemyCount에 대입
        //UIManager.instance.UpdateCountEnemyText(enemyCount);
        //if (enemyCount >= 50 && !isGameover)
        //{
        //    Lose();
        //    //enemySpaw.EnemyofCount -= 1;
        //}

        //if (isGameover && Input.anyKeyDown)
        //{
        //    ReTurnClient();
        //}

        //if (isClear && Input.anyKeyDown)
        //{
        //    GetClearReward();
        //    ReTurnClient();
        //}

        if (Input.GetKeyDown(KeyCode.K)) // 빠른 게임 클리어 테스트 용
        {
            Time.timeScale = 20f;
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

    // 멀티는 챌린지 없음
    //void GetChallengeReward()
    //{
    //    int reward = Mathf.FloorToInt(enemySpawn.stageNumber / 10);
    //    reward = Mathf.RoundToInt(reward * reward * 1.5f);
    //    Wood += reward; 
    //    Iron += reward;
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
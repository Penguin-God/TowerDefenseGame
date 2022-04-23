using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{

    private static GameManager m_instance;
    ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource { get { return m_instance._resource; } }

    //private int Stage;
    public int Gold;
    public int Food;
    public int Iron;
    public int Wood;
    public int Hammer;
    public int StartGold;
    public int StartFood;
    public EnemySpawn enemySpawn;
    private bool isGameover;
    public bool isClear;
    public float timer;
    public int waitingTime;
    //public Enemy enemy;
    
    
  
    public bool playerEnterStoryMode = false;

    public int enemyCount; // EnemySpaw에 있던거 옮김

    



    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        enemySpawn.GetComponent<EnemySpawn>();
        gameManagerAudio = GetComponent<AudioSource>();

        Set_EnemyTowerHpDictionary();
    }



    public int AddGold;
    public int HighScore;
    private int LastHighScore;
    void Start()
    {
        HighScore = PlayerPrefs.GetInt("HighScore");
        LastHighScore = PlayerPrefs.GetInt("HighScore");
        UIManager.instance.UpdateHighScoreText(LastHighScore);
        Wood = PlayerPrefs.GetInt("Wood");
        Iron = PlayerPrefs.GetInt("Iron");
        Hammer = PlayerPrefs.GetInt("Hammer");
        StartGold = PlayerPrefs.GetInt("StartGold");
        StartFood = PlayerPrefs.GetInt("StartFood");
        isGameover = false;
        Gold = 15 + StartGold;
        Food = 1  + StartFood;
        PlusTouchDamege = PlayerPrefs.GetInt("PlusTouchDamege");
        UIManager.instance.UpdateGoldText(Gold);
        UIManager.instance.UpdateFoodText(Food);
        //adManager.ShowAD();
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
        if(enemySpawn.stageNumber > HighScore && isChallenge == true)
        {
            HighScore += 1;
            UIManager.instance.UpdateHighScoreText(HighScore);
        }
        enemyCount = enemySpawn.currentEnemyList.Count; // 리스트 크기를 enemyCount에 대입
        UIManager.instance.UpdateCountEnemyText(enemyCount);
        if (enemyCount >= 50 && !isGameover)
        {
            Lose();
            //enemySpaw.EnemyofCount -= 1;
        }
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
    int ClearRewardvalue;
    void GetClearReward()
    {
        
        switch (currentDifficult)
        {
            case "Baby": 
                Wood += 10; Iron += 10;
                ClearRewardvalue = 10;
                break;
            case "Easy":
                Wood += 30; Iron += 30;
                ClearRewardvalue = 30;
                break;
            case "Normal":
                Wood += 100; Iron += 100;
                ClearRewardvalue = 100;
                break;
            case "Hard":
                Wood += 300; Iron += 300;
                ClearRewardvalue = 300;
                break;
            case "Impossiable":
                Wood += 700; Iron += 700;
                ClearRewardvalue = 700;
                break;
            default:
                Debug.Log("난이도가 설정되지 않음"); break;
        }
        
    }

    void GetChallengeReward()
    {
        int reward = Mathf.FloorToInt(enemySpawn.stageNumber / 10);
        reward = Mathf.RoundToInt(reward * reward * 1.5f);
        Wood += reward; 
        Iron += reward;
    }

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
                Debug.Log(hit.transform.gameObject);

                HitEnemy = hit.transform.gameObject;

                //Enemy enemy = HitEnemy.GetComponent<Enemy>();


                //if ( enemy != null)
                //{
                    //enemy.currentHp -= 1;
                //}
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

    public void ClickPauseButton()
    {
        SoundManager.instance.PlayEffectSound_ByName("PopSound", 0.6f);

        Time.timeScale = 0f;
        gameTimeSpeed = 0f;

        PauseBackGround.SetActive(false);
        PlayAgainBackGround.SetActive(true);

    }

    public GameObject GameoverUi;
    public Text IronRewardText;
    public Text WoodRewardText;
    public Text HammerRewardText;
    public Text EndText;
    public void Lose()
    {
        EndText.text = "Lose!";
        //GetChallengeReward(); 
        adManager.ShowAD();
        isGameover = true;
        //UIManager.instance.SetActiveGameOverUI();
        Time.timeScale = 0;

        SoundManager.instance.PlayEffectSound_ByName("Lose");
        if (enemySpawn.maxStage == 10000000)
        {
            PlayerPrefs.SetInt("HighScore", HighScore);
            Iron += enemySpawn.stageNumber;
            Wood += enemySpawn.stageNumber;//보상 방식,,
            if (enemySpawn.stageNumber >= 50)
            {
                Hammer += 10;
                HammerRewardText.text = "+ 10";
            }
            else
            {
                HammerRewardText.text = "+ 0";
            }
            IronRewardText.text = "+ " + enemySpawn.stageNumber;
            WoodRewardText.text = "+ " + enemySpawn.stageNumber;
            


        }
        else
        {
            IronRewardText.text = "+ 0";
            WoodRewardText.text = "+ 0";
            HammerRewardText.text = "+ 0";
        }
        
        GameoverUi.SetActive(true);
        PlayerPrefs.SetInt("Iron", Iron);
        PlayerPrefs.SetInt("Wood", Wood);
        PlayerPrefs.Save();
    }

    public void Clear()
    {
        EndText.text = "Clear!";
        Hammer += 1;
        isClear = true;
        for (int i = 0; i < enemySpawn.currentEnemyList.Count; i++)
        {
            NomalEnemy enemy = enemySpawn.currentEnemyList[i].GetComponent<NomalEnemy>();
            enemy.Dead();
        }
        //UIManager.instance.SetActiveClearUI();
        Time.timeScale = 0;
        adManager.ShowAD();
        SoundManager.instance.PlayEffectSound_ByName("Clear");
        GetClearReward();
        IronRewardText.text = "+ " + ClearRewardvalue;
        WoodRewardText.text = "+ " + ClearRewardvalue;
        HammerRewardText.text = "+ 1";
        GameoverUi.SetActive(true);
        PlayerPrefs.SetInt("Iron", Iron);
        PlayerPrefs.SetInt("Wood", Wood);
        PlayerPrefs.SetInt("Hammer", Hammer);
        PlayerPrefs.Save();
    }

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

    [HideInInspector]
    public bool gameStart;
    string currentDifficult;
    public event System.Action OnStart;
    public void GameStart(string difficult)
    {
        gameStart = true;
        UIManager.instance.Set_GameUI();
        //EventManager.instance.RandomUnitEvenet(); // 랜덤 유닛 이벤트

        SelectDifficult(difficult);
        UnitManager.instance.ReSpawnStartUnit();
        OnStart();
    }

    public Text diffcultText;
    public GameObject HighScorePanel;
    public bool isChallenge;

    public void SelectDifficult(string difficult)
    {
        currentDifficult = difficult;
        diffcultText.text = "난이도 : " + difficult;
        enemySpawn.arr_TowersHp = Dic_enemyTowerHp[difficult];
        switch (difficult)
        {
            case "Baby":
                SetDifficult(30, 10, 200);
                break;
            case "Easy":
                SetDifficult(30, 15, 250);
                break;
            case "Normal":
                SetDifficult(30, 35, 300);
                break;
            case "Hard":
                SetDifficult(35, 70, 350);
                break;
            case "Impossiable":
                SetDifficult(120, 250, 1000);
                break;
            case "Challenge":
                SelectChallenge();
                SetDifficult(10, 100, 100);
                break;
            default: 
                Debug.Log("난이도가 설정되지 않음");
                break;
        }
    }
    void SetDifficult(int hpWeight, int plusHpWeigh, int minhp)
    {
        enemySpawn.enemyHpWeight = hpWeight;
        enemySpawn.plusEnemyHpWeight = plusHpWeigh;
        enemySpawn.minHp = minhp;
    }

    void SelectChallenge()
    {
        isChallenge = true;
        HighScorePanel.SetActive(true);
        enemySpawn.maxStage = 10000000;
    }

    private Dictionary<string, int[]> Dic_enemyTowerHp;
    void Set_EnemyTowerHpDictionary() // key : 난이도, value : 레벨 1~6 까지 적군의 성 체력
    {
        Dic_enemyTowerHp = new Dictionary<string, int[]>
        {
            { "Baby", new int[] { 40000, 80000, 300000, 800000, 2000000, 10000000 } },
            { "Easy", new int[] { 80000, 200000, 600000, 2000000, 6000000, 20000000 } },
            { "Normal", new int[] { 600000, 2000000, 6000000, 20000000, 60000000, 100000000 } },
            { "Hard", new int[] { 1000000, 2400000, 8000000, 30000000, 80000000, 300000000 } },
            { "Impossiable", new int[] { 1500000, 4000000, 15000000, 40000000, 140000000, 500000000 } },
            { "Challenge", new int[] { 1000000, 2400000, 8000000, 30000000, 80000000, 300000000 } },
        };
    }
    public void LoadClient()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }






}
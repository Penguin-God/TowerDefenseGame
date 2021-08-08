using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //private int Stage;
    public int Gold;
    public int Food;
    public int Iron;
    public int Wood;
    public EnemySpawn enemySpawn;
    private bool isGameover;
    public bool isClear;
    public float timer;
    public int waitingTime;
    public Enemy enemy;

    public bool playerEnterStoryMode;

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

    private static GameManager m_instance;

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
    void Start()
    {
        isGameover = false;
        Gold = 15;
        Food = 1;
        //Wood += 1;
        //Iron += 1;
        UIManager.instance.UpdateGoldText(Gold);
        UIManager.instance.UpdateFoodText(Food);
        //adManager.ShowAD();
    }

    public AudioClip gameLoseClip;

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
        enemyCount = enemySpawn.currentEnemyList.Count; // 리스트 크기를 enemyCount에 대입
        UIManager.instance.UpdateCountEnemyText(enemyCount);
        if (enemyCount >= 50 && !isGameover)
        {
            Lose();
            //enemySpaw.EnemyofCount -= 1;
        }
        if (isGameover && Input.anyKeyDown)
        {
            ReTurnClient();
        }

        if (isClear && Input.anyKeyDown)
        {
            ReTurnClient();
        }
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
                    nomalenemy.currentHp -= 10 * (Food + 1);
                    nomalenemy.hpSlider.value = nomalenemy.currentHp;
                    if (nomalenemy.currentHp <= 0)
                    {
                        nomalenemy.Dead();
                    }
                }
            }
        }



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

    int FasterAdCount = 0;
    public AdManager adManager;
    public void ClickFasterButton() // 클릭 할때 마다 게임 속도 증가
    {
        //if(FasterAdCount == 0)
        //{
        //    adManager.ShowRewardAd();
        //    FasterAdCount += 1;
        //}
        unitManageButton.UnitManageAudio.Play();
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
        //else if (FasterCount == 1)
        //{
        //    FasterText.text = "X3";
        //    Time.timeScale = 3f;
        //    gameTimeSpeed = 3f;
        //    FasterCount += 1;
        //}
        //else if (FasterCount == 2)
        //{
        //    FasterText.text = "X1";
        //    Time.timeScale = 1f;
        //    gameTimeSpeed = 1f;
        //    FasterCount = 0;
        //}
        //else if (FasterCount == 3)
        //{
        //    Time.timeScale = 1f;
        //    FasterCount = 0;
        //}
    }

    public GameObject PlayAgainBackGround;
    public GameObject PauseBackGround;

    public void ClickPlayAgainButton()
    {
        unitManageButton.UnitManageAudio.Play();

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
        unitManageButton.UnitManageAudio.Play();

        Time.timeScale = 0f;
        gameTimeSpeed = 0f;

        PauseBackGround.SetActive(false);
        PlayAgainBackGround.SetActive(true);

    }

    public GameObject SettingMenu;
    public UnitManageButton unitManageButton;

    public void ClickSettingButton()
    {
        SettingMenu.SetActive(true);
        unitManageButton.UnitManageAudio.Play();
    }



    public void Lose()
    {
        adManager.ShowAD();
        isGameover = true;
        UIManager.instance.SetActiveGameOverUI();
        Time.timeScale = 0;

        gameManagerAudio.PlayOneShot(gameLoseClip);
        PlayerPrefs.SetInt("Iron", Iron);
        PlayerPrefs.SetInt("Wood", Wood);
    }

    public AudioClip clearClip;
    public void Clear()
    {
        adManager.ShowAD();
        isClear = true;
        for (int i = 0; i < enemySpawn.currentEnemyList.Count; i++)
        {
            NomalEnemy enemy = enemySpawn.currentEnemyList[i].GetComponent<NomalEnemy>();
            enemy.Dead();
        }
        UIManager.instance.SetActiveClearUI();
        Time.timeScale = 0;
        gameManagerAudio.PlayOneShot(clearClip, 1.3f);
        PlayerPrefs.SetInt("Iron", Iron);
        PlayerPrefs.SetInt("Wood", Wood);
    }

    public void ReTurnClient()
    {
        Time.timeScale = 1;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Loding.LoadScene("클라이언트");
    }

    public void Reset()
    {
        unitManageButton.UnitManageAudio.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public GameObject ResetOkButton;
    public void OnResetButton()
    {
        unitManageButton.UnitManageAudio.Play();
        ResetOkButton.SetActive(true);
    }

    public void DownResetButton()
    {
        unitManageButton.UnitManageAudio.Play();
        ResetOkButton.SetActive(false);
    }

    [HideInInspector]
    public bool gameStart;
    public void GameStart(string difficult)
    {
        gameStart = true;
        UIManager.instance.Set_GameUI();
        EventManager.instance.RandomUnitEvenet(); // 랜덤 유닛 이벤트

        SelectDifficult(difficult);
        enemySpawn.StageStart();
        enemySpawn.towers[0].SetActive(true);
        UnitManager.instance.ReSpawnStartUnit();
    }

    public Text diffcultText;

    public void SelectDifficult(string difficult)
    {
        diffcultText.text = "난이도 : " + difficult;
        enemySpawn.arr_TowersHp = Dic_enemyTowerHp[difficult];
        switch (difficult)
        {
            case "Baby":
                SetDifficult(20, 5, 200);
                break;
            case "Easy":
                SetDifficult(20, 10, 250);
                break;
            case "Normal":
                SetDifficult(20, 25, 300);
                break;
            case "Hard":
                SetDifficult(25, 45, 350);
                break;
            case "Impossiable":
                SetDifficult(30, 70, 400);
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

    private Dictionary<string, int[]> Dic_enemyTowerHp;
    void Set_EnemyTowerHpDictionary() // key : 난이도, value : 레벨 1~6 까지 적군의 성 체력
    {
        Dic_enemyTowerHp = new Dictionary<string, int[]>
        {
            { "Baby", new int[] { 40000, 80000, 300000, 800000, 2000000, 10000000 } },
            { "Easy", new int[] { 60000, 120000, 400000, 1500000, 4000000, 15000000 } },
            { "Normal", new int[] { 80000, 160000, 600000, 2000000, 6000000, 20000000 } },
            { "Hard", new int[] { 100000, 240000, 800000, 3000000, 8000000, 30000000 } },
            { "Impossiable", new int[] { 150000, 400000, 1500000, 4000000, 14000000, 50000000 } }
        };
    }
    public void LoadClient()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }






}
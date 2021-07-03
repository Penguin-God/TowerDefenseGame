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
    public EnemySpawn enemySpawn;
    private bool isGameover;
    public bool isClear;
    public float timer;
    public int waitingTime;
    //public Starts starts;
    public Enemy enemy;

    public bool playerEnterStoryMode; // 박준 코드
    // public GameObject[] Soldiers;




    //public GameObject target;

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

    //public enum Starts
    //{
    //    Easy,
    //    Normal,
    //    Hard,
    //    Impossiable,
    //}

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        enemySpawn.GetComponent<EnemySpawn>();
        gameManagerAudio = GetComponent<AudioSource>();
    }



    public int AddGold;
    void Start()
    {
        //if (starts == Starts.Easy)s
        //{
            //enemySpawn.enemyHpWeight = 15;
        //}
       // else if (starts == Starts.Normal)
        //{
            //enemySpawn.enemyHpWeight = 25;
        //}
        //else if (starts == Starts.Hard)
        //{
            //enemySpawn.enemyHpWeight = 35;
        //}
        //else if (starts == Starts.Impossiable)
        //{
            //enemySpawn.enemyHpWeight = 45;
        //}
        isGameover = false;
        Gold = 15 + AddGold;
        Food = 1;
        UIManager.instance.UpdateGoldText(Gold);
        UIManager.instance.UpdateFoodText(Food);
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

        //if(Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        //Debug.Log(hit.transform.gameObject);

        //        HitEnemy = hit.transform.gameObject;

        //        NomalEnemy nomalenemy = HitEnemy.transform.GetChild(0).GetComponent<NomalEnemy>();


        //        if ( nomalenemy != null)
        //        {
        //            nomalenemy.currentHp -= 10 * (Food+1);
        //            nomalenemy.hpSlider.value = nomalenemy.currentHp;
        //            if (nomalenemy.currentHp <= 0)
        //            {
        //                nomalenemy.Dead();
        //            }
        //        }
        //    }
        //}



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
            FasterText.text = "X3";
            Time.timeScale = 3f;
            gameTimeSpeed = 3f;
            FasterCount += 1;
        }
        else if (FasterCount == 2)
        {
            FasterText.text = "X1";
            Time.timeScale = 1f;
            gameTimeSpeed = 1f;
            FasterCount = 0;
        }
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
        isGameover = true;
        UIManager.instance.SetActiveGameOverUI();
        Time.timeScale = 0;

        gameManagerAudio.PlayOneShot(gameLoseClip);
    }

    public AudioClip clearClip;
    public void Clear()
    {
        isClear = true;
        for (int i = 0; i < enemySpawn.currentEnemyList.Count; i++)
        {
            NomalEnemy enemy = enemySpawn.currentEnemyList[i].GetComponent<NomalEnemy>();
            enemy.Dead();
        }
        UIManager.instance.SetActiveClearUI();
        Time.timeScale = 0;
        gameManagerAudio.PlayOneShot(clearClip, 1.3f);
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

    [Header("UI GameObject")]
    public GameObject status_UI;
    public GameObject unitControll_UI;
    public GameObject buyUnit_UI;
    //public GameObject reStartButton;
    public GameObject moveFiledButton;
    public GameObject difficult_UI;
    public GameObject event_UI;
    public GameObject setting_UI;
    public GameObject adManager_UI;
    public void GameStart(string difficult)
    {
        Set_UI();
        EventManager.instance.RandomUnitEvenet(); // 랜덤 유닛 이벤트

        SelectDifficult(difficult);
        enemySpawn.StageStart();
        UnitManager.instance.ReSpawnStartUnit();
    }

    public bool gameStart;
    public void Set_UI()
    {
        gameStart = true;
        //Time.timeScale = 0;
        // 키기
        status_UI.SetActive(true);
        unitControll_UI.SetActive(true);
        buyUnit_UI.SetActive(true);
        //reStartButton.SetActive(true);
        moveFiledButton.SetActive(true);
        event_UI.SetActive(true);
        setting_UI.SetActive(true);

        // 끄기
        difficult_UI.SetActive(false);
        adManager_UI.SetActive(false);
    }

    public Text diffcultText;
    public void SelectDifficult(string difficult)
    {
        diffcultText.text = "난이도 : " + difficult;
        switch (difficult)
        {
            case "Baby":
                enemySpawn.enemyHpWeight = 20;
                enemySpawn.plusEnemyHpWeight = 0;
                enemySpawn.minHp = 200;
                break;
            case "Easy":
                enemySpawn.enemyHpWeight = 30;
                enemySpawn.plusEnemyHpWeight = 1;
                enemySpawn.minHp = 250;
                break;
            case "Normal":
                enemySpawn.enemyHpWeight = 30;
                enemySpawn.plusEnemyHpWeight = 2;
                enemySpawn.minHp = 300;
                break;
            case "Hard":
                enemySpawn.enemyHpWeight = 10;
                enemySpawn.plusEnemyHpWeight = 8;
                enemySpawn.minHp = 350;
                break;
            case "Impossiable":
                enemySpawn.enemyHpWeight = 20;
                enemySpawn.plusEnemyHpWeight = 12;
                enemySpawn.minHp = 400;
                break;
            default: 
                Debug.Log("난이도가 설정되지 않음");
                break;
        }
    }

    public void LoadClient()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }






}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int Stage;
    public int Gold;
    public int Food;
    public EnemySpawn enemySpawn;
    private bool isGameover;
    public bool isClear;
    public float timer;
    public int waitingTime;
    public Starts starts;

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

    public enum Starts
    {
        Easy,
        Normal,
        Hard,
        Impossiable,
    }

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        enemySpawn.GetComponent<EnemySpawn>();
    }




    void Start()
    {
        //if (starts == Starts.Easy)
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
        Gold = 25;
        Food = 1;
        UIManager.instance.UpdateGoldText(Gold);
        UIManager.instance.UpdateFoodText(Food);
    }


    void Update()
    {
        enemyCount = enemySpawn.currentEnemyList.Count; // 리스트 크기를 enemyCount에 대입
        UIManager.instance.UpdateCountEnemyText(enemyCount);
        if (enemyCount >= 50)
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



    }

    public Queue<GameObject> hitSoliderColor;
    public GameObject hitSolider;
    public void Chilk()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.transform.gameObject);
                //SoldiersTag();
                hitSolider = hit.transform.gameObject;
            }
        }
    }

    public void Lose()
    {
        isGameover = true;
        UIManager.instance.SetActiveGameOverUI();
        Time.timeScale = 0;
        
    }

    public void Clear()
    {
        isClear = true;
        UIManager.instance.SetActiveClearUI();
        Time.timeScale = 0;

    }

    public void ReTurnClient()
    {
        Time.timeScale = 1;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Loding.LoadScene("클라이언트");
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [Header("UI GameObject")]

    public GameObject status_UI;
    public GameObject unitControll_UI;
    public GameObject buyUnit_UI;
    public GameObject reStartButton;
    public GameObject moveFiledButton;
    public GameObject NaniDo;

    public void GameStart(int enemyHpWeight)
    {
        SelectDifficult(enemyHpWeight);
        enemySpawn.StageStart();

        status_UI.SetActive(true);
        unitControll_UI.SetActive(true);
        buyUnit_UI.SetActive(true);
        reStartButton.SetActive(true);
        moveFiledButton.SetActive(true);
        NaniDo.SetActive(false);
    }

    public void SelectDifficult(int enemyHpWeight)
    {
        enemySpawn.enemyHpWeight = enemyHpWeight;
    }

    public void ClickUengaeButton()
    {
        enemySpawn.enemyHpWeight = 5;
        NaniDo.SetActive(false);

    }
    public void ClickEasyButton()
    {
        enemySpawn.enemyHpWeight = 15;
        NaniDo.SetActive(false);
    
    }

    public void ClickNormalButton()
    {
        enemySpawn.enemyHpWeight = 25;
        NaniDo.SetActive(false);

    }

    public void ClickHardButton()
    {
        enemySpawn.enemyHpWeight = 35;
        NaniDo.SetActive(false);

    }

    public void ClickImpassiableButton()
    {
        enemySpawn.enemyHpWeight = 45;
        NaniDo.SetActive(false);

    }

















}
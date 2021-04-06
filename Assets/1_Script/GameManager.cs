using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int Stage;
    public int Gold;
    public EnemySpaw enemySpaw;
    private bool isGameover;

    //public GameObject target;




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

        enemySpaw.GetComponent<EnemySpaw>();
    }


    void Start()
    {
        isGameover = false;
        Gold = 20;
        UIManager.instance.UpdateGoldText(Gold);
    }


    void Update()
    {
        UIManager.instance.UpdateCountEnemyText(enemySpaw.EnemyofCount);
        if (enemySpaw.EnemyofCount >= 50)
        {
            Lose();
            //enemySpaw.EnemyofCount -= 1;
        }
        if (isGameover && Input.anyKeyDown)
        {
            Restart();
        }

    }

    public GameObject hitSolider;
    public void Chilk()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject);
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

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }






}
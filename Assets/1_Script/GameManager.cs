﻿using System.Collections;
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
    // public GameObject[] Soldiers;

    public GameObject[] RedSwordman;
    public GameObject[] RedArcher;
    public GameObject[] RedSpearman;
    public GameObject[] RedMage;


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
        Gold = 300;
        UIManager.instance.UpdateGoldText(Gold);
    }


    void Update()
    {
        enemyCount = enemySpaw.currentEnemyList.Count; // 리스트 크기를 enemyCount에 대입
        UIManager.instance.UpdateCountEnemyText(enemyCount);
        if (enemyCount >= 50)
        {
            Lose();
            //enemySpaw.EnemyofCount -= 1;
        }
        if (isGameover && Input.anyKeyDown)
        {
            Restart();
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
                SoldiersTag();
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

    public void SoldiersTag()
    {
        RedSwordman = GameObject.FindGameObjectsWithTag("RedSwordman");
        RedArcher = GameObject.FindGameObjectsWithTag("RedArcher");
        RedSpearman = GameObject.FindGameObjectsWithTag("RedSpearman");
        RedMage = GameObject.FindGameObjectsWithTag("RedMage");

    }







}
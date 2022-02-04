﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Multi_EnemySpawner : MonoBehaviourPun
{
    public int stageNumber;
    [SerializeField] int currentEenmyNumber = 0;
    public int maxStage = 50;

    [SerializeField] GameObject[] enemyPrefab; // 0 : 아처, 1 : 마법사, 2 : 창병, 3 : 검사
    [SerializeField] GameObject[] bossPrefab; // 0 : 아처, 1 : 마법사, 2 : 창병, 3 : 검사

    public static Multi_EnemySpawner instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        timerSlider.maxValue = stageTime;
        timerSlider.value = stageTime;

        if (PhotonNetwork.IsMasterClient) Multi_GameManager.instance.OnStart += NewStageStart;
        //Multi_GameManager.instance.OnStart += RespawnTower;
    }

    [ContextMenu("멀티 오브젝트 풀링")]
    void Pooling()
    {
        poolEnemyCount = 51;
        CreatePoolingEnemy(poolEnemyCount, new Vector3(500, 500, 500), hostPool, hostPoolParent, hostCurrnetEnemyList, hostEnemyTurnPoints);
        CreatePoolingEnemy(poolEnemyCount, new Vector3(1000, 1000, 1000), clientPool, clientPoolParent, clientCurrnetEnemyList, clientEnemyTurnPoints);

        respawnEnemyCount = 15;
    }

    int poolEnemyCount; // 풀링을 위해 미리 생성하는 enemy 수
    Queue<GameObject>[] hostPool = new Queue<GameObject>[4];
    Queue<GameObject>[] clientPool = new Queue<GameObject>[4];

    public List<GameObject> hostCurrnetEnemyList = new List<GameObject>();
    public List<GameObject> clientCurrnetEnemyList = new List<GameObject>();

    private int respawnEnemyCount;
    [SerializeField] Transform hostPoolParent = null;
    [SerializeField] Transform clientPoolParent = null;

    [SerializeField] Vector3 hostSpawnPos;
    [SerializeField] Vector3 clientSpawnPos;

    [SerializeField] Transform[] hostEnemyTurnPoints = null;
    [SerializeField] Transform[] clientEnemyTurnPoints = null;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        // 풀링 준비
        poolEnemyCount = 51;
        CreatePoolingEnemy(poolEnemyCount, new Vector3(500, 500, 500), hostPool, hostPoolParent, hostCurrnetEnemyList, hostEnemyTurnPoints);
        CreatePoolingEnemy(poolEnemyCount, new Vector3(1000, 1000, 1000), clientPool, clientPoolParent, clientCurrnetEnemyList, clientEnemyTurnPoints);

        respawnEnemyCount = 15;
    }

    void CreatePoolingEnemy(int count, Vector3 poolPos, Queue<GameObject>[] enemyPools, Transform parent, List<GameObject> currentList, Transform[] turnPoints)
    {
        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            enemyPools[i] = new Queue<GameObject>();
            for (int k = 0; k < count; k++)
            {
                GameObject instantEnemy = PhotonNetwork.Instantiate(enemyPrefab[i].name, poolPos, Quaternion.identity);
                instantEnemy.transform.SetParent(parent); // 자기 자신 자식으로 둠(인스펙터 창에서 보기 편하게 하려고)
                enemyPools[i].Enqueue(instantEnemy);

                // 죽으면 Queue에 반환됨
                Multi_NormalEnemy enemy = instantEnemy.GetComponent<Multi_NormalEnemy>();
                enemy.OnDeath += () => enemyPools[enemy.GetEnemyNumber].Enqueue(instantEnemy);
                enemy.OnDeath += () => currentList.Remove(enemy.gameObject);
                enemy.TurnPoints = turnPoints;
            }
        }

        Debug.Log("Done");
    }

    public int plusEnemyHpWeight;
    public void NewStageStart() // 무한반복하는 재귀 함수( StageCoroutine() 마지막 부분에 다시 StageStart()를 호출함)
    {
        // RPC해야됨
        //Multi_GameManager.instance.Gold += stageGold;
        //UIManager.instance.UpdateGoldText(Multi_GameManager.instance.Gold);
        //enemyHpWeight += plusEnemyHpWeight; // 적 체력 가중치 증가
        //SoundManager.instance.PlayEffectSound_ByName("NewStageClip", 0.6f);
        //timerSlider.maxValue = stageTime;
        //timerSlider.value = stageTime;

        if (!PhotonNetwork.IsMasterClient) return;

        int _enemyNum = Random.Range(0, enemyPrefab.Length);
        int _hp = SetRandomHp();
        float _speed = SetRandomSeepd();


        StartCoroutine(Co_Stage(hostPool[_enemyNum], _hp, _speed, hostSpawnPos, hostCurrnetEnemyList));

        StartCoroutine(Co_Stage(clientPool[_enemyNum], _hp, _speed, clientSpawnPos, clientCurrnetEnemyList));
    }

    void RespawnEnemy(GameObject respawnEnemy, int hp, float speed, Vector3 spawnPos)
    {
        respawnEnemy.GetComponent<Multi_NormalEnemy>().photonView.RPC("SetPos", RpcTarget.All, spawnPos);
        respawnEnemy.GetComponent<Multi_NormalEnemy>().photonView.RPC("SetStatus", RpcTarget.All, hp, speed);
        //respawnEnemy.transform.position = spawnPos;
    }

    IEnumerator Co_Stage(Queue<GameObject> pool, int hp, float speed, Vector3 spawnPos, List<GameObject> addList)
    {
        int respawnCount = respawnEnemyCount;
        if (stageNumber % 10 == 0)
        {
            RespawnBoss();
            respawnCount = 0;
        }

        // normal enemy를 정해진 숫자만큼 소환
        while (respawnCount > 0)
        {
            GameObject respawnEnemy = pool.Dequeue();
            RespawnEnemy(respawnEnemy, hp, speed, spawnPos);
            AddEnemyList(addList, respawnEnemy);
            respawnCount--;
            yield return new WaitForSeconds(2f);
        }

        if (skipButton != null) skipButton.SetActive(true);
        yield return new WaitUntil(() => timerSlider.value <= 0); // 스테이지 타이머 0이되면
        if (skipButton != null) skipButton.SetActive(false);

        stageNumber++;
        //UIManager.instance.UpdateStageText(stageNumber);
        NewStageStart();
    }

    [SerializeField] GameObject skipButton = null;
    public int stageGold;
    public float stageTime = 40f;

    void AddEnemyList(List<GameObject> addList, GameObject enemyObj)
    {
        addList.Add(enemyObj);
        if (addList.Count > 45 && 50 > addList.Count) SoundManager.instance.PlayEffectSound_ByName("Denger", 0.8f);
    }

    public void Skip() // 버튼에서 사용
    {
        timerSlider.value = 0;
    }

    public int queueCount1 = 0;
    public int queueCount2 = 0;
    public int queueCount3 = 0;
    public int queueCount4 = 0;

    [SerializeField] Slider timerSlider;
    private void Update()
    {
        if (Multi_GameManager.instance.gameStart && stageNumber < maxStage)
            timerSlider.value -= Time.deltaTime;
    }


    void SetEnemyData(GameObject enemyObject, int hp, float speed) // enemy 능력치 설정
    {
        Multi_NormalEnemy nomalEnemy = enemyObject.GetComponentInChildren<Multi_NormalEnemy>();
        //nomalEnemy.SetStatus(hp, speed);
    }

    [HideInInspector]
    public int minHp = 0;
    public int enemyHpWeight;

    int SetRandomHp()
    {
        // satge에 따른 가중치 변수들
        int stageHpWeight = stageNumber * stageNumber * enemyHpWeight;

        int hp = minHp + stageHpWeight;
        return hp;
    }

    private float maxSpeed = 5f;
    private float minSpeed = 3f;
    float SetRandomSeepd()
    {
        // satge에 따른 가중치 변수들
        float stageSpeedWeight = stageNumber / 6;

        float enemyMinSpeed = minSpeed + stageSpeedWeight;
        float enemyMaxSpeed = maxSpeed + stageSpeedWeight;
        float speed = Random.Range(enemyMinSpeed, enemyMaxSpeed);
        return speed;
    }

    // 보스 코드
    public bool bossRespawn;
    public int bossLevel;

    // 시발같은 코드
    public int bossRewordGold;
    public int bossRewordFood;

    [HideInInspector]
    public List<EnemyBoss> currentBossList;

    void RespawnBoss()
    {
        bossLevel++;
        bossRespawn = true;
        Multi_GameManager.instance.ChangeBGM(Multi_GameManager.instance.bossbgmClip);

        SetBossStatus();
        UnitManager.instance.UpdateTarget_CurrnetFieldUnit();
    }

    void SetBossStatus()
    {
        int random = Random.Range(0, bossPrefab.Length);
        GameObject instantBoss = Instantiate(bossPrefab[random], bossPrefab[random].transform.position, bossPrefab[random].transform.rotation);
        instantBoss.transform.SetParent(transform);

        int stageWeigh = (stageNumber / 10) * (stageNumber / 10) * (stageNumber / 10);
        int hp = 10000 * (stageWeigh * Mathf.CeilToInt(enemyHpWeight / 15f)); // boss hp 정함
        SetEnemyData(instantBoss, hp, 10);
        instantBoss.transform.position = this.transform.position;
        currentBossList.Add(instantBoss.GetComponentInChildren<EnemyBoss>());
        instantBoss.SetActive(true);

        SetBossDeadAction(instantBoss.GetComponentInChildren<EnemyBoss>());
    }

    void SetBossDeadAction(EnemyBoss boss)
    {
        boss.OnDeath += () => GetBossReword(bossRewordGold, bossRewordFood);
        boss.OnDeath += () => Get_UnitReword();
        boss.OnDeath += () => SoundManager.instance.PlayEffectSound_ByName("BossDeadClip");
        boss.OnDeath += () => Multi_GameManager.instance.ChangeBGM(Multi_GameManager.instance.bgmClip);
        //boss.OnDeath += ClearGame;
        boss.OnDeath += () => SetData(boss);
        boss.OnDeath += () => shop.OnShop(bossLevel, TriggerType.Boss);
        boss.OnDeath += () => UnitManager.instance.UpdateTarget_CurrnetFieldUnit();
    }

    void SetData(EnemyBoss boss)
    {
        currentBossList.Remove(boss);
        bossRespawn = false;
    }

    //void ClearGame()
    //{
    //    if (stageNumber >= maxStage) Multi_GameManager.instance.Clear();
    //}

    void GetBossReword(int rewardGold, int rewardFood)
    {
        Multi_GameManager.instance.Gold += rewardGold * Mathf.FloorToInt(stageNumber / 10);
        UIManager.instance.UpdateGoldText(Multi_GameManager.instance.Gold);

        Multi_GameManager.instance.Food += rewardFood * Mathf.FloorToInt(stageNumber / 10);
        UIManager.instance.UpdateFoodText(Multi_GameManager.instance.Food);
    }

    void Get_UnitReword()
    {
        switch (bossLevel)
        {
            case 1: case 2: createDefenser.CreateSoldier(7, 1); break;
            case 3: case 4: createDefenser.CreateSoldier(7, 2); break;
        }
    }



    // 타워 코드
    public GameObject[] towers;
    [HideInInspector]
    public int[] arr_TowersHp;
    [HideInInspector]
    private int currentTowerLevel = 0;
    public EnemyTower currentTower = null;

    public CreateDefenser createDefenser;
    public Shop shop;

    void RespawnTower()
    {
        // 처음에는 0
        currentTower = towers[currentTowerLevel].GetComponent<EnemyTower>();
        currentTowerLevel = currentTower.level;
        currentTower.Set_RespawnStatus(arr_TowersHp[currentTowerLevel - 1]);

        if (currentTower != null)
        {
            SetDeadAction();
            towers[currentTowerLevel - 1].SetActive(true);
        }
    }

    void SetDeadAction()
    {
        // tower 레밸에 따라 다음 타워 소환할지 안할지 결정
        if (currentTowerLevel < towers.Length) currentTower.OnDeath += () => StartCoroutine(Co_AfterRespawnTower(1.5f));
        else currentTower.OnDeath += () => StartCoroutine(ClearLastTower());

        currentTower.OnDeath += () => SoundManager.instance.PlayEffectSound_ByName("TowerDieClip");
        currentTower.OnDeath += () => shop.OnShop(currentTowerLevel, TriggerType.EnemyTower);
    }

    IEnumerator Co_AfterRespawnTower(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        RespawnTower();
    }

    // 마지막 성 클리어 시
    IEnumerator ClearLastTower() // 검은 창병 두마리 소환 후 모든 유닛 필드로 옮기기
    {
        yield return new WaitForSeconds(0.1f); // 상점 이용 후 유닛이동하기 위해서 대기
        for (int i = 0; i < 2; i++) createDefenser.CreateSoldier(6, 2);
        UnitManager.instance.UnitTranslate_To_EnterStroyMode();
    }

    public Enemy GetRandom_CurrentEnemy(List<GameObject> currentList)
    {
        int index = Random.Range(0, currentList.Count);
        Enemy enemy = currentList[index].GetComponent<Enemy>();
        return enemy;
    }
}
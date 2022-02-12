using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[System.Serializable]
public struct NormalEnemyData
{
    public int number;
    public int hp;
    public float speed;

    public NormalEnemyData(int _number, int _hp, float _speed)
    {
        number = _number;
        hp = _hp;
        speed = _speed;
    }
}

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

        Multi_GameManager.instance.OnStart += NewStageStart;
        if (PhotonNetwork.IsMasterClient) SetNormalEnemyData();
        SetMultiValue();
    }


    [SerializeField] Vector3 spawnPos;
    [SerializeField] Transform[] enemyTurnPoints = null;
    [SerializeField] Transform poolParent = null;
    [SerializeField] Vector3 poolPos;
    void SetMultiValue()
    {
        spawnPos = Multi_Data.instance.EnemySpawnPos;
        enemyTurnPoints = Multi_Data.instance.EnemyTurnPoints;
        poolParent = Multi_Data.instance.EnemyPoolParent;
        poolPos = Vector3.one * ((PhotonNetwork.IsMasterClient) ? 500 : 1000);
    }

    private void Start()
    {
        // 풀링 오브젝트 생성
        poolEnemyCount = 51;
        CreatePoolingEnemy(poolEnemyCount, poolQueues, poolPos, enemyTurnPoints, poolParent);

        respawnEnemyCount = 15;
    }


    int poolEnemyCount; // 풀링을 위해 미리 생성하는 enemy 수
    Queue<GameObject>[] poolQueues = new Queue<GameObject>[4];

    public List<GameObject> currentNormalEnemyList = new List<GameObject>();

    private int respawnEnemyCount;
    void CreatePoolingEnemy(int count, Queue<GameObject>[] enemyPools, Vector3 poolPos, Transform[] turnPoints, Transform parent)
    {
        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            enemyPools[i] = new Queue<GameObject>();
            for (int k = 0; k < count; k++)
            {
                GameObject instantEnemy = PhotonNetwork.Instantiate(enemyPrefab[i].name, poolPos, Quaternion.identity);
                instantEnemy.transform.SetParent(parent); // 자기 자신 자식으로 둠(인스펙터 창에서 보기 편하게 하려고)
                enemyPools[i].Enqueue(instantEnemy);

                Multi_NormalEnemy enemy = instantEnemy.GetComponent<Multi_NormalEnemy>();
                enemy.TurnPoints = turnPoints;

                // 죽으면 Queue에 반환되며 현재 리스트에서 삭제
                enemy.OnDeath += () => enemyPools[enemy.GetEnemyNumber].Enqueue(instantEnemy);
                enemy.OnDeath += () => currentNormalEnemyList.Remove(enemy.gameObject);
            }
        }

        Debug.Log("Done");
    }

    public GameObject GetPoolEnemy(int _num) => poolQueues[_num].Dequeue();


    Dictionary<int, NormalEnemyData> enemyDataDic = new Dictionary<int, NormalEnemyData>();
    void SetNormalEnemyData()
    {
        int maxNumber = enemyPrefab.Length;

        for (int stage = 1; stage < 200; stage++)
        {
            enemyHpWeight += plusEnemyHpWeight;

            int _rand = Random.Range(0, maxNumber);
            int _hp = SetRandomHp(stage, enemyHpWeight);
            float _speed = SetRandomSeepd(stage);
            photonView.RPC("AddEnemyData", RpcTarget.All, stage, _rand, _hp, _speed);
        }
    }

    public List<NormalEnemyData> debugData = new List<NormalEnemyData>();

    [PunRPC]
    void AddEnemyData(int _stage, int _enemyNum, int _hp, float _speed)
    {
        NormalEnemyData _data = new NormalEnemyData(_enemyNum, _hp, _speed);
        enemyDataDic.Add(_stage, _data);

        debugData.Add(_data);
    }

    public int minHp = 0;
    public int enemyHpWeight;
    public int plusEnemyHpWeight;
    int SetRandomHp(int _stage, int _weight)
    {
        // satge에 따른 가중치 변수들
        int stageHpWeight = _stage * _stage * _weight;

        int hp = minHp + stageHpWeight;
        return hp;
    }

    private float maxSpeed = 5f;
    private float minSpeed = 3f;
    float SetRandomSeepd(int _stage)
    {
        // satge에 따른 가중치 변수들
        float stageSpeedWeight = _stage / 6;

        float enemyMinSpeed = minSpeed + stageSpeedWeight;
        float enemyMaxSpeed = maxSpeed + stageSpeedWeight;
        float speed = Random.Range(enemyMinSpeed, enemyMaxSpeed);
        return speed;
    }


    void UpStage(int _addGold, int _upCount = 1)
    {
        stageNumber += _upCount;
        Multi_UIManager.instance.UpdateStageText(stageNumber);

        Multi_GameManager.instance.AddGold(_addGold);
        timerSlider.maxValue = stageTime;
        timerSlider.value = stageTime;

        //SoundManager.instance.PlayEffectSound_ByName("NewStageClip", 0.6f);
    }

    public void NewStageStart() // 무한반복하는 재귀 함수( Co_Stage() 마지막 부분에 다시 NewStageStart()를 호출함)
    {
        UpStage(stageGold);

        currentEenmyNumber = enemyDataDic[stageNumber].number;
        int _hp = enemyDataDic[stageNumber].hp;
        float _speed = enemyDataDic[stageNumber].speed;

        StartCoroutine(Co_Stage(poolQueues[currentEenmyNumber], _hp, _speed, spawnPos));
    }

    IEnumerator Co_Stage(Queue<GameObject> pool, int hp, float speed, Vector3 spawnPos)
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
            respawnCount--;
            yield return new WaitForSeconds(2f);
        }

        if (skipButton != null) skipButton.SetActive(true);
        yield return new WaitUntil(() => timerSlider.value <= 0); // 스테이지 타이머 0이되면
        if (skipButton != null) skipButton.SetActive(false);

        NewStageStart();
    }

    void RespawnEnemy(GameObject respawnEnemy, int hp, float speed, Vector3 spawnPos)
    {
        respawnEnemy.GetComponent<Multi_NormalEnemy>().photonView.RPC("SetPos", RpcTarget.All, spawnPos);
        respawnEnemy.GetComponent<Multi_NormalEnemy>().photonView.RPC("Setup", RpcTarget.All, hp, speed);
        AddCurrentEnemyList(currentNormalEnemyList, respawnEnemy);
    }

    void AddCurrentEnemyList(List<GameObject> addList, GameObject enemyObj)
    {
        addList.Add(enemyObj);
        Multi_UIManager.instance.UpdateCountEnemyText(addList.Count);
        if (addList.Count > 45 && 50 > addList.Count) SoundManager.instance.PlayEffectSound_ByName("Denger", 0.8f);
    }

    [SerializeField] GameObject skipButton = null;
    public int stageGold;
    public float stageTime = 40f;

    public void Skip() // 버튼에서 사용
    {
        timerSlider.value = 0;
    }

    [SerializeField] Slider timerSlider;
    private void Update()
    {
        if (Multi_GameManager.instance.gameStart && stageNumber < maxStage)
            timerSlider.value -= Time.deltaTime;
    }

    // 풀링 디버그용
    //public int queueCount1 = 0;
    //public int queueCount2 = 0;
    //public int queueCount3 = 0;
    //public int queueCount4 = 0;


    public Enemy GetRandom_CurrentEnemy(List<GameObject> currentList)
    {
        int index = Random.Range(0, currentList.Count);
        Enemy enemy = currentList[index].GetComponent<Enemy>();
        return enemy;
    }

    void SetEnemyData(GameObject enemyObject, int hp, float speed) // enemy 능력치 설정
    {
        Multi_NormalEnemy nomalEnemy = enemyObject.GetComponentInChildren<Multi_NormalEnemy>();
        //nomalEnemy.SetStatus(hp, speed);
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
        boss.OnDeath += () => Multi_GameManager.instance.OnEventShop(bossLevel, TriggerType.Boss);
        boss.OnDeath += () => UnitManager.instance.UpdateTarget_CurrnetFieldUnit();
    }

    void SetData(EnemyBoss boss)
    {
        currentBossList.Remove(boss);
        bossRespawn = false;
    }

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

    public CreateDefenser createDefenser;

    // 타워 코드
    [SerializeField] Multi_EnemyTowerSpawner towerSpawner = null;
    public Multi_EnemyTower CurrentTower => towerSpawner.currentTower;
}

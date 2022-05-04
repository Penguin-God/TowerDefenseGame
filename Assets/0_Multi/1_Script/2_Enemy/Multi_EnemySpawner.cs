using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
using Random = UnityEngine.Random;

[Serializable]
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

// 웨이브 관리, enemy spawn
public class Multi_EnemySpawner : MonoBehaviourPun
{
    #region enemy prefab path
    const string PoolGroupName = "Enemys";
    const string NormalPath = "Enemy/Normal";
    const string BossPath = "Enemy/Boss";
    const string TowerPath = "Enemy/Tower";
    string GetJoinPath(string enemyTypePath, string enemyName) => $"{enemyTypePath}/{enemyName}";
    #endregion


    #region events
    public event Action<Multi_NormalEnemy> OnNormalEnemySpawn = null;
    public event Action<Multi_NormalEnemy> OnNormalEnemyDead = null;

    public event Action<Multi_BossEnemy> OnBossSpawn = null;
    public event Action<int> OnBossDead = null;

    public event Action<Multi_EnemyTower> OnTowerSpawn = null;
    public event Action<Multi_EnemyTower> OnTowerDead = null;

    public event Action<int> OnStartNewStage = null;
    #endregion

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

    public int stageNumber;
    [SerializeField] int currentEenmyNumber = 0;
    public int maxStage = 50;

    [SerializeField] GameObject[] enemyPrefab; // 0 : 아처, 1 : 마법사, 2 : 창병, 3 : 검사
    [SerializeField] GameObject[] bossPrefab; // 0 : 아처, 1 : 마법사, 2 : 창병, 3 : 검사

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
        CreatePoolingEnemy(poolEnemyCount, poolPos, enemyTurnPoints);

        respawnEnemyCount = 15;
    }


    [SerializeField] int poolEnemyCount; // 풀링을 위해 미리 생성하는 enemy 수
    Queue<GameObject>[] poolQueues = new Queue<GameObject>[4];

    //public List<GameObject> currentNormalEnemyList = new List<GameObject>();

    private int respawnEnemyCount;
    void CreatePoolingEnemy(int count, Vector3 poolPos, Transform[] turnPoints)
    {
        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            string path = GetJoinPath(NormalPath, enemyPrefab[i].name);
            Transform root = Multi_Managers.Pool.CreatePool(enemyPrefab[i], path, count, PoolGroupName);

            for (int k = 0; k < root.childCount; k++)
            {
                Multi_NormalEnemy enemy = root.GetChild(k).GetComponent<Multi_NormalEnemy>();
                enemy.TurnPoints = turnPoints;
                enemy.OnDeath += () => OnNormalEnemyDead(enemy);
                enemy.OnDeath += () => Multi_Managers.Pool.Push(enemy.gameObject, path);
            }
        }

        //for (int i = 0; i < enemyPrefab.Length; i++)
        //{
        //    enemyPools[i] = new Queue<GameObject>();
        //    for (int k = 0; k < count; k++)
        //    {
        //        GameObject instantEnemy =
        //                PhotonNetwork.Instantiate(GetJoinPath(NormalPath, enemyPrefab[i].name), poolPos, Quaternion.identity);
        //        enemyPools[i].Enqueue(instantEnemy);

        //        Multi_NormalEnemy enemy = instantEnemy.GetComponent<Multi_NormalEnemy>();
        //        enemy.TurnPoints = turnPoints;

        //        // 죽으면 Queue에 반환되며 현재 리스트에서 삭제
        //        enemy.OnDeath += () => OnNormalEnemyDead(enemy);

        //        //enemy.OnDeath += () => enemyPools[enemy.GetEnemyNumber].Enqueue(instantEnemy);
        //        //enemy.OnDeath += () => currentNormalEnemyList.Remove(enemy.gameObject);
        //    }
        //}

        Debug.Log("Done");
    }

    public GameObject GetPoolEnemy(int _num) => poolQueues[_num].Dequeue();


    Dictionary<int, NormalEnemyData> enemyStatusDataByStageNumber = new Dictionary<int, NormalEnemyData>();
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

    [PunRPC] //  RPC에서 사용
    void AddEnemyData(int _stage, int _enemyNum, int _hp, float _speed)
    {
        NormalEnemyData _data = new NormalEnemyData(_enemyNum, _hp, _speed);
        enemyStatusDataByStageNumber.Add(_stage, _data);

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


    void UpdateStage() // 나중에 스테이지를 2개 이상 건너뛰는 기능을 만들지도?
    {
        stageNumber += 1;
        OnStartNewStage?.Invoke(stageNumber);

        timerSlider.maxValue = stageTime;
        timerSlider.value = stageTime;

        //SoundManager.instance.PlayEffectSound_ByName("NewStageClip", 0.6f);
    }

    public void NewStageStart() // 무한반복하는 재귀 함수( Co_Stage() 마지막 부분에 다시 NewStageStart()를 호출함)
    {
        UpdateStage();

        //currentEenmyNumber = enemyStatusDataByStageNumber[stageNumber].number;

        string path = GetJoinPath(NormalPath, enemyPrefab[enemyStatusDataByStageNumber[stageNumber].number].name);
        int _hp = enemyStatusDataByStageNumber[stageNumber].hp;
        float _speed = enemyStatusDataByStageNumber[stageNumber].speed;
        
        StartCoroutine(Co_Stage(path, _hp, _speed, spawnPos));
    }

    IEnumerator Co_Stage(string path, int hp, float speed, Vector3 spawnPos)
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
            RespawnEnemy(path, hp, speed, spawnPos);
            respawnCount--;
            yield return new WaitForSeconds(2f);
        }

        if (skipButton != null) skipButton.SetActive(true);
        yield return new WaitUntil(() => timerSlider.value <= 0); // 스테이지 타이머 0이되면
        if (skipButton != null) skipButton.SetActive(false);

        NewStageStart();
    }

    void RespawnEnemy(string path, int hp, float speed, Vector3 spawnPos)
    {
        Multi_NormalEnemy enemy = Multi_Managers.Resources.PhotonInsantiate(path).GetComponent<Multi_NormalEnemy>();
        Debug.Log(enemy.gameObject.name);
        RPC_Utility.Instance.RPC_Position(enemy.PV.ViewID, spawnPos);
        enemy.SetStatus(RpcTarget.All, hp, speed, false);
        OnNormalEnemySpawn?.Invoke(enemy);
    }

    void AddCurrentEnemyList(List<GameObject> addList, GameObject enemyObj)
    {
        addList.Add(enemyObj);
        // TODO : 멀티 사운드 매니저 이 부분 구현하고 enemyManager에 OnListChanged에 구독하기
        if (addList.Count > 45 && 50 > addList.Count) SoundManager.instance.PlayEffectSound_ByName("Denger", 0.8f);
    }

    [SerializeField] GameObject skipButton = null;
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

    void SetEnemyData(GameObject enemyObject, int hp, float speed) // enemy 능력치 설정
    {
        Multi_NormalEnemy nomalEnemy = enemyObject.GetComponentInChildren<Multi_NormalEnemy>();
        //nomalEnemy.SetStatus(hp, speed);
    }


    // TODO : Boss랑 타워 스폰 구조 바꾸기
    // 보스 코드

    public bool bossRespawn;
    public int bossLevel;

    [HideInInspector]
    public List<Multi_BossEnemy> currentBossList;

    void RespawnBoss()
    {
        bossLevel++;
        bossRespawn = true;
        SetBossStatus();

        Multi_GameManager.instance.ChangeBGM(Multi_GameManager.instance.bossbgmClip);
        UnitManager.instance.UpdateTarget_CurrnetFieldUnit();
    }

    void SetBossStatus()
    {
        GameObject _bossObj = Instantiate(bossPrefab[Random.Range(0, bossPrefab.Length)]);
        Multi_BossEnemy _instantBoss = _bossObj.GetComponent<Multi_BossEnemy>();
        OnBossSpawn?.Invoke(_instantBoss); // 이벤트 실행

        //instantBoss.transform.SetParent(transform);

        int stageWeigh = (stageNumber / 10) * (stageNumber / 10) * (stageNumber / 10);
        int hp = 10000 * (stageWeigh * Mathf.CeilToInt(enemyHpWeight / 15f)); // boss hp 정함
        //SetEnemyData(_instantBoss, hp, 10);

        _instantBoss.transform.position = Multi_Data.instance.EnemySpawnPos;
        _instantBoss.gameObject.SetActive(true);
        //currentBossList.Add(instantBoss.GetComponentInChildren<Multi_BossEnemy>());

        _instantBoss.photonView.RPC("SetPos", RpcTarget.All, spawnPos);
        _instantBoss.photonView.RPC("Setup", RpcTarget.All, hp, 10); // TODO : speed 값 따로 변수 만들기
        SetBossDeadAction(_instantBoss);
    }

    void SetBossDeadAction(Multi_BossEnemy boss)
    {
        boss.OnDeath += () => OnBossDead(boss.Level);
        boss.OnDeath += () => SetData(boss);
    }

    void SetData(Multi_BossEnemy boss)
    {
        currentBossList.Remove(boss);
        bossRespawn = false;
    }

    // 타워 코드
    [SerializeField] Multi_EnemyTowerSpawner towerSpawner = null;
    public Multi_EnemyTower CurrentTower => towerSpawner.currentTower;
}

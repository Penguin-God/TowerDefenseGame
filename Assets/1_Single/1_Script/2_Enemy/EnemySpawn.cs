using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemySpawn : MonoBehaviour
{
    public int stageNumber;
    [SerializeField] int currentEenmyNumber = 0;
    public int maxStage = 50;
    public List<GameObject> currentEnemyList; // 생성된 enemy의 게임 오브젝트가 담겨있음

    private int respawnEnemyCount;

    [SerializeField] GameObject[] enemyPrefab; // 0 : 아처, 1 : 마법사, 2 : 창병, 3 : 검사
    [SerializeField] GameObject[] bossPrefab; // 0 : 아처, 1 : 마법사, 2 : 창병, 3 : 검사

    int poolEnemyCount; // 풀링을 위해 미리 생성하는 enemy 수
    Queue<GameObject>[] arr_DisabledEnemy_Queue;
    Vector3 poolPosition = new Vector3(500, 500, 500);

    public static EnemySpawn instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        timerSlider.maxValue = stageTime;
        timerSlider.value = stageTime;

        GameManager.instance.OnStart += StageStart;
        GameManager.instance.OnStart += RespawnTower;
    }

    private void Start()
    {
        // 게임 관련 변수 설정
        poolEnemyCount = 51;

        arr_DisabledEnemy_Queue = new Queue<GameObject>[enemyPrefab.Length];

        for(int i = 0; i < enemyPrefab.Length; i++)
        {
            arr_DisabledEnemy_Queue[i] = new Queue<GameObject>();
            for (int k = 0; k < poolEnemyCount; k++)
            {
                GameObject instantEnemy = Instantiate(enemyPrefab[i], poolPosition, Quaternion.identity);
                instantEnemy.transform.SetParent(transform); // 자기 자신 자식으로 둠(인스펙터 창에서 보기 편하게 하려고)
                arr_DisabledEnemy_Queue[i].Enqueue(instantEnemy);

                // 죽으면 Queue에 반환됨
                NomalEnemy enemy = instantEnemy.GetComponentInChildren<NomalEnemy>();
                enemy.OnDeath += () => arr_DisabledEnemy_Queue[enemy.GetEnemyNumber].Enqueue(instantEnemy);
            }
        }
        respawnEnemyCount = 15;
    }

    public int plusEnemyHpWeight;
    public void StageStart() // 무한반복하는 재귀 함수( StageCoroutine() 마지막 부분에 다시 StageStart()를 호출함)
    {
        GameManager.instance.Gold += stageGold;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        enemyHpWeight += plusEnemyHpWeight; // 적 체력 가중치 증가
        SoundManager.instance.PlayEffectSound_ByName("NewStageClip", 0.6f);

        StartCoroutine(StageCoroutine(respawnEnemyCount));
    }

    [SerializeField] GameObject skipButton = null;
    public int stageGold;
    public float stageTime = 40f;
    IEnumerator StageCoroutine(int stageRespawnEenemyCount)
    {
        if (stageNumber % 10 == 0)
        {
            RespawnBoss();
            stageRespawnEenemyCount = 0;

            if (stageNumber >= maxStage) // 마지막 보스일시
            {
                UIManager.instance.StageText.text = "현재 스테이지 : Final";
                UIManager.instance.StageText.color = new Color32(255, 0, 0, 255);
                stageRespawnEenemyCount = 10000;
                yield return new WaitForSeconds(5f);
            }
        }

        // 관련 변수 세팅
        currentEenmyNumber = Random.Range(0, enemyPrefab.Length);
        int hp = SetRandomHp();
        float speed = SetRandomSeepd();

        timerSlider.maxValue = stageTime;
        timerSlider.value = stageTime;

        // normal enemy를 정해진 숫자만큼 소환
        while (stageRespawnEenemyCount > 0)
        {
            RespawnEnemy(currentEenmyNumber, hp, speed);
            stageRespawnEenemyCount--;
            yield return new WaitForSeconds(2f);
        }

        if(skipButton != null) skipButton.SetActive(true);
        yield return new WaitUntil(() => timerSlider.value <= 0); // 스테이지 타이머 0이되면
        if (skipButton != null) skipButton.SetActive(false);

        stageNumber++;
        UIManager.instance.UpdateStageText(stageNumber);
        StageStart();
    }

    void RespawnEnemy(int enemyNumber, int hp, float speed)
    {
        GameObject respawnEnemy = arr_DisabledEnemy_Queue[enemyNumber].Dequeue();
        AddEnemyList(respawnEnemy);
        respawnEnemy.GetComponentInChildren<NomalEnemy>().SetStatus(hp, speed);

        respawnEnemy.transform.position = this.transform.position;
        respawnEnemy.SetActive(true);
    }

    void AddEnemyList(GameObject enemyObj)
    {
        currentEnemyList.Add(enemyObj.transform.GetChild(0).gameObject);
        if (currentEnemyList.Count > 45 && 50 > currentEnemyList.Count)
            SoundManager.instance.PlayEffectSound_ByName("Denger", 0.8f);
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
        if(GameManager.instance.gameStart && stageNumber < maxStage)
            timerSlider.value -= Time.deltaTime;

        queueCount1 = arr_DisabledEnemy_Queue[0].Count;
        queueCount2 = arr_DisabledEnemy_Queue[1].Count;
        queueCount3 = arr_DisabledEnemy_Queue[2].Count;
        queueCount4 = arr_DisabledEnemy_Queue[3].Count;
    }


    void SetEnemyData(GameObject enemyObject, int hp, float speed) // enemy 능력치 설정
    {
        NomalEnemy nomalEnemy = enemyObject.GetComponentInChildren<NomalEnemy>();
        nomalEnemy.SetStatus(hp, speed);
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
    public bool BossRespawn { get; private set; } = false;
    [SerializeField] int bossLevel;

    // 시발같은 코드
    public int bossRewordGold;
    public int bossRewordFood;

    [HideInInspector]
    public List<EnemyBoss> currentBossList;

    void RespawnBoss()
    {
        bossLevel++;
        BossRespawn = true;
        GameManager.instance.ChangeBGM(GameManager.instance.bossbgmClip);

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
        boss.OnDeath += () => GameManager.instance.ChangeBGM(GameManager.instance.bgmClip);
        boss.OnDeath += ClearGame; // 보스가 마지막 보스일 때만 Clear하는 건데 이름 좀 바꾸기
        boss.OnDeath += () => SetData(boss);
        boss.OnDeath += () => shop.OnShop(bossLevel, TriggerType.Boss);
        boss.OnDeath += () => UnitManager.instance.UpdateTarget_CurrnetFieldUnit();
    }

    void SetData(EnemyBoss boss)
    {
        currentBossList.Remove(boss);
        if(currentBossList.Count > 0) BossRespawn = false;
    }

    // 이름 좀 바꾸기
    void ClearGame() // 막보스 잡으면 게임 클리어
    {
        if (stageNumber >= maxStage && !GameManager.instance.isChallenge)
            GameManager.instance.Clear();
    }

    void GetBossReword(int rewardGold, int rewardFood)
    {
        GameManager.instance.Gold += rewardGold * Mathf.FloorToInt(stageNumber / 10);
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        GameManager.instance.Food += rewardFood * Mathf.FloorToInt(stageNumber / 10);
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);
    }

    void Get_UnitReword()
    {
        switch (bossLevel)
        {
            case 1: case 2: CombineSoldierPooling.GetObject(UnitManager.instance.GetUnitKey(UnitColor.white, UnitClass.spearman), 7, 1); break;
            case 3: case 4: CombineSoldierPooling.GetObject(UnitManager.instance.GetUnitKey(UnitColor.white, UnitClass.spearman), 7, 2); break;
        }
    }



    // 타워 코드
    public GameObject[] towers;
    [HideInInspector]
    public int[] arr_TowersHp;
    [HideInInspector]
    private int currentTowerLevel = 0;
    public EnemyTower currentTower { get; private set; } = null;

    //public CreateDefenser createDefenser;
    public Shop shop;

    void RespawnTower()
    {
        // 처음에는 0
        currentTower = towers[currentTowerLevel].GetComponent<EnemyTower>();
        currentTowerLevel = currentTower.level;
        currentTower.Set_RespawnStatus(arr_TowersHp[currentTowerLevel - 1]);

        if(currentTower != null)
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
        currentTower.OnDeath += () => currentTower = null;
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
        for (int i = 0; i < 2; i++) 
            CombineSoldierPooling.GetObject(UnitManager.instance.GetUnitKey(UnitColor.black, UnitClass.spearman), 6, 2);

        UnitManager.instance.UnitTranslate_To_EnterStroyMode();
    }

    public Enemy GetRandom_CurrentEnemy()
    {
        int index = Random.Range(0, currentEnemyList.Count);
        Enemy enemy = currentEnemyList[index].GetComponent<Enemy>();
        return enemy;
    }
}

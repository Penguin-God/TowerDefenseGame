using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemySpawn : MonoBehaviour
{
    public int stageNumber;
    public int maxStage = 50;
    public List<GameObject> currentEnemyList; // 생성된 enemy의 게임 오브젝트가 담겨있음

    private int respawnEnemyCount;

    [SerializeField] GameObject[] enemyPrefab; // 0 : 아처, 1 : 마법사, 2 : 창병, 3 : 검사
    [SerializeField] GameObject[] bossPrefab; // 0 : 아처, 1 : 마법사, 2 : 창병, 3 : 검사

    int poolEnemyCount; // 풀링을 위해 미리 생성하는 enemy 수
    Queue<GameObject>[] arr_DisabledEnemy_Queue;
    Vector3 poolPosition = new Vector3(500, 500, 500);

    [HideInInspector]
    public AudioSource enemyAudioSource;
    public AudioClip towerDieClip;

    public static EnemySpawn instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        enemyAudioSource = GetComponent<AudioSource>();
        timerSlider.maxValue = stageTime;
        timerSlider.value = stageTime;
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
        enemyAudioSource.PlayOneShot(newStageClip, 0.6f); // 사운드 재생

        StartCoroutine(StageCoroutine(respawnEnemyCount));
    }

    public AudioClip newStageClip;
    public AudioClip dengerClip;
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
        int instantEnemyNumber = Random.Range(0, enemyPrefab.Length);
        int hp = SetRandomHp();
        float speed = SetRandomSeepd();

        timerSlider.maxValue = stageTime;
        timerSlider.value = stageTime;

        // normal enemy를 정해진 숫자만큼 소환
        while (stageRespawnEenemyCount > 0)
        {
            RespawnEnemy(instantEnemyNumber, hp, speed);
            stageRespawnEenemyCount--;
            yield return new WaitForSeconds(2f);
        }

        yield return new WaitUntil(() => timerSlider.value <= 0); // 스테이지 타이머 0이되면

        stageNumber++;
        UIManager.instance.UpdateStageText(stageNumber);
        StageStart();
    }
    void RespawnEnemy(int enemyNumber, int hp, float speed)
    {
        GameObject respawnEnemy = arr_DisabledEnemy_Queue[enemyNumber].Dequeue();
        respawnEnemy.GetComponentInChildren<NomalEnemy>().SetStatus(hp, speed);

        respawnEnemy.transform.position = this.transform.position;
        respawnEnemy.SetActive(true);
    }

    public void ReturnObject_ByPoolQueue(int num, GameObject obj)
    {
        arr_DisabledEnemy_Queue[num].Enqueue(obj);
    }

    [SerializeField] Slider timerSlider;
    private void Update()
    {
        if(GameManager.instance.gameStart && stageNumber < maxStage)
            timerSlider.value -= Time.deltaTime;
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
    public AudioClip bossDeadClip;
    public bool bossRespawn;
    public int bossLevel;
    public int bossRewordGold;
    public int bossRewordFood;
    [HideInInspector]
    public List<GameObject> currentBossList;
    void RespawnBoss()
    {
        bossLevel++;
        bossRespawn = true;
        int random = Random.Range(0, bossPrefab.Length);
        GameObject instantBoss = Instantiate(bossPrefab[random], bossPrefab[random].transform.position, bossPrefab[random].transform.rotation);
        instantBoss.transform.SetParent(transform);

        int stageWeigh = (stageNumber / 10) * (stageNumber / 10) * (stageNumber / 10);
        int hp = 10000 * (stageWeigh * Mathf.CeilToInt(enemyHpWeight / 15f) ); // boss hp 정함
        SetEnemyData(instantBoss, hp, 10);
        instantBoss.transform.position = this.transform.position;
        instantBoss.SetActive(true);
    }

    // 타워 코드
    public GameObject[] towers;
    [HideInInspector]
    public int[] arr_TowersHp;
    [HideInInspector]
    public int currentTowerLevel = 0;

    public CreateDefenser createDefenser;
    public Shop shop;
    public void RespawnNextTower(int towerLevel, float delayTime)
    {
        // 상점 뜨게 하고 텍스트 설정
        shop.OnShop(towerLevel, shop.towerShopWeighDictionary);
        shop.SetGuideText("적군의 성을 파괴하였습니다");

        enemyAudioSource.PlayOneShot(towerDieClip, 1f);

        currentTowerLevel++;

        if (towerLevel >= towers.Length) // 마지막 성 클리어 시
        {
            StartCoroutine(ClearLastTower());
            return;
        }

        StartCoroutine(SetNexTwoer_Coroutine(currentTowerLevel, delayTime));
    }

    IEnumerator SetNexTwoer_Coroutine(int towerLevel, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        towers[towerLevel].SetActive(true);
    }

    IEnumerator ClearLastTower() // 검은 창병 두마리 소환 후 모든 유닛 필드로 옮기기
    {
        yield return new WaitForSeconds(0.1f); // 상점 이용 후 유닛이동하기 위해서 대기
        for (int i = 0; i < 2; i++)
        {
            createDefenser.CreateSoldier(6, 2);
        }
        UnitManager.instance.UnitTranslate_To_EnterStroyMode();
    }
}

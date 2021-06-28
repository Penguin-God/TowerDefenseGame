using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemySpawn : MonoBehaviour
{
    public int stageNumber;
    public List<GameObject> currentEnemyList; // 생성된 enemy의 게임 오브젝트가 담겨있음

    private int respawnEnemyCount;

    public GameObject[] enemyPrefab; // 0 : 아처, 1 : 마법사, 2 : 창병, 3 : 검사
    public GameObject[] bossPrefab; // 0 : 아처, 1 : 마법사, 2 : 창병, 3 : 검사

    int poolEnemyCount; // 풀링을 위해 미리 생성하는 enemy 수
    GameObject[,] enemyArrays;
    int[] countArray; // 2차원 배열을 사용할 때 2번째 배열의 index가 담긴 배열
    Vector3 poolPosition = new Vector3(500, 500, 500);

    public AudioSource enemyAudioSource;
    public AudioClip towerDieClip;

    private void Awake()
    {
        enemyAudioSource = GetComponent<AudioSource>();
        timerSlider.maxValue = stageTime;
        timerSlider.value = stageTime;
    }

    private void Start()
    {
        // 게임 관련 변수 설정
        poolEnemyCount = 51;
        enemyArrays = new GameObject[enemyPrefab.Length, poolEnemyCount];

        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            for(int k = 0; k < poolEnemyCount; k++)
            {
                GameObject instantEnemy = Instantiate(enemyPrefab[i], poolPosition, Quaternion.identity);
                instantEnemy.transform.SetParent(transform);
                enemyArrays[i, k] = instantEnemy;
            }
        }
        countArray = new int[enemyPrefab.Length];
        respawnEnemyCount = 15;
    }

    public int plusEnemyHpWeight;
    public void StageStart()
    {
        //if (stageNumber == 51)
        //{
        //    GameManager.instance.Clear();
        //    return;
        //}

        GameManager.instance.Gold += stageGold;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        if (stageNumber % 2 == 0) enemyHpWeight += plusEnemyHpWeight;
        StartCoroutine(StageCoroutine(respawnEnemyCount));
    }

    public AudioClip newStageClip;
    public AudioClip dengerClip;
    public int stageGold;
    //public float stageWait_Time;
    public float stageTime = 40f;
    IEnumerator StageCoroutine(int stageRespawnEenemyCount) // 재귀함수 무한반복
    {
        // 사운드 재생
        enemyAudioSource.PlayOneShot(newStageClip, 0.6f);
        if (stageNumber % 10 == 0)
        {
            RespawnBoss();
            stageRespawnEenemyCount = 0;

            if (stageNumber >= 50) // 마지막 보스일시
            {
                UIManager.instance.StageText.text = "현재 스테이지 : 마지막";
                UIManager.instance.StageText.color = new Color32(255, 0, 0, 255);
                stageRespawnEenemyCount = 100;
            }
        }

        // 관련 변수 세팅
        int instantEnemyNumber = Random.Range(0, enemyPrefab.Length);
        int hp = SetRandomHp();
        float speed = SetRandomSeepd();

        timerSlider.maxValue = stageTime;
        timerSlider.value = stageTime;

        while (stageRespawnEenemyCount > 0)
        {
            Check_RespawnEnemyIsDead(instantEnemyNumber);

            // enemy 소환
            GameObject enemy = enemyArrays[instantEnemyNumber, countArray[instantEnemyNumber]];
            SetEnemyData(enemy, hp, speed);
            RespawnEnemy(instantEnemyNumber);

            // 변수 설정
            countArray[instantEnemyNumber]++;
            stageRespawnEenemyCount--;

            ResetEnemyCount(instantEnemyNumber);
            yield return new WaitForSeconds(2f);
        }

        //yield return new WaitForSeconds(stageWait_Time);
        yield return new WaitUntil(() => timerSlider.value <= 0); // 스테이지 타이머 0이되면
        //timerSlider.value = stageTime;
        //stageWait_Time = 10f;

        stageNumber++;
        UIManager.instance.UpdateStageText(stageNumber);
        StageStart();
    }

    public Slider timerSlider;
    private void Update()
    {
        if(GameManager.instance.gameStart && stageNumber < 50)
            timerSlider.value -= Time.deltaTime;
    }

    public AudioClip bossDeadClip;
    public bool bossRespawn;
    public int bossLevel;
    public int bossRewordGold;
    public int bossRewordFood;
    public List<GameObject> currentBossList;
    void RespawnBoss()
    {
        bossLevel++;
        bossRespawn = true;
        int random = Random.Range(0, bossPrefab.Length);
        GameObject instantBoss = Instantiate(bossPrefab[random], bossPrefab[random].transform.position, bossPrefab[random].transform.rotation);
        instantBoss.transform.SetParent(transform);

        int hp = 10000 * ( (stageNumber / 10) * (stageNumber / 10) * Mathf.CeilToInt(enemyHpWeight / 10f) ); // boss hp 정함
        SetEnemyData(instantBoss, hp, 10);
        instantBoss.transform.position = this.transform.position;
        instantBoss.SetActive(true);
    }

    GameObject RespawnEnemy(int instantEnemyNumber)
    {
        
        GameObject enemy = enemyArrays[instantEnemyNumber, countArray[instantEnemyNumber]];
        enemy.transform.position = this.transform.position;
        enemy.SetActive(true);
        return enemy;
    }

    void SetEnemyData(GameObject enemyObject, int hp, float speed) // enemy 능력치 설정
    {
        NomalEnemy nomalEnemy = enemyObject.GetComponentInChildren<NomalEnemy>();
        nomalEnemy.ResetStatus(hp, speed);
    }


    void Check_RespawnEnemyIsDead(int instantEnemyNumber) // 풀링할 때 끌어쓸 enemy가 살아있는지 확인후 살아있으면 index++
    {
        for (int i = 0; i < poolEnemyCount; i++)
        {
            NomalEnemy identfyEnemy = enemyArrays[instantEnemyNumber, countArray[instantEnemyNumber]].GetComponentInChildren<NomalEnemy>();
            if (identfyEnemy.isDead) break;
            countArray[instantEnemyNumber]++;
            ResetEnemyCount(instantEnemyNumber);
        }
    }

    public int minHp = 200;
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

    void ResetEnemyCount(int enemyNumber) // 풀링 배열 index의 range가 오버되면 0으로 초기화
    {
        if (countArray[enemyNumber] > poolEnemyCount - 1) countArray[enemyNumber] = 0;
    }


    // 타워 코드
    public GameObject[] towers;
    public int currentTowerLevel;

    [SerializeField]
    public CreateDefenser createDefenser;
    public Shop shop;
    public void RespawnNextTower(int towerLevel, float delayTime)
    {
        // 상점 뜨게 하고 텍스트 설정
        shop.OnShop(towerLevel, shop.towerShopWeighDictionary);
        shop.SetGuideText("적군의 성을 파괴하였습니다");

        enemyAudioSource.PlayOneShot(towerDieClip, 1f);

        currentTowerLevel++;
        if (towerLevel >= towers.Length) // 마지막 성 보상
        {
            for(int i = 0; i < 2; i++)
            {
                createDefenser.CreateSoldier(6, 2);
            }
            UnitManager.instance.UnitTranslate_To_EnterStroyMode();
            return;
        }

        StartCoroutine(SetNexTwoer_Coroutine(towerLevel, delayTime));;
    }

    IEnumerator SetNexTwoer_Coroutine(int towerLevel, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        towers[towerLevel].SetActive(true);
    }
}

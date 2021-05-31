using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    AudioSource enemyAudioSource;
    public AudioClip towerDieClip;

    private void Awake()
    {
        enemyAudioSource = GetComponent<AudioSource>();
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

    public void StageStart()
    {
        if (stageNumber == 51)
        {
            GameManager.instance.Clear();
            return;
        }

        UIManager.instance.UpdateStageText(stageNumber);
        StartCoroutine(StageCoroutine(respawnEnemyCount));
    }


    public float stageWait_Time = 10f;
    IEnumerator StageCoroutine(int stageRespawnEenemyCount) // 재귀함수 무한반복
    {
        if (stageNumber % 10 == 0)
        {
            RespawnBoss();
            stageRespawnEenemyCount = 0;
            stageWait_Time = 40f;
        }

        // 관련 변수 세팅
        int instantEnemyNumber = Random.Range(0, enemyPrefab.Length);
        int hp = SetRandomHp();
        float speed = SetRandomSeepd();
        //float respawnDelayTime = SetRandom_RespawnDelayTime();

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

        stageNumber += 1;
        yield return new WaitForSeconds(stageWait_Time);
        stageWait_Time = 10f;
        StageStart();
    }

    public bool bossRespawn;
    public List<GameObject> currentBossList;
    void RespawnBoss()
    {
        bossRespawn = true;
        int random = Random.Range(0, bossPrefab.Length);
        GameObject instantBoss = Instantiate(bossPrefab[random], bossPrefab[random].transform.position, bossPrefab[random].transform.rotation);
        instantBoss.transform.SetParent(transform);
        //currentEnemyList.Add(instantBoss.transform.GetChild(0).gameObject);
        int hp = 10000 * ( (stageNumber / 10) * (enemyHpWeight / 5) ); // boss hp 정함
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

    private int maxHp = 50;
    private int minHp = 200;
    public int enemyHpWeight;
    int SetRandomHp()
    {
        // satge에 따른 가중치 변수들
        int stageHpWeight = stageNumber * stageNumber * enemyHpWeight;

        int enemyMinHp = minHp + stageHpWeight;
        int enemyMaxHp = maxHp + (stageHpWeight * 2);
        int hp = Random.Range(enemyMinHp, enemyMaxHp);
        return hp;
    }

    private float maxSpeed = 10f;
    private float minSpeed = 5f;
    float SetRandomSeepd()
    {
        // satge에 따른 가중치 변수들
        float stageSpeedWeight = stageNumber / 2;

        float enemyMinSpeed = minSpeed + stageSpeedWeight;
        float enemyMaxSpeed = maxSpeed + stageSpeedWeight;
        float speed = Random.Range(enemyMinSpeed, enemyMaxSpeed);
        return speed;
    }

    //private float maxRespawnDelayTime = 1f;
    //private float minRespawnDelayTime = 4f;
    //float SetRandom_RespawnDelayTime()
    //{
    //    float delayRime = Random.Range(minRespawnDelayTime, maxRespawnDelayTime);
    //    return delayRime;
    //}

    void ResetEnemyCount(int enemyNumber) // 풀링 배열 index의 range가 오버되면 0으로 초기화
    {
        if (countArray[enemyNumber] > poolEnemyCount - 1) countArray[enemyNumber] = 0;
    }


    // 타워 코드
    public GameObject[] towers;
    public int currentTowerLevel;

    [SerializeField]
    public CreateDefenser createDefenser;
    public void RespawnNextTower(int towerLevel, float delayTime)
    {
        currentTowerLevel++;
        if (towerLevel >= towers.Length)
        { 
            if(towerLevel == towers.Length)
            {
                createDefenser.CreateSoldier(6, 3);
            }
        }
        StartCoroutine(SetNexTwoer_Coroutine(towerLevel, delayTime));
    }

    IEnumerator SetNexTwoer_Coroutine(int towerLevel, float delayTime)
    {
        enemyAudioSource.PlayOneShot(towerDieClip, 1f);
        towers[towerLevel - 1].SetActive(false);
        yield return new WaitForSeconds(delayTime);
        if(towerLevel < towers.Length) towers[towerLevel].SetActive(true);
    }
}

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
    int[] countArray;
    Vector3 poolPosition = new Vector3(500, 500, 500);

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

        // 스테이지 시작
        StageStart();
    }

    void StageStart()
    {
        if (stageNumber == 51)
        {
            GameManager.instance.Clear();
            return;
        }

        UIManager.instance.UpdateStageText(stageNumber);
        StartCoroutine(StageCoroutine(respawnEnemyCount));
    }

    IEnumerator StageCoroutine(int stageRespawnEenemyCount) // 재귀함수 무한반복
    {
        if (stageNumber % 10 == 0)
        {
            RespawnBoss();
            stageRespawnEenemyCount = 0;
        }

        // 관련 변수 세팅
        int instantEnemyNumber = Random.Range(0, enemyPrefab.Length);
        int hp = SetRandomHp();
        float speed = SetRandomSeepd();
        //float respawnDelayTime = SetRandom_RespawnDelayTime();
        
        while (stageRespawnEenemyCount > 0)
        {
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
        yield return new WaitForSeconds(10f);
        StageStart();
    }

    void RespawnBoss()
    {
        int random = Random.Range(0, bossPrefab.Length);
        GameObject instantBoss = Instantiate(bossPrefab[random], bossPrefab[random].transform.position, bossPrefab[random].transform.rotation);
        currentEnemyList.Add(instantBoss);
        int hp = 10000 * (stageNumber / 10); // boss hp 정함
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

    private float maxRespawnDelayTime = 1f;
    private float minRespawnDelayTime = 4f;
    float SetRandom_RespawnDelayTime()
    {
        float delayRime = Random.Range(minRespawnDelayTime, maxRespawnDelayTime);
        return delayRime;
    }

    void ResetEnemyCount(int enemyNumber) // 풀링 배열 index의 range가 오버되면 0으로 초기화
    {
        if (countArray[enemyNumber] > poolEnemyCount - 1) countArray[enemyNumber] = 0;
    }

    public GameObject[] towers;
    public int currentTowerLevel;
    public void SetNextTower(int towerLevel)
    {
        if (towerLevel >= towers.Length) return;

        towers[towerLevel - 1].SetActive(false);
        towers[towerLevel].SetActive(true);
        currentTowerLevel++;
    }
}

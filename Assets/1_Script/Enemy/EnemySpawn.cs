using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawn : MonoBehaviour
{
    private int maxHp = 30;
    private int minHp = 50;
    private float maxSpeed = 10f;
    private float minSpeed = 5f;

    public GameObject[] enemyPrefab; // 0 : 아처, 1 : 마법사, 2 : 창병, 3 : 검사
    public int stageNumber;

    int enemyCount;
    GameObject[,] enemyArrays;
    int[] countArray;
    Vector3 poolPosition = new Vector3(500, 500, 500);

    public List<GameObject> currentEnemyList; // 생성된 enemy의 게임 오브젝트가 담겨있음

    private void Awake()
    {
        // 풀링 관련 변수 설정
        enemyCount = 51;
        enemyArrays = new GameObject[enemyPrefab.Length, enemyCount];

        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            for(int k = 0; k < enemyCount; k++)
            {
                GameObject instantEnemy = Instantiate(enemyPrefab[i], poolPosition, Quaternion.identity);
                instantEnemy.transform.SetParent(transform);
                enemyArrays[i, k] = instantEnemy;
            }
        }
        countArray = new int[enemyPrefab.Length];

        StageStart();
    }

    IEnumerator StageCoroutine(int instantEnemyNumber, int enemyCount, int waitTime) // 재귀함수 무한반복
    {
        int hp = SetRandomHp();
        float speed = SetRandomSeepd();
        while (enemyCount > 0)
        {
            GameObject enemy = enemyArrays[instantEnemyNumber, countArray[instantEnemyNumber]];
            SetEnemyData(enemy, hp, speed);
            RespawnEnemy(instantEnemyNumber);

            //currentEnemyList.Add(enemy);
            countArray[instantEnemyNumber]++;
            enemyCount--;

            ResetEnemyCount(instantEnemyNumber);
            yield return new WaitForSeconds(waitTime);
        }
        stageNumber += 1;
        yield return new WaitForSeconds(3f);
        StageStart();
    }

    GameObject RespawnEnemy(int instantEnemyNumber)
    {
        GameObject enemy = enemyArrays[instantEnemyNumber, countArray[instantEnemyNumber]];
        enemy.transform.position = this.transform.position;
        enemy.SetActive(true);
        return enemy;
    }

    void SetEnemyData(GameObject enemyObject, int hp, float speed) // enemy에 값 부여
    {
        Enemy enemy = enemyObject.GetComponentInChildren<Enemy>();
        enemy.ResetStatus(hp, speed);
    }

    void ResetEnemyCount(int enemyNumber)
    {
        if (countArray[enemyNumber] > enemyCount - 1) countArray[enemyNumber] = 0;
    }

    int[] SetStageData()
    {
        int[] stageData = new int[3];
        stageData[0] = Random.Range(0, 4);
        stageData[1] = Random.Range(5, 16);
        stageData[2] = Random.Range(1, 4);
        return stageData;
    }

    void StageStart()
    {
        UIManager.instance.UpdateStageText(stageNumber);
        int[] stageData = SetStageData();
        StartCoroutine(StageCoroutine(stageData[0], stageData[1], stageData[2]));
    }

    int SetRandomHp()
    {
        // satge에 따른 가중치 변수들
        int stageHpWeight = stageNumber * 2;

        int enemyMinHp = minHp + stageHpWeight;
        int enemyMaxHp = maxHp + stageHpWeight;
        int hp = Random.Range(enemyMinHp, enemyMaxHp);
        return hp;
    }

    float SetRandomSeepd()
    {
        // satge에 따른 가중치 변수들
        float stageSpeedWeight = stageNumber / 2;

        float enemyMinSpeed = minSpeed + stageSpeedWeight;
        float enemyMaxSpeed = maxSpeed + stageSpeedWeight;
        float speed = Random.Range(enemyMinSpeed, enemyMaxSpeed);
        return speed;
    }
}

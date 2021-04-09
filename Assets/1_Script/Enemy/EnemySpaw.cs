using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpaw : MonoBehaviour
{
    private int maxHp = 30;
    private int minHp = 50;
    private float maxSpeed = 10f;
    private float minSpeed = 5f;

    public GameObject[] enemyPrefab; // 0 : 아처, 1 : 마법사, 2 : 창병, 3 : 검사
    public int stageNumber;
    //Dictionary<int, int[]> stageDictionary; // 0 : enemyPrefab에 대입할 수 즉 소환할 enemy, 1 : enemy 수, 2 : 대기 시간

    public List<GameObject> currentEnemyList; // 생성된 enemy의 게임 오브젝트가 담겨있음
    //public int EnemyofCount;

    private void Awake()
    {
        //stageDictionary = new Dictionary<int, int[]>();
        //stageDictionary.Add(1, new int[] { 0, 8, 3});
        //stageDictionary.Add(2, new int[] { 1, 8, 2 });
        //stageDictionary.Add(3, new int[] { 2, 8, 1 });
        //stageDictionary.Add(4, new int[] { 2, 8, 1 });
        //stageDictionary.Add(5, new int[] { 2, 8, 1 });
        //stageDictionary.Add(6, new int[] { 2, 8, 1 });
        //stageDictionary.Add(7, new int[] { 2, 8, 1 });

        StageStart();
    }

    IEnumerator StageCoroutine(int instantEnemyNumber, int enemyCount, int waitTime) // 재귀함수 무한반복
    {
        int hp = SetRandomHp();
        float speed = SetRandomSeepd();
        while (enemyCount > 0)
        {
            //EnemyofCount += 1;
            GameObject enemy = Instantiate(enemyPrefab[instantEnemyNumber], transform.position, transform.rotation);
            SetEnemyData(enemy, hp, speed);
            currentEnemyList.Add(enemy);
            enemyCount--;
            yield return new WaitForSeconds(waitTime);
        }
        stageNumber += 1;
        yield return new WaitForSeconds(3f);
        StageStart();
    }

    int[] SetStageData()
    {
        int[] stageData = new int[3];
        stageData[0] = Random.Range(0, 4);
        stageData[1] = Random.Range(5, 16);
        stageData[2] = Random.Range(2, 6);
        return stageData;
    }

    void StageStart()
    {
        UIManager.instance.UpdateStageText(stageNumber);
        int[] stageData = SetStageData();
        StartCoroutine(StageCoroutine(stageData[0], stageData[1], stageData[2]));
    }

    GameObject SetEnemyData(GameObject enemyObject, int hp, float speed)
    {
        // enemy에 값 부여
        Enemy enemy = enemyObject.GetComponentInChildren<Enemy>();
        enemy.speed = speed;
        enemy.maxHp = hp;
        enemy.currentHp = hp;
        enemy.hpSlider.maxValue = hp;
        enemy.hpSlider.value = hp;
        return enemyObject;
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

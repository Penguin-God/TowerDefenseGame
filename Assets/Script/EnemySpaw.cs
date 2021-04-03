using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaw : MonoBehaviour
{
    public GameObject[] enemyPrefab; // 0 : 아처, 1 : 마법사, 2 : 창병, 3 : 검사
    public int stageNumber;
    //public int startEnemyCount;
    //private int EnemyCount;
    Dictionary<int, int[]> stageDictionary; // 0 : 소환할 enemy, 1 : enemy 수, 2 : 대기 시간 

    private void Awake()
    {
        stageDictionary = new Dictionary<int, int[]>();
        stageDictionary.Add(1, new int[] { 0, 8, 2});
        stageDictionary.Add(2, new int[] { 1, 8, 2 });
        stageDictionary.Add(3, new int[] { 2, 8, 1 });
        NextStage();
    }

    //void Update()
    //{
    //    if (stageNumber + 7 == EnemyCount)
    //    {
    //        stageNumber += 1;
    //        StartCoroutine(StageCoroutine(0, EnemyCount, 2));
    //    }
    //}

    IEnumerator StageCoroutine(int instantEnemyNumber, int enemyCount, int waitTime)
    {
        while (enemyCount > 0)
        {
            GameObject enemy = Instantiate(enemyPrefab[instantEnemyNumber], transform.position, transform.rotation);
            enemyCount--;
            yield return new WaitForSeconds(waitTime);
        }
        stageNumber += 1;
        yield return new WaitForSeconds(3f);
        NextStage();
    }

    int[] SetStageData(int currentStage)
    {
        int[] stageData = new int[3];
        stageData[0] = stageDictionary[currentStage][0];
        stageData[1] = stageDictionary[currentStage][1];
        stageData[2] = stageDictionary[currentStage][2];
        return stageData;
    }

    void NextStage()
    {
        UIManager.instance.UpdateStageText(stageNumber);
        int[] stageData = SetStageData(stageNumber);
        StartCoroutine(StageCoroutine(stageData[0], stageData[1], stageData[2]));
    }
}

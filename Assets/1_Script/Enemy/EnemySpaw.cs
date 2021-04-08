using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpaw : MonoBehaviour
{
    public GameObject[] enemyPrefab; // 0 : 아처, 1 : 마법사, 2 : 창병, 3 : 검사
    public int stageNumber;
    Dictionary<int, int[]> stageDictionary; // 0 : enemyPrefab에 대입할 수 즉 소환할 enemy, 1 : enemy 수, 2 : 대기 시간
    public List<GameObject> currentEnemyList;
    public int EnemyofCount;

    private void Awake()
    {
        stageDictionary = new Dictionary<int, int[]>();
        stageDictionary.Add(1, new int[] { 0, 8, 3});
        stageDictionary.Add(2, new int[] { 1, 8, 2 });
        stageDictionary.Add(3, new int[] { 2, 8, 1 });
        stageDictionary.Add(4, new int[] { 2, 8, 1 });
        stageDictionary.Add(5, new int[] { 2, 8, 1 });
        stageDictionary.Add(6, new int[] { 2, 8, 1 });
        stageDictionary.Add(7, new int[] { 2, 8, 1 });
        StageStart();
    }

    IEnumerator StageCoroutine(int instantEnemyNumber, int enemyCount, int waitTime) // 재귀함수 무한반복
    {
        while (enemyCount > 0)
        {
            GameObject enemy = Instantiate(enemyPrefab[instantEnemyNumber], transform.position, transform.rotation);
            currentEnemyList.Add(enemy);
            EnemyofCount += 1;
            enemyCount--;
            yield return new WaitForSeconds(waitTime);
        }
        stageNumber += 1;
        yield return new WaitForSeconds(3f);
        StageStart();
    }

    int[] SetStageData(int currentStage)
    {
        int[] stageData = new int[3];
        stageData[0] = stageDictionary[currentStage][0];
        stageData[1] = stageDictionary[currentStage][1];
        stageData[2] = stageDictionary[currentStage][2];
        return stageData;
    }

    void StageStart()
    {
        UIManager.instance.UpdateStageText(stageNumber);
        int[] stageData = SetStageData(stageNumber);
        StartCoroutine(StageCoroutine(stageData[0], stageData[1], stageData[2]));
    }
}

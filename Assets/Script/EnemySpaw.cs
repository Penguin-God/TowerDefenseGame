using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaw : MonoBehaviour
{
    public GameObject enemyPrefab;
    private int Stagenumber;
    private int EnemyCount;
    public int startEnemyCount;
    private UIManager UIManager;

    private void Start()
    {
        EnemyCount = startEnemyCount;
        Stagenumber = 1;
        
    }

    private void Update()
    {
        if (Stagenumber + 7 == EnemyCount)
        {
            Stagenumber += 1;
            StartCoroutine(Stage(enemyPrefab, EnemyCount, 2));
        }
    }

    IEnumerator Stage(GameObject instantEnemy, int enemyCount, int waitTime)
    {
        yield return new WaitForSeconds(1f); // 버그 방지용 임시 대기
        UIManager.instance.UpdateStageText(Stagenumber-1);
        
        
        while (enemyCount > 0)
        {
            GameObject enemy = Instantiate(instantEnemy, transform.position, transform.rotation);
            yield return new WaitForSeconds(waitTime);
            enemyCount--;
        }
        if (enemyCount == 0)
        {
            // Stagenumber -= 1;
            EnemyCount += 1;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaw : MonoBehaviour
{
    public GameObject enemyPrefab;

    private void Start()
    {
        StartCoroutine(Stage(enemyPrefab, 8, 2));
    }

    IEnumerator Stage(GameObject instantEnemy, int enemyCount, int waitTime)
    {
        yield return new WaitForSeconds(1f); // 버그 방지용 임시 대기
        while(enemyCount > 0)
        {
            GameObject enemy = Instantiate(instantEnemy, transform.position, transform.rotation);
            yield return new WaitForSeconds(waitTime);
            enemyCount--;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDifficult : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject difficultCanvas;
    public EnemySpawn enemySpawn;

    public void Select_Difficult(int enemyHpWeigh)
    {
        enemySpawn.enemyHpWeight = enemyHpWeigh;
        difficultCanvas.SetActive(false);
        difficultCanvas.SetActive(true);
    }
}

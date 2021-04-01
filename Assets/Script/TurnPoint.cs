using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPoint : MonoBehaviour
{
    static public Transform[] enemyTurnPoints;

    private void Awake()
    {
        enemyTurnPoints = new Transform[transform.childCount];
        for(int i = 0; i < enemyTurnPoints.Length; i++)
        {
            enemyTurnPoints[i] = transform.GetChild(i);
        }
    }
}

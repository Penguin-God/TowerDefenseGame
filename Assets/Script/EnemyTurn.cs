using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : MonoBehaviour
{
    public float speed = 10f;
    public TurnPoint turnPoint;
    
    private Transform target;
    private int pointIndex = 0;
    private Vector3 dir;


    void Start()
    {
        GetNextPoint();
    }

    void Update()
    {
        transform.Translate(dir.normalized * speed * Time.deltaTime);
        if (Vector3.Distance(target.position, this.transform.position) <= 0.5f) GetNextPoint();
    }

    void GetNextPoint()
    {
        if (pointIndex >= turnPoint.enemyTurnPoints.Length) pointIndex = 0; // 무한반복을 위한 조건
        target = turnPoint.enemyTurnPoints[pointIndex];
        dir = target.position - this.transform.position;
        pointIndex++;
    }
}

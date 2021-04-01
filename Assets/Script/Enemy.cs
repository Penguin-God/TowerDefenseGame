using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;
    public float maxHp;
    public float currentHp;
    public bool isDead;

    public TurnPoint turnPoint;
    public Slider hpSlider;
    
    private Transform target;
    private Vector3 dir;
    private int pointIndex = 0;


    void OnEnable()
    {
        isDead = false;
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
    }

    void Start()
    {
        GetNextPoint();
    }

    void Update()
    {
        EnemyMove();
        //hpSlider.value = currentHp;
    }

    void EnemyMove()
    {
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        if (Vector3.Distance(target.position, this.transform.position) <= 0.2f) GetNextPoint();
    }

    void GetNextPoint()
    {
        if (pointIndex >= turnPoint.enemyTurnPoints.Length) pointIndex = 0; // 무한반복을 위한 조건

        transform.rotation = Quaternion.Euler(0, -90 * pointIndex, 0);
        target = turnPoint.enemyTurnPoints[pointIndex];
        dir = target.position - this.transform.position;
        pointIndex++;
    }

    void OnDamage(float damage)
    {
        currentHp -= damage;
        hpSlider.value = currentHp;
    }
}

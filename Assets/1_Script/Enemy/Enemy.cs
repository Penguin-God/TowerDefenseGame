using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // 상태 변수
    public float speed = 10f;
    public float maxHp;
    public float currentHp;
    public bool isDead;
    public Slider hpSlider;

    // 이동, 회전 관련 변수
    private Transform parent;
    private Transform target;
    private Vector3 dir;
    private int pointIndex = -1;


    void OnEnable()
    {
        isDead = false;
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
        SetNextPoint();
    }

    private void Awake()
    {
        parent = transform.parent.GetComponent<Transform>();
    }

    private void Update()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        parent.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
    }

    void SetNextPoint()
    {
        pointIndex++;
        if (pointIndex >= TurnPoint.enemyTurnPoints.Length) pointIndex = 0; // 무한반복을 위한 조건
        target = TurnPoint.enemyTurnPoints[pointIndex];
        dir = target.position - parent.transform.position;
    }

    void SetTransfrom()
    {
        transform.rotation = Quaternion.Euler(0, -90 * pointIndex, 0);
        parent.transform.position = target.position;
    }

    void OnDamage(float damage)
    {
        currentHp -= damage;
        hpSlider.value = currentHp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "WayPoint")
        {
            SetTransfrom();
            SetNextPoint();
        }
    }
}
